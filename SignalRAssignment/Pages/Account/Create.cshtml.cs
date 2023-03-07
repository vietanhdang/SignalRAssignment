using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Account
{
    [StaffPermission]
    public class CreateModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public CreateModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;
        

        
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account.UserName == null || Account.Password == null || Account.FullName == null)
            {
                return Page();
            }

            // check if username already exists
            var account = _context.Accounts.FirstOrDefault(a => a.UserName == Account.UserName);
            if (account != null)
            {
                ModelState.AddModelError(string.Empty, "Username already exists");
                return Page();
            }
            
            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
