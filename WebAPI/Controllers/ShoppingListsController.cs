
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using WebAPI.Controllers.Generic;

namespace WebAPI.Controllers
{
    public class ShoppingListsController : GenericResourceApiController<Models.ShoppingList>
    {
        public ShoppingListsController(IGenericService<Models.ShoppingList> service) : base(service)
        {
        }

        // Produces - wymusza format odpowiedzi bez względu na nagłówek Accept w żądaniu
        [Produces("application/xml")]
        public override Task<ActionResult<IEnumerable<ShoppingList>>> Get()
        {
            return base.Get();
        }
    }
}
