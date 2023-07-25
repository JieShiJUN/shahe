namespace SaleSupportySystem.Utility.Options
{
    public class JWTOptions
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; } = null;
        /// <summary>
        /// 签发token者
        /// </summary>
        public string Issuer { get; set; } = null;
        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; } = null;
        /// <summary>
        /// 过期时间：分钟
        /// </summary>
        public int ExpireMinutes { get; set; }
    }
}
