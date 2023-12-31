using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using SaleSupportySystem.Utility.Page;

namespace SaleSupportySystem.Controllers.WeChat
{
    [ApiController]
    [Route("[controller]")]
    public class WeChatMessagesController : ControllerBase
    {

        private readonly ILogger<WeChatMessagesController> _logger;
        private readonly IWeChatMessagesService _servic;
        public WeChatMessagesController(ILogger<WeChatMessagesController> logger, IWeChatMessagesService servic)
        {
            _logger = logger;
            _servic = servic;
        }

        /// <summary>
        /// 获取全部微信消息
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
        /// <param name="model">微信消息</param>
        /// <param name="size">数量</param>
        /// <param name="no">页码</param>
        /// <returns></returns>
        [HttpPost("SelectPageWeChatMessages")]
        public async Task<PagingUtil<WeChatMessages>> SelectPageWeChatMessages(WeChatMessages model, int size, int no)
        {
            var data = await _servic.QueryAsync(s => s.Time > model.Time);
            return new PagingUtil<WeChatMessages>(data, size, no);
        }
    }
}