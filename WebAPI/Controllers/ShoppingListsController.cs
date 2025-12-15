using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    public class ShoppingListsController : ApiController
    {
        private readonly IList<Models.ShoppingList> _shoppingLists;
        public ShoppingListsController(IList<Models.ShoppingList> shoppingLists) //wartości są wstrzykiwane przez mechanizm DI
        {
            _shoppingLists = shoppingLists;
        }

        [HttpGet]
        public IEnumerable<Models.ShoppingList> Get()
        {
            return _shoppingLists;
        }

        [HttpGet("{id:int}")]
        public Models.ShoppingList? Get(int id)
        {
            return _shoppingLists.FirstOrDefault(sl => sl.Id == id);
        }

        [HttpPost]
        public void Post([FromBody] Models.ShoppingList shoppingList) //wartość pochodzi z ciała requestu
        {
            shoppingList.Id = _shoppingLists.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _shoppingLists.Add(shoppingList);
        }

        [HttpPut("{id:int}")]
        public void Put(int id, [FromBody] Models.ShoppingList shoppingList) //wartość pochodzi z ciała requestu
        {
            var existingList = _shoppingLists.FirstOrDefault(sl => sl.Id == id);
            if (existingList != null)
            {
                existingList.Name = shoppingList.Name;
                // Aktualizuj inne właściwości w razie potrzeby
            }
        }
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            var shoppingList = _shoppingLists.FirstOrDefault(sl => sl.Id == id);
            if (shoppingList != null)
            {
                _shoppingLists.Remove(shoppingList);
            }
        }
    }
}
