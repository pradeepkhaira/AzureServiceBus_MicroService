using BurgerKingOrderProcess.MicroService.Data;
using BurgerKingOrderProcess.MicroService.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderProcessContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderProcessContext") ?? throw new InvalidOperationException("Connection string 'OrderProcessContext' not found.")));

// Add services to the container.
builder.Services.AddScoped<IServiceBusConsumer,ServiceBusConsumer>();
builder.Services.AddScoped<IProcessData,ProcessData>();
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
using (var scope = ((IApplicationBuilder)app).ApplicationServices.CreateScope())
{
    var sc = scope.ServiceProvider.GetService<IServiceBusConsumer>();
    sc.RegisterOnMessageHandlerAndReceiveMessages().GetAwaiter().GetResult();
}

app.Run();
