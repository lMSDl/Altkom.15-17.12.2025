
using Services.Interfaces;
using WebAPI.Controllers.Generic;

namespace WebAPI.Controllers
{
    public class ShoppingListsController : GenericResourceApiController<Models.ShoppingList>
    {
        public ShoppingListsController(IGenericService<Models.ShoppingList> service) : base(service)
        {
        }
    }
}
