using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers.Generic
{
    public abstract class GenericController<T> : ApiController
    {
        private readonly IGenericService<T> _service;
        protected IValidator<T>? _validator;
        public GenericController(IGenericService<T> service, IValidator<T>? validator = null) //wartości są wstrzykiwane przez mechanizm DI
        {
            _service = service;
            _validator = validator;
        }

       
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Get(int id)
        {
            var entity = await _service.ReadByIdAsync(id);
            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
