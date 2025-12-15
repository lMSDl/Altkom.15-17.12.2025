var builder = WebApplication.CreateBuilder(args);
//miejsce na konfiguracjê us³ug (dependency injection)


//budowanie aplikacji - tworzenie potoku (pipe) przetwarzania ¿¹dañ
var app = builder.Build();


//middleware poœrednicz¹cy
app.Use(async (context, next) =>
{
    Console.WriteLine(context.GetEndpoint()?.DisplayName ?? "None");
    Console.WriteLine("Before Use1");

    await next(context);

    Console.WriteLine("After Use1");
});

//rêczne w³¹czenie routingu - jeœli nie jest jawnie w³¹czone, to bêdzie automatycznie w³¹czone na pocz¹ku potoku
//app.UseRouting();

app.Use(async (context, next) =>
{
    Console.WriteLine(context.GetEndpoint()?.DisplayName);
    Console.WriteLine("Before Use2");

    await next(context);

    Console.WriteLine("After Use2");
});

//mapowanie endpointów - tworzy middleware terminalny dla ka¿dego endpointu
//jest to skrócona forma app.UseEndpoints(...)
app.MapGet("/", () => "Hello World!");

//pe³na obs³uga endpointów
//jawne w³¹czenie routingu jest wymagane
//automatyczne w³¹czenie (brak jawnego wywo³ania) enpointów powoduje, ¿e trafiaj¹ one na koniec potoku
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "Hello World!");

    endpoints.MapGet("/endpoints", async context =>
    {
        await context.Response.WriteAsync("Hello from UseEndpoints!");
    });

});*/

//mapowanie œcie¿ki - tworzy podaplikacjê (nowego pipe)
app.Map("/app2", app2 =>
{
    //middleware terminalny (terminal middleware)
    app2.Run(async (context) =>
    {
        Console.WriteLine("In Run");
        await context.Response.WriteAsync("Hello from Run middleware!");
    });
});



//w³¹czenie aplikacji
app.Run();
