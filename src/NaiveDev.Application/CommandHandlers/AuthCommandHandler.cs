using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaiveDev.Application.Commands;
using NaiveDev.Application.Dtos;
using NaiveDev.Domain.Entities;
using NaiveDev.Domain.Enums;
using NaiveDev.Infrastructure.Caches;
using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.DataBase;
using NaiveDev.Infrastructure.Enums;
using NaiveDev.Infrastructure.Helpers;
using NaiveDev.Infrastructure.Internet;
using NaiveDev.Infrastructure.Jwt;
using NaiveDev.Infrastructure.Service;
using NaiveDev.Infrastructure.Tools;
using NaiveDev.Shared.Tools;

namespace NaiveDev.Application.CommandHandlers
{
    /// <summary>
    /// 认证命令处理器
    /// </summary>
    public class AuthCommandHandler(ILogger<AuthCommandHandler> logger, ICache cache, IEmailService email, IRepository<User> user) : ServiceBase,
        IRequestHandler<EmailVerifyCommand, ResponseBody>,
        IRequestHandler<SignUpCommand, ResponseBody>,
        IRequestHandler<SignInCommand, ResponseBody<SignInResponseDto>>,
        IRequestHandler<RenewalCommand, ResponseBody<RenewalResponseDto>>,
        IRequestHandler<SignOutCommand, ResponseBody>
    {
        private readonly ILogger<AuthCommandHandler> _logger = logger;
        private readonly ICache _cache = cache;
        private readonly IEmailService _email = email;
        private readonly IRepository<User> _user = user;

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBody> Handle(EmailVerifyCommand request, CancellationToken cancellationToken)
        {
            string verifyCodeTemplate = AssemblyUtils.GetManifestResource("EmailAssets.VerifyCode.html");

            int code = new RandomHelper().RandomGen(4);

            verifyCodeTemplate = verifyCodeTemplate.Replace("{0}", "欢迎注册");
            verifyCodeTemplate = verifyCodeTemplate.Replace("{1}", $"{code}");
            verifyCodeTemplate = verifyCodeTemplate.Replace("{2}", "5");

            string key = CacheKeyUtils.Format(CacheKey.EmailVerifyCode, request.Email);

            // 移除旧的验证码，使其不能再继续使用。再放新的进去。
            await _cache.RemoveCacheAsync(key, cancellationToken);
            await _cache.SetCacheAsync(key, code, TimeSpan.FromMinutes(5), cancellationToken);

            await _email.SendEmailAsync(request.Email, "欢迎注册", verifyCodeTemplate, cancellationToken);

            return ResponseBody.Succeed("验证码已发送到您的邮箱，请注意查收");
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBody> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            bool user = await _user.NoTrackingQuery().AnyAsync(q => q.Email != null && q.Email.Address.Equals(request.Email), cancellationToken);
            if (user)
            {
                return ResponseBody.Fail(ResponseCode.Conflict, "电子邮箱地址已被注册，请使用不同的电子邮箱或登录到您的账户");
            }

            string key = CacheKeyUtils.Format(CacheKey.EmailVerifyCode, request.Email);
            string code = await _cache.GetCacheAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(code) || !request.Code.Equals(code))
            {
                return ResponseBody.Fail(ResponseCode.UnprocessableEntity, "验证码错误，请重新输入或检查您的验证码是否正确");
            }

            User addUser = User.Create(request.NickName, request.Password, request.Email);
            await _user.AddAsync(addUser);

            // 注册完成，验证码马上失效。
            await _cache.RemoveCacheAsync(key, cancellationToken);

            return ResponseBody.Succeed("注册成功，现在您可以登录并开始探索了");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBody<SignInResponseDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            User? user = await _user.NoTrackingQuery().FirstOrDefaultAsync(q => q.Email != null && q.Email.Address.Equals(request.Account) && q.Status == AccountStatus.Active, cancellationToken);
            if (user == null)
            {
                return ResponseBody<SignInResponseDto>.Fail(ResponseCode.Conflict, "账号不存在");
            }

            if (!user.VerifyPassword(request.Password))
            {
                return ResponseBody<SignInResponseDto>.Fail(ResponseCode.UnprocessableEntity, "密码不正确，请重新输入");
            }

            int csrf = new RandomHelper().RandomGen(6);

            string activeKey = CacheKeyUtils.ActiveFormat(UserAgent.Device.Brand, user.Account!);
            string accessTokenKey = CacheKeyUtils.TokenFormat(JwtType.AccessToken, UserAgent.Device.Brand, user.Account!);
            string refreshTokenKey = CacheKeyUtils.TokenFormat(JwtType.RefreshToken, UserAgent.Device.Brand, user.Account!);

            string accessToken = await JwtTokenProvider.BuildTokenAsync(JwtType.AccessToken, UserAgent.Device.Brand, user.Account!, csrf, DateTime.Now.AddHours(2));
            string refreshToken = await JwtTokenProvider.BuildTokenAsync(JwtType.RefreshToken, UserAgent.Device.Brand, user.Account!, csrf, DateTime.Now.AddDays(7));

            await _cache.SetCacheAsync(activeKey, csrf, TimeSpan.FromDays(2), cancellationToken);
            await _cache.SetCacheAsync(accessTokenKey, user, TimeSpan.FromHours(2), cancellationToken);
            await _cache.SetCacheAsync(refreshTokenKey, user, TimeSpan.FromDays(2), cancellationToken);

            SignInResponseDto signInDto = new()
            {
                AccessToken = accessToken,
                ExpiresIn = 7200,
                RefreshToken = refreshToken
            };

            return ResponseBody<SignInResponseDto>.Succeed(signInDto);
        }

