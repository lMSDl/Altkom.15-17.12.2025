using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IList<int> _values;
        public ValuesController(IList<int> values) //wartości są wstrzykiwane przez mechanizm DI
        {
            _values = values;
        }

        [HttpGet] //metoda obsługuje żądania HTTP GET
        public IEnumerable<int> Get()
        {
            return _values;
        }

        [HttpGet("{index:int}")]
        public int Get(int index)
        {
            return _values[index];
        }

        // [HttpDelete] // domyślny route to api/values - parametr w query requestu
        [HttpDelete("{value:int}")] // {value:int} - parametr w path z określonym typem
        public void Delete(int value)
        {
            _values.Remove(value);
        }

        [HttpDelete("query")] // jeśli route nie zaczyna się od / to jest to route względny - dołącza się do domyślnego adresu kontrolera
        [HttpDelete("/wartosci/query")] //jeśli route zaczyna się od / to jest to route absolutny - pełny nowy adres
        public void DeleteFromQuery(int value)
        {
            _values.Remove(value);
        }

        [HttpPost("{value:int:max(50):min(30)}")] //metoda "walidacji" parametru w route - działają jak filtry przed wykonaniem metody (jeśli nie przejdzie to 404)
        public void Post(int value)
        {
            _values.Add(value);
        }

        [HttpPut("{index:int}")]
        public void Put(int index, [FromQuery] int value) //wartość pochodzi z query string
        {
            _values[index] = value;
        }
    }
}
