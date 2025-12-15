using Microsoft.AspNetCore.Mvc;
using Models;

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
        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public ActionResult<IList<ShoppingList>> Get()
        {
            //metoda pomocnicza Ok() - zwraca kod statusu 200 wraz z danymi
            return Ok(_shoppingLists);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ShoppingList> Get(int id)
        {
            var entity = _shoppingLists.FirstOrDefault(sl => sl.Id == id);
            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public ActionResult<ShoppingList> Post([FromBody] Models.ShoppingList shoppingList) //wartość pochodzi z ciała requestu
        {
            shoppingList.Id = _shoppingLists.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _shoppingLists.Add(shoppingList);

            //metoda CreatedAtAction pozwala na wskazanie funkcji, która może zostać użyta do pobrania nowo utworzonego zasobu - spowoduje to dodanie nagłówka Location do odpowiedzi

            /*return CreatedAtAction(nameof(Get), new { id = shoppingList.Id }, 
             * shoppingList.Id);*/ //zwracamy tylko Id w body odpowiedzi

            return CreatedAtAction(nameof(Get), 
                new { id = shoppingList.Id }, //obiekt anonimowy z parametrami trasy - "id" to nazwa parametru w metodzie Get
                shoppingList); //zwracamy cały obiekt w body odpowiedzi
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Models.ShoppingList shoppingList) //wartość pochodzi z ciała requestu
        {
            var existingList = _shoppingLists.FirstOrDefault(sl => sl.Id == id);
            if (existingList is null)
            {
                return NotFound();
            }

            existingList.Name = shoppingList.Name;
            // Aktualizuj inne właściwości w razie potrzeby

            return NoContent(); // Zwraca kod statusu 204 No Content
        }
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var shoppingList = _shoppingLists.FirstOrDefault(sl => sl.Id == id);
            if (shoppingList is null)
            {
                return NotFound();
            }

            _shoppingLists.Remove(shoppingList);
            return NoContent();
        }
    }
}
