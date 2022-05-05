using BurgerKingOrderProcess.MicroService.Data;
using BurgerKingOrderProcess.MicroService.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderProcessContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderProcessContext") ?? throw new InvalidOperationException("Connection string 'OrderProcessContext' not found.")));

// Add services to the container.
builder.Services.AddSingleton<IServiceBusConsumer,ServiceBusConsumer>();
builder.Services.AddSingleton<IProcessData,ProcessData>();
builder.Services.AddControllers();
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
var bus = app.Services.GetService<IServiceBusConsumer>();
bus.RegisterOnMessageHandlerAndReceiveMessages().GetAwaiter().GetResult();

app.Run();
