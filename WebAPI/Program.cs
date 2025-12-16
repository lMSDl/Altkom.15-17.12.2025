using Bogus;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;
using WebAPI.Filters;
using WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IList<int>>([.. Enumerable.Repeat(0, 100).Select(x => Random.Shared.Next())]);
/*builder.Services.AddSingleton<IList<ShoppingList>>([
    new ShoppingList { Id = 1, Name = "Groceries" },
    new ShoppingList { Id = 2, Name = "Electronics" },
    new ShoppingList { Id = 3, Name = "Clothing" }
    ]);*/
builder.Services.AddSingleton<IGenericService<ShoppingList>, GenericBogusService<ShoppingList>>();
builder.Services.AddSingleton<IGenericService<Product>, GenericService<Product>>();
builder.Services.AddSingleton<IPeopleService, PeopleBogusService>();

builder.Services.AddTransient<Faker<ShoppingList>, ShoppingListFaker>();
builder.Services.AddTransient<Faker<Models.Person>, PersonFaker>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        //options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseUpper;
        //ignorowanie wlasciwosci tylko do odczytu podczas serializacji
        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        //ignorowanie wartosci domyslnych podczas serializacji
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
        //konfigurujemy serializacje aby obslugiwala cykle referencji za pomoc¹ mechanizmu referencji ($id, $ref)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    })
    .AddXmlDataContractSerializerFormatters(); //w³¹czamy obs³ugê dla formatu XML

//zawieszenie automatycznej walidacji modelu
builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);
//rêczna obs³uga b³êdów walidacji modelu
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return new BadRequestObjectResult(context.ModelState);
    };
});

//podejscie legacy (dla wersji FluentValudation < 12)
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<ShoppingList>, ShoppingListValidator>();

builder.Services.AddSingleton<ConsoleLogFilter>();
builder.Services.AddSingleton(new LimiterFilter(5));

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();



app.UseExceptionHandler();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
