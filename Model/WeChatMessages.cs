using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("WeChatMessages")]
    public class WeChatMessages
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Talker { get; set; }

        public string Json { get; set; }

        public long MsgId { get; set; }

        public long Time { get; set; }

        public string? test { get; set; }
    }
}