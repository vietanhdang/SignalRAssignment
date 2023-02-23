using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages.Cart
{
    
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;
        public CartCRUD? cartCRUD;

        [BindProperty]
        public Models.Order Order { get; set; }
        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to add a product to the cart post request.
        /// </summary>
        /// <param name="productId">The id of the product to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add to the cart.</param>
        public IActionResult OnPostCartProcess([FromBody] CartProcessParam data)
        {
            try
            {
                cartCRUD = new CartCRUD(_context, HttpContext.Session);
                int productId = data.ProductId;
                int quantity = data.Quantity;
                switch (data.Action)
                {
                    case "add":
                        cartCRUD.AddToCart(productId, quantity);
                        break;
                }
                var res = new { success = true, totalQty = cartCRUD.GetTotalQty() };
                return new JsonResult(res);
            }
            catch (Exception e)
            {
                var res = new { success = false, message = e.Message };
                return new JsonResult(res);
            }
        }

        /// <summary>
        /// This method is used to get the cart.
        /// </summary>
        public void OnGet(int id, string handler)
        {
            cartCRUD = new CartCRUD(_context, HttpContext.Session);
            bool isLogged = false;
            if(VaSession.Get<Account>(HttpContext.Session, "Account") != null)
            {
                isLogged = true;
            }
            switch (handler)
            {
                case "increment":
                    cartCRUD.UpdateQuantity(id, isAdd: true);
                    break;
                case "decrement":
                    cartCRUD.UpdateQuantity(id, isRemove: true);
                    break;
                case "delete":
                cartCRUD.RemoveFromCart(id);
                    break;
                case "clear":
                    cartCRUD.ClearCart();
                    break;

            }
            ViewData["Cart"] = cartCRUD.GetCart();
            ViewData["IsLogged"] = isLogged;
        }

        /// <summary>
        /// This method is used to process the order.
        /// </summary>
         public IActionResult OnPost()
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new Exception("Invalid data");
                }
                return RedirectToPage("/feature");
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return Page();
            }
        }
    }
}
