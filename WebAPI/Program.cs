using Bogus;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;

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

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
