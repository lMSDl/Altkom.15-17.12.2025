using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    public class PeopleController : GenericController<Person>
    {
        private readonly IPeopleService _peopleService;
        public PeopleController(IPeopleService service) : base(service)
        {
            _peopleService = service;
        }

        //blokujemy endpoint Get z klasy bazowej
        [NonAction]
        public override Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return base.Get();
        }

        //nowy endpoint GetByName, zastępujący poprzedni i wprowadzający filtrowanie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetByName(string? name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return await Get();
            }

            var people = await _peopleService.ReadByName(name);
            return Ok(people);
        }

        //wyłączamy endpoint Put z klasy bazowej - nie chcemy pozwolić na aktualizację osób
        [NonAction]
        public override Task<ActionResult> Put(int id, [FromBody] Person entity)
        {
            return base.Put(id, entity);
        }

    }
}
