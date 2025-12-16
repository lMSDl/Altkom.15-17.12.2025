using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebAPI.Controllers.Generic
{
    public abstract class GenericNestedResourceApiController<T, TParent> : ApiController
    {
        private readonly IGenericService<T> _service;
        private readonly IGenericService<TParent> _parentService;

        public GenericNestedResourceApiController(IGenericService<T> service, IGenericService<TParent> parentService)
        {
            _service = service;
            _parentService = parentService;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<T>>> GetAllByParentId(int parentId)
        {
            var parentEntity = await _parentService.ReadByIdAsync(parentId);
            if (parentEntity is null)
            {
                return NotFound($"Parent entity with ID {parentId} not found.");
            }

            var result = await _service.ReadAsync(x => GetParentId(x) == parentId);

            return Ok(result);
        }

        protected abstract int GetParentId(T item);
        protected abstract void SetParentId(T item, int parentId);

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Post(int parentId, [FromBody] T item)
        {
            var parentEntity = await _parentService.ReadByIdAsync(parentId);
            if (parentEntity is null)
            {
                return NotFound($"Parent entity with ID {parentId} not found.");
            }

            SetParentId(item, parentId);

            var createdId = await _service.CreateAsync(item);

            return CreatedAtAction(createdId);
        }

        protected abstract ActionResult<int> CreatedAtAction(int id);
    }
}
