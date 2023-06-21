using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Hubs;

namespace SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        private IHubContext<ChatHub> _hubContext { get; }

        public HomeController(IHubContext<ChatHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Push(string msg)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", msg);
            return Json("ok");
        }
    }
}
