namespace SaleSupportySystem.Utility.ApiResult
{
    public class ApiResultHelper:Exception
  {
    //成功后返回的数据
    public static ApiResult Success(dynamic data)
    {
      return new ApiResult
      {
        Code = 200,
        Data = data,
        Msg = "操作成功",
        Total = 0
      };
    }
    public static ApiResult Success(dynamic data, int? total=0,string? msg = "操作成功")
    {
      return new ApiResult
      {
        Code = 200,
        Data = data,
        Msg = msg,
        Total = total
      };
    }
    public static ApiResult Error(string msg="未知异常")
    {
      return new ApiResult
      {
        Code = 500,
        Data = null,
        Msg = msg,
        Total = 0
      };
    }

    public static ApiResult Success()
    {
      return new ApiResult
      {
        Code = 200,
        Data = null,
        Msg = "操作成功",
        Total = 0
      };
    }
  }
}
