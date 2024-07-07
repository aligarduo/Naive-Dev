using Microsoft.EntityFrameworkCore;
using NaiveDev.Infrastructure.DataBase;
using NaiveDev.Infrastructure.Extensions;
using NaiveDev.Infrastructure.Jwt;
using NaiveDev.Infrastructure.Middleware;
using NaiveDev.Infrastructure.Settings;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.AddAutofac();
builder.Host.AddCache();
builder.Host.UseNLog();
builder.Logging.AddNLog("nlog.config");
builder.Services.AddSetting(configuration);
builder.Services.AddRepository();
builder.Services.AddMemoryCache();
builder.Services.AddController();
builder.Services.AddSwagger();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCorsPolicy();
builder.Services.AddJwt();
builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddMediatR();
builder.Services.AddAutoMapper();
builder.Services.AddApiBehaviorOption();
builder.Services.AddDbContext<DbContext1>(options =>
    options
    .UseMySql(configuration.GetConnectionString("Connection1"), ServerVersion.AutoDetect(configuration.GetConnectionString("Connection1")))
    .EnableDetailedErrors(true)
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggers();
    app.UseDeveloperExceptionPage();
}
app.UseException(app.Environment);
app.UseCorsPolicy();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<RateLimitMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AccessorMiddleware>();
app.MapControllers();

app.Run();