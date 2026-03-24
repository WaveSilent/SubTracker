using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Models;

namespace SubscriptionManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public List<Subscription> Subscriptions { get; set; } = new();
        public decimal TotalMonthly { get; set; }
        public decimal TotalYearly { get; set; }
        public int TotalCount { get; set; }

        public string CurrentSort { get; set; } = "date";

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync(string sortOrder)
        {
            CurrentSort = sortOrder ?? "date";

            var query = _context.Subscriptions
                .Include(s => s.Category)
                .AsQueryable();

            query = CurrentSort switch
            {
                "name" => query.OrderBy(s => s.Name),
                "name_desc" => query.OrderByDescending(s => s.Name),
                "price" => query.OrderBy(s => s.MonthlyPrice),
                "price_desc" => query.OrderByDescending(s => s.MonthlyPrice),
                "date" => query.OrderBy(s => s.BillingDay),
                "date_desc" => query.OrderByDescending(s => s.BillingDay),
                _ => query.OrderBy(s => s.BillingDay)
            };

            Subscriptions = await query.ToListAsync();

            TotalMonthly = Subscriptions.Sum(s => s.MonthlyPrice);
            TotalYearly = TotalMonthly * 12;
            TotalCount = Subscriptions.Count;
        }
    }
}
