using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class DbEntitys : DbContext
    {
        /// <summary>
        /// 配置连接字符串，每次访问数据库之前会自动执行此方法，在这里配置连接字符串
        /// 相当于连接前事件
        /// 使用 IOC 注入的方式不实现此方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string ConnString;
#if (DEBUG)
            ConnString = "Data Source=localhost;Initial Catalog=WeChatAssistant;User ID=sa;Password=51475720;TrustServerCertificate=true;";
            builder.UseSqlServer(ConnString, options => options.EnableRetryOnFailure());
#else
           builder.UseSqlServer("Data Source=localhost;Initial Catalog=WeChatAssistant;User ID=sa;Password=51475720;TrustServerCertificate=true;");
#endif

        }

        /// <summary>
        /// 默认构造函数 使用方法与原来一样
        /// </summary>
        public DbEntitys() : base() { }

        /// <summary>
        /// 通过IOC
        /// </summary>
        /// <param name="options"></param>
        public DbEntitys(DbContextOptions<DbEntitys> options) : base(options)
        { }


        #region 表映射
        public virtual DbSet<WeChatMessages> WeChatMessages { get; set; }



        #endregion

    }
}
