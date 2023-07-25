using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using SaleSupportySystem.Utility.ApiResult;
using System.Collections.Generic;
using System.Reflection;

namespace SaleSupportySystem.Filers
{
    public class ResponseFiler:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result!=null)
            {
                if (context.Result is ObjectResult result)
                {
                    if (result.Value is IEnumerable<object> list) // 判断 result.Value 是否可以转化为列表
                    {
                        int rowCount = list.ToList().Count; // 获取 result.Value 列表行数
                        // 进行相应的处理，例如将行数加入响应体中
                        context.Result = new ObjectResult(ApiResultHelper.Success(result?.Value, rowCount));
                    }
                    else
                    {
                        // 如果 result.Value 不是 IList<object> 类型，则无法获取行数，直接返回成功响应体
                        context.Result = new ObjectResult(ApiResultHelper.Success(result?.Value));
                    }
                }
                else if (context.Result is EmptyResult)
                {
                    context.Result = new ObjectResult(ApiResultHelper.Success());
                }
                else
                {
                    context.Result = new ObjectResult(ApiResultHelper.Success());
                }
            }
            else
            {
                context.Result = new ObjectResult(ApiResultHelper.Success());
            }
            base.OnActionExecuted(context);
        }
    }
}
