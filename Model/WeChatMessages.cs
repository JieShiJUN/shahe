using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    /// <summary>
    /// 微信消息
    /// </summary>
    [Table("WeChatMessages")]
    public class WeChatMessages
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        public string Talker { get; set; }
        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Json { get; set; }
        /// <summary>
        /// MsgId
        /// </summary>
        public long MsgId { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long Time { get; set; }

    }
}