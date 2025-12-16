


using ConsoleApp;
using Models;
using System.Net.Http.Json;
using System.Text.Json;

var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5120/api/");
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

var result = await httpClient.GetAsync("people");

if(result.StatusCode != System.Net.HttpStatusCode.OK)
{
    Console.WriteLine($"Error: {(int)result.StatusCode} - {result.ReasonPhrase}");
}

if(!result.IsSuccessStatusCode) //sprawdza czy status code jest w zakresie 200-299
{
    Console.WriteLine($"Error: {(int)result.StatusCode} - {result.ReasonPhrase}");
}

result.EnsureSuccessStatusCode(); //rzuca wyjątek jeśli status code nie jest w zakresie 200-299


var jsonString = await result.Content.ReadAsStringAsync();
Console.WriteLine(jsonString);

JsonSerializerOptions options = new()
{
    WriteIndented = true,
    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
    PropertyNameCaseInsensitive = true
};

//var people = await result.Content.ReadFromJsonAsync<IEnumerable<Person>>();
var people = JsonSerializer.Deserialize<IEnumerable<Person>>(jsonString, options);

var shoppingList = new ShoppingList
{
    Name = "My Shopping List"
};

//metoda pozwalająca wysłać obiekt jako JSON w ciele żądania - korzysta z Jsona z bibliotek standardowych
var postResult = await httpClient.PostAsJsonAsync("shoppinglists", shoppingList, options);

//jeśli chcemy użyć innego serializatora lub dostosować jego ustawienia, możemy wygenerować string i przesłać go jako StringContent
//using StringContent stringContent = new StringContent(JsonSerializer.Serialize(shoppingList, options), System.Text.Encoding.UTF8, "application/json");
//postResult = await httpClient.PostAsync("shoppinglists", stringContent);

postResult.EnsureSuccessStatusCode();

result = await httpClient.GetAsync($"shoppinglists/{await postResult.Content.ReadFromJsonAsync<int>()}");
result.EnsureSuccessStatusCode();

var shoppingListFromApi = await result.Content.ReadFromJsonAsync<ShoppingList>(options);


using WebApiClient webApiClient = new WebApiClient("http://localhost:5120/api/");
webApiClient.JsonSerializerOptions.WriteIndented = true;
webApiClient.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
webApiClient.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

var peopleFromClient =  await webApiClient.GetAsync<IEnumerable<Person>>("people");

await webApiClient.PutAsync("shoppinglists", 99, new ShoppingList
{
    Name = "Updated Shopping List"
});
shoppingListFromApi = await webApiClient.GetAsync<ShoppingList>("shoppinglists", 99);


Console.ReadLine();


