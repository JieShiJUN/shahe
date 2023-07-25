using IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using SaleSupportySystem.Controllers.WeChat;
using SaleSupportySystem.Utility.Options;
using SaleSupportySystem.Utility.Page;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaleSupportySystem.Controllers.Auto
{
    [ApiController]
    [Route("[controller]")]
    public class AutoController : Controller
    {
        private readonly ILogger<AutoController> _logger;
        private readonly IOptions<JWTOptions> _options;
        public AutoController(ILogger<AutoController> logger,IOptions<JWTOptions> options)
        {
            _logger = logger;
            _options = options;
        }
        // GET: AutoController

        [HttpPost("GetToken")]
        public async Task<string> GetToken()
        {
            string token = null;

            #region 获取token
            var jwt = _options.Value;
            //创建了一个包含声明（Claim）的数组 其类型作为权限。值为admin。
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType,"admin")
            };
            //如果 jwt.SecretKey 为 null，则运算符 ! 将阻止编译器报告可能的 Null 引用错误
            var keyBytes = Encoding.UTF8.GetBytes(jwt.SecretKey!);
            //创建一个签名凭据（SigningCredentials），用于对 JSON Web Token (JWT) 进行签名,使用HmacSha256算法进行签名。
            var cred = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                jwt.Issuer,     //签名者
                jwt.Audience,   //接收者
                claims,         //声明（Claim）的数组 其类型作为权限。
                expires: DateTime.Now.AddMinutes(jwt.ExpireMinutes),
                signingCredentials: cred//令牌 签名凭据
                );
            token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            #endregion

            return token;
        }

    }
}
