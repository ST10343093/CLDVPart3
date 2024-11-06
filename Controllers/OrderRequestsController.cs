using CLDVPart3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDVPart3.Controllers
{
    public class OrderRequestsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderRequestsController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var orderRequests = from o in _context.OrderRequests.Include(o => o.Order).Include(o => o.Product)
                                select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                orderRequests = orderRequests.Where(o => o.OrderId.ToString() == searchString);
            }

            return View(await orderRequests.ToListAsync());
        }

        private bool OrderRequestExists(int id)
        {
            return _context.OrderRequests.Any(e => e.OrderRequestId == id);
        }
    }
}
