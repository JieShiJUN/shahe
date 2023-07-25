using IService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Repository;
using SaleSupportySystem;
using SaleSupportySystem.Filers;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

#region 服务依赖注入
IOCExtend.AddCustomIOC(builder.Services);
IOCExtend.AddFiltersIOC(builder.Services);
#endregion


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebSocket",
        Version = "v1",
        Description = "项目基构"
    });
    var file = Path.Combine(AppContext.BaseDirectory, "SaleSupportySystem.xml");  // xml文档绝对路径
    var path = Path.Combine(AppContext.BaseDirectory, file); // xml文档绝对路径
    c.IncludeXmlComments(path, true);
    c.OrderActionsBy(o => o.RelativePath);

});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c=>c.SwaggerEndpoint("v1/swagger.json","wsk Core v1"));
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

/*app.Run("https://localhost:7271");*/
app.Run();

static class IOCExtend
{
    /// <summary>
    /// 服务注册
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
    /// 过滤器
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