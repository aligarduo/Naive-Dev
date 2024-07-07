using AutoMapper;
using NaiveDev.Application.Dtos;
using NaiveDev.Domain.Entities;

namespace NaiveDev.Application.Mappings
{
    /// <summary>
    /// 用户映射配置类
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// 初始化用户映射配置
        /// </summary>
        public UserProfile()
        {
            CreateMap<User, GetCurrentUserInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile));
        }
    }
}
