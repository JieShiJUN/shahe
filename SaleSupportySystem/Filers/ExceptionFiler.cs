using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using SaleSupportySystem.Utility.ApiResult;

namespace SaleSupportySystem.Filers
{
    public class ExceptionFiler : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFiler> _logger;

        public ExceptionFiler(ILogger<ExceptionFiler> logger)
        {
            _logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;

            _logger.LogError($"拦截器捕获异常：{context.HttpContext.Request.Path}错误原因：{ex}");
            if (ex is ApiResultHelper apiex)
            {
                context.Result = new ObjectResult(ApiResultHelper.Error(apiex.Message));
            }
            else
            {
                context.Result = new ObjectResult(ApiResultHelper.Error(ex.Message));

            }
            return Task.CompletedTask;
        }
    }
}
