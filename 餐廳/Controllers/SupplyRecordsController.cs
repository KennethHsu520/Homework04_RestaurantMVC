using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 餐廳.Data;

namespace 餐廳.Controllers
{
    public class SupplyRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplyRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupplyRecords
        public async Task<IActionResult> Index()
        {
            var records = await _context.SupplyRecords
                .OrderByDescending(r => r.SupplyDate)
                .ToListAsync();

            return View(records);
        }
    }
}