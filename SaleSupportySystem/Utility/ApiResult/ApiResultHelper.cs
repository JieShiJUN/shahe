namespace SaleSupportySystem.Utility.ApiResult
{
    public static class ApiResultHelper
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
    public static ApiResult Success(dynamic data, int total=0)
    {
      return new ApiResult
      {
        Code = 200,
        Data = data,
        Msg = "操作成功",
        Total = total
      };
    }
    public static ApiResult Error(string msg)
    {
      return new ApiResult
      {
        Code = 500,
        Data = null,
        Msg = msg,
        Total = 0
      };
    }
  }
}
