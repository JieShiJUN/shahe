using IService;
using Model;
using Repository;

namespace Service
{
    public class WeChatMessagesService : BaseService<WeChatMessages>, IWeChatMessagesService
    {
        private readonly IWeChatMessagesRepository _weChatMessagesRepository;
        public WeChatMessagesService(IWeChatMessagesRepository weChatMessagesRepository)
        {
            base._repository = weChatMessagesRepository;
            _weChatMessagesRepository = weChatMessagesRepository;
        }
    }
}