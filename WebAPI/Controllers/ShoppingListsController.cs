
using Services.Interfaces;

namespace WebAPI.Controllers
{
    public class ShoppingListsController : GenericController<Models.ShoppingList>
    {
        public ShoppingListsController(IGenericService<Models.ShoppingList> service) : base(service)
        {
        }
    }
}
