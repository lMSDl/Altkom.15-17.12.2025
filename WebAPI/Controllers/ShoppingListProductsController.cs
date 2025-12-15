using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using WebAPI.Controllers.Generic;

namespace WebAPI.Controllers
{
    [Route("api/ShoppingLists/{parentId:int}/Products")]
    public class ShoppingListProductsController : GenericNestedResourceApiController<Product, ShoppingList>
    {
        public ShoppingListProductsController(IGenericService<Product> service, IGenericService<ShoppingList> parentService) : base(service, parentService)
        {
        }

        protected override ActionResult<int> CreatedAtAction(int id)
        {
            return CreatedAtAction(nameof(GenericController<Product>.Get), "Products", new { id }, id);
        }

        protected override int GetParentId(Product product)
        {
            return product.ShoppingListId;
        }

        protected override void SetParentId(Product item, int parentId)
        {
            item.ShoppingListId = parentId;
        }
    }
}
