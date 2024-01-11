using CardGame.Entities;
using CardGame.UseCases;
using Mediator;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddScoped<ICommandHandler<CreateGameCommand, Result>, CreateGameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<JoinGameCommand, Result>, JoinGameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<PlayNextCardCommand, Result>, PlayNextCardCommandHandler>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
