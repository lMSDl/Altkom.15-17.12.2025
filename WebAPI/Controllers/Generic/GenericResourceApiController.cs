using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers.Generic
{
    public abstract class GenericResourceApiController<T> : GenericController<T>
    {
        private readonly IGenericService<T> _service;
        public GenericResourceApiController(IGenericService<T> service, IValidator<T>? validator = null) : base(service, validator) //wartości są wstrzykiwane przez mechanizm DI
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            //metoda pomocnicza Ok() - zwraca kod statusu 200 wraz z danymi
            var entities = await _service.ReadAsync();
            return Ok(entities);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<T>> Post([FromBody] T entity) //wartość pochodzi z ciała requestu
        {

            if(_validator is not null)
            {
                var validationResult = await _validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }
            }

            var id = await _service.CreateAsync(entity);

            //metoda CreatedAtAction pozwala na wskazanie funkcji, która może zostać użyta do pobrania nowo utworzonego zasobu - spowoduje to dodanie nagłówka Location do odpowiedzi

            return CreatedAtAction(nameof(Get), new { id }, id); //zwracamy tylko Id w body odpowiedzi

            /*return CreatedAtAction(nameof(Get), 
                new { id = shoppingList.Id }, //obiekt anonimowy z parametrami trasy - "id" to nazwa parametru w metodzie Get
                shoppingList);*/ //zwracamy cały obiekt w body odpowiedzi
        }

    }
}