        /// <summary>
        /// 续约
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBody<RenewalResponseDto>> Handle(RenewalCommand command, CancellationToken cancellationToken)
        {
            if (!JwtTokenProvider.ValidateTokenAsync(command.RefreshToken ?? "", out JwtSecurityToken? jwtSecurityToken))
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            // 提取令牌中的声明信息
            List<Claim>? claimList = jwtSecurityToken?.Claims.ToList();

            // 尝试从声明列表中获取字段的值
            string? type = claimList?.Where(q => q.Type == "type").FirstOrDefault()?.Value;
            string? client = claimList?.Where(q => q.Type == "client").FirstOrDefault()?.Value;
            string? account = claimList?.Where(q => q.Type == "account").FirstOrDefault()?.Value;
            string? csrf = claimList?.Where(q => q.Type == "csrf").FirstOrDefault()?.Value;

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(client) || string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(csrf))
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            if (!type.Equals(JwtType.RefreshToken.GetDescription()))
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            string activeKey = CacheKeyUtils.ActiveFormat(UserAgent.Device.Brand, account);
            string? cacheActive = await _cache.GetCacheAsync<string>(activeKey, cancellationToken);
            if (cacheActive != csrf)
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            string cacheAccessTokenKey = CacheKeyUtils.TokenFormat(JwtType.AccessToken, UserAgent.Device.Brand, account);
            string cacheRefreshTokenKey = CacheKeyUtils.TokenFormat(JwtType.RefreshToken, UserAgent.Device.Brand, account);
            dynamic? cacheUser = await _cache.GetCacheAsync<dynamic>(cacheRefreshTokenKey, cancellationToken);
            if (cacheUser == null)
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            long userId = cacheUser.Id;
            User? user = await _user.NoTrackingQuery().FirstOrDefaultAsync(q => q.Id == userId && q.Status == AccountStatus.Active, cancellationToken);
            if (user == null)
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            await _cache.RemoveCacheAsync(activeKey, cancellationToken);
            await _cache.RemoveCacheAsync(cacheAccessTokenKey, cancellationToken);
            await _cache.RemoveCacheAsync(cacheRefreshTokenKey, cancellationToken);

            int newCsrf = new RandomHelper().RandomGen(6);

            string accessTokenKey = CacheKeyUtils.TokenFormat(JwtType.AccessToken, UserAgent.Device.Brand, user.Account!);
            string refreshTokenKey = CacheKeyUtils.TokenFormat(JwtType.RefreshToken, UserAgent.Device.Brand, user.Account!);

            string accessToken = await JwtTokenProvider.BuildTokenAsync(JwtType.AccessToken, UserAgent.Device.Brand, user.Account!, newCsrf, DateTime.Now.AddHours(2));
            string refreshToken = await JwtTokenProvider.BuildTokenAsync(JwtType.RefreshToken, UserAgent.Device.Brand, user.Account!, newCsrf, DateTime.Now.AddDays(7));

            await _cache.SetCacheAsync(activeKey, newCsrf, TimeSpan.FromDays(2), cancellationToken);
            await _cache.SetCacheAsync(accessTokenKey, user, TimeSpan.FromHours(2), cancellationToken);
            await _cache.SetCacheAsync(refreshTokenKey, user, TimeSpan.FromDays(2), cancellationToken);

            RenewalResponseDto renewalDto = new()
            {
                AccessToken = accessToken,
                ExpiresIn = 7200,
                RefreshToken = refreshToken
            };

            return ResponseBody<RenewalResponseDto>.Succeed(renewalDto);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBody> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            string activeKey = CacheKeyUtils.ActiveFormat(UserAgent.Device.Brand, Accessor.Account);
            string cacheAccessTokenKey = CacheKeyUtils.TokenFormat(JwtType.AccessToken, UserAgent.Device.Brand, Accessor.Account);
            string cacheRefreshTokenKey = CacheKeyUtils.TokenFormat(JwtType.RefreshToken, UserAgent.Device.Brand, Accessor.Account);

            dynamic? cacheUser = await _cache.GetCacheAsync<dynamic>(cacheAccessTokenKey, cancellationToken);
            if (cacheUser == null)
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            long userId = cacheUser.Id;
            User? user = await _user.NoTrackingQuery().FirstOrDefaultAsync(q => q.Id == userId && q.Status == AccountStatus.Active, cancellationToken);
            if (user == null)
            {
                return ResponseBody<RenewalResponseDto>.Fail(ResponseCode.Forbidden);
            }

            await _cache.RemoveCacheAsync(activeKey, cancellationToken);
            await _cache.RemoveCacheAsync(cacheAccessTokenKey, cancellationToken);
            await _cache.RemoveCacheAsync(cacheRefreshTokenKey, cancellationToken);

            return ResponseBody.Succeed();
        }
    }
}