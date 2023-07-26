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

// ����������ע��
builder.Services.AddControllers();
//  ����һ����չ������������Ӧ�ó���ķ��񼯺������Endpoints API Explorer������������������API��Ԫ���ݣ����ṩSwagger UI
builder.Services.AddEndpointsApiExplorer();

#region ����ע��
IOCExtend.AddCustomIOC(builder.Services);
IOCExtend.AddFiltersIOC(builder.Services);
#endregion

#region JWT����
//�����ļ�
IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();

//jwt����ӳ��
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
    options.RequireHttpsMetadata = true; // �Ƿ�Ҫ��HTTPS
    options.SaveToken = true; // �Ƿ񱣴�����

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt.Issuer, // �����ߵ����ƻ���ַ
        ValidAudience = jwt.Audience, // �����ߵ����ƻ���ַ
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)) // ��Կ��ͨ����һ�����ַ���
    };
});

#endregion

#region Swagger����
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CI/CD��Ŀ����",
        Version = "v1",
        Description = "��Ŀ����",
        Contact = new OpenApiContact
        {
            Name = "JieShi",
            Email = "jieshiovo@qq.com",
            Url = new Uri("http://43.136.130.228:8099/jieshi/SaleSupportySystem")
        }
    });
    //Ӧ�ó���xml�ĵ�
    string[] files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (string file in files)
    {
        c.IncludeXmlComments(file, true);
    }

    //Swagger���������һ����Ϊ"Bearer"�İ�ȫ������Security Definition��������ָ��JWT�����֤�����뷽ʽ�� ���̳г�jwt������ui
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "����������������� 'Bearer' Ϊ��ͷ�� JWT ����ֵ����ʽ��Bearer (token)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    //wagger��OpenAPI�淶���ð�ȫҪ��Security Requirement���Ͱ�ȫ������Security Scheme
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

#region ����app�м��
//�����ڿ�����������ʾ��ϸ���쳣��Ϣҳ����м��
app.UseDeveloperExceptionPage();
//Swagger�м������������Swagger���ɵ�JSON�˵㣬���ὫSwagger�ĵ�����Ϊһ��JSON�ļ������ṩ��Swagger UIʹ�á�
app.UseSwagger();
//Swagger�м�������������������Swagger UI�����ṩ��һ�����ӻ����棬�ÿ����߿������������̽���Ͳ���API��
app.UseSwaggerUI(c=>c.SwaggerEndpoint("v1/swagger.json","wsk Core v1"));
//���ڽ�HTTP�����ض���HTTPSЭ����м��
app.UseHttpsRedirection();

app.UseAuthentication();//��֤ ����ͨ��Services.AddAuthenticationע����jwt������Ϊ��֤����
app.UseAuthorization();//��Ȩ ͨ��������jwt������֤�ж��Ƿ���Ȩ��ͨ������[Authorize]
//���ڽ�������·����ӵ�Ӧ�ó����������ܵ��е��м��
app.MapControllers();
#endregion
/*app.Run("https://localhost:7271");*/
app.Run();

static class IOCExtend
{
    /// <summary>
    /// API����ע��
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
    /// ����������ע��
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