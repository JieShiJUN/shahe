using IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using SaleSupportySystem.Filers;
using SaleSupportySystem.Utility.Options;
using Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 控制器服务注册
builder.Services.AddControllers();
//  这是一个扩展方法，用于向应用程序的服务集合中添加Endpoints API Explorer服务。它允许我们生成API的元数据，并提供Swagger UI
builder.Services.AddEndpointsApiExplorer();

#region 服务注册
IOCExtend.AddCustomIOC(builder.Services);
IOCExtend.AddFiltersIOC(builder.Services);
#endregion

#region JWT配置
//配置文件
IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();

//jwt数据映射
var jwtsection = configuration.GetSection(nameof(JWTOptions));
//
builder.Services.Configure<JWTOptions>(jwtsection);
var jwt  = jwtsection.Get<JWTOptions>();

builder.Services.AddAuthentication(options =>
{   
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // 是否要求HTTPS
    options.SaveToken = true; // 是否保存令牌

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt.Issuer, // 发行者的名称或网址
        ValidAudience = jwt.Audience, // 接收者的名称或网址
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)) // 密钥，通常是一个长字符串
    };
});

#endregion

#region Swagger配置
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CI/CD项目基构",
        Version = "v1",
        Description = "项目基构",
        Contact = new OpenApiContact
        {
            Name = "JieShi",
            Email = "jieshiovo@qq.com",
            Url = new Uri("http://43.136.130.228:8099/jieshi/SaleSupportySystem")
        }
    });
    //应用程序xml文档
    string[] files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (string file in files)
    {
        c.IncludeXmlComments(file, true);
    }

    //Swagger配置中添加一个名为"Bearer"的安全方案（Security Definition），用于指定JWT身份验证的输入方式。 并继承出jwt输入框的ui
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请在输入框中输入以 'Bearer' 为开头的 JWT 令牌值。格式：Bearer (token)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    //wagger的OpenAPI规范配置安全要求（Security Requirement）和安全方案（Security Scheme
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        Array.Empty<string>()
    }
    });

    c.OrderActionsBy(o => o.RelativePath);


});
#endregion

var app = builder.Build();

#region 配置app中间件
//用于在开发环境下显示详细的异常信息页面的中间件
app.UseDeveloperExceptionPage();
//Swagger中间件：用于配置Swagger生成的JSON端点，它会将Swagger文档生成为一个JSON文件，并提供给Swagger UI使用。
app.UseSwagger();
//Swagger中间件：这个方法用于启用Swagger UI，它提供了一个可视化界面，让开发者可以在浏览器中探索和测试API。
app.UseSwaggerUI(c=>c.SwaggerEndpoint("v1/swagger.json","wsk Core v1"));
//用于将HTTP请求重定向到HTTPS协议的中间件
app.UseHttpsRedirection();

app.UseAuthentication();//认证 上面通过Services.AddAuthentication注册了jwt服务作为验证方案
app.UseAuthorization();//授权 通过对上面jwt进行验证判断是否授权，通过特性[Authorize]
//用于将控制器路由添加到应用程序的请求处理管道中的中间件
app.MapControllers();
#endregion
/*app.Run("https://localhost:7271");*/
app.Run();

static class IOCExtend
{
    /// <summary>
    /// API服务注册
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddFiltersIOC(this IServiceCollection services)
    {
        services.AddMvcCore(options =>
        {
            options.Filters.Add<ResponseFiler>();
            options.Filters.Add<ExceptionFiler>();
        });
        return services;
    }

    /// <summary>
    /// 过滤器服务注册
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomIOC(this IServiceCollection services)
    {
        services.AddScoped<IWeChatMessagesRepository, WeChatMessagesRepository>();
        services.AddScoped<IWeChatMessagesService, WeChatMessagesService>();
        return services;
    }

}