using Models;
using Services.Interfaces;
using WebAPI.Controllers.Generic;

namespace WebAPI.Controllers
{
    public class ProductsController : GenericController<Models.Product>
    {
        public ProductsController(IGenericService<Product> service) : base(service)
        {
        }
    }
}
