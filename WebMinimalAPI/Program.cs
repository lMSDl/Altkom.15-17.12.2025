var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IList<int>>([.. Enumerable.Repeat(0, 100).Select(x => Random.Shared.Next())]);

var app = builder.Build();

//minimal API - mapowanie endpointów bezpoœrednio na aplikacjê
//parametry wyra¿enia lambda s¹ wstrzykiwane przez mechanizm dependency injection (DI)
app.MapGet("/values", (IList<int> values) => values);

//mo¿emy dodawaæ adnotacje do metod anonimowych
app.MapGet("/values/{index:int}", /*[Authorize]*/ (IList<int> values, int index) => values[index]);

//[value:int] - ograniczenie routingu do wartoœci ca³kowitych
app.MapDelete("/values/{value:int}", (IList<int> values, int value) => values.Remove(value));
app.MapPost("/values/{value:int}", (IList<int> values, int value) => values.Add(value));

//mo¿emy dodaæ atrybut [FromBody] lub [FromQuery] aby okreœliæ sk¹d pochodzi wartoœæ
app.MapPut("/values/{index:int}", (IList<int> values, int index, /*[FromQuery]*/ int value) => values[index] = value);


app.Run();

