using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Xml.Linq;
using WebAPI.Controllers.Generic;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ServiceFilter<LimiterFilter>]
    public class PeopleController : GenericResourceApiController<Person>
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

        public override async Task<ActionResult<Person>> Post([FromBody] Person entity)
        {
            if((await _peopleService.ReadByName(entity.FirstName)).Any() &&
               (await _peopleService.ReadByName(entity.LastName)).Any())
            {
                ModelState.AddModelError(nameof(Person), "A person with the same first name and last name already exists.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys.Where(x => x.StartsWith(nameof(Person.Parent))))
                    ModelState.Remove(key);
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
            }

            return await base.Post(entity);
        }
    }
}
