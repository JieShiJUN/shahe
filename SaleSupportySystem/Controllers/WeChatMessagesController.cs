using IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Model;
using SaleSupportySystem.DbUitil;
using SaleSupportySystem.Utility.Page;
using System.Drawing;
using System.Linq;

namespace SaleSupportySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeChatMessagesController : ControllerBase
    {
       
        private readonly ILogger<WeChatMessagesController> _logger;
        private readonly DbEntitys _entitys;
        private readonly IWeChatMessagesService _servic;
        public WeChatMessagesController(ILogger<WeChatMessagesController> logger,IWeChatMessagesService servic)
        {
            _logger = logger;
            _entitys = new DbEntitys();
            _servic = servic;
        }

        /// <summary>
        /// 获取全部微信消息
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllWeChatMessages")]
        public async Task<List<WeChatMessages>> AllWeChatMessages()
        {
            return await _servic.GetAllAsync();
        }

        /// <summary>
        /// 分页获取消息
        /// </summary>
        /// <param name="size">数量</param>
        /// <param name="no">页码</param>
        /// <returns></returns>
        [HttpPost("PageWeChatMessages")]
        public async Task<PagingUtil<WeChatMessages>> PageWeChatMessages(int size, int no)
        {
            var data = await _servic.GetAllAsync();
            return new PagingUtil<WeChatMessages>(data, size, no);
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpPost("SelectPageWeChatMessages")]
        public async Task<PagingUtil<WeChatMessages>> SelectPageWeChatMessages(WeChatMessages model,int size, int no)
        {
            var data = await _servic.QueryAsync(s=>s.Id>model.Id);
            return new PagingUtil<WeChatMessages>(data, size, no);
        }
    }
}