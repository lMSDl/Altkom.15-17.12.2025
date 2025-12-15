using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    public abstract class GenericController<T> : ApiController
    {
        private readonly IGenericService<T> _service;
        public GenericController(IGenericService<T> service) //wartości są wstrzykiwane przez mechanizm DI
        {
            _service = service;
        }

        [HttpGet]
        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            //metoda pomocnicza Ok() - zwraca kod statusu 200 wraz z danymi
            var entities = await _service.ReadAsync();
            return Ok(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<T>> Get(int id)
        {
            var entity = await _service.ReadByIdAsync(id);
            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<T>> Post([FromBody] T entity) //wartość pochodzi z ciała requestu
        {
            var id = await _service.CreateAsync(entity);

            //metoda CreatedAtAction pozwala na wskazanie funkcji, która może zostać użyta do pobrania nowo utworzonego zasobu - spowoduje to dodanie nagłówka Location do odpowiedzi

            return CreatedAtAction(nameof(Get), new { id }, id); //zwracamy tylko Id w body odpowiedzi

            /*return CreatedAtAction(nameof(Get), 
                new { id = shoppingList.Id }, //obiekt anonimowy z parametrami trasy - "id" to nazwa parametru w metodzie Get
                shoppingList);*/ //zwracamy cały obiekt w body odpowiedzi
        }

        [HttpPut("{id:int}")]
        public virtual async Task<ActionResult> Put(int id, [FromBody] T entity) //wartość pochodzi z ciała requestu
        {
            if (await _service.ReadByIdAsync(id) is null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, entity);

            return NoContent(); // Zwraca kod statusu 204 No Content
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _service.ReadByIdAsync(id);
            if (entity is null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
