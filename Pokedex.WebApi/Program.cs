using Pokedex.WebApi.Factories;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Services
builder.Services.AddHttpClient<IPokeApiService, PokeApiService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["ExternalApis:PokeAPI"]);
});
builder.Services.AddScoped<ITranslatorService, TranslatorService>();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<ICustomHttpClientFactory, CustomHttpClientFactory>();

//Libraries
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
