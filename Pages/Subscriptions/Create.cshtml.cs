using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Models;

namespace SubscriptionManager.Pages.Subscriptions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subscription Subscription { get; set; } = new();

        public SelectList Categories { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            Categories = new SelectList(categories, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categories = await _context.Categories.ToListAsync();
                Categories = new SelectList(categories, "Id", "Name");
                return Page();
            }

            Subscription.CreatedAt = DateTime.UtcNow;

            _context.Subscriptions.Add(Subscription);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
