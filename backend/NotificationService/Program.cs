using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Application;
using NotificationService.Extensions;
using NotificationService.Infrastructure.Interfaces;
using NotificationService.Infrastructure.PDF;
using NotificationService.Infrastructure.Repositories;
using QuestPDF.Infrastructure;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

Console.WriteLine("Connection string = " + builder.Configuration["MongoDb:ConnectionString"]);

// Додаємо сервіси до контейнера залежностей
builder.Services.AddControllers();
builder.Services.AddCommunication(builder.Configuration); // Інжекція, яку ви вказали
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();

builder.Services.AddSingleton<TicketPdfGenerator>();

var app = builder.Build();

// Налаштування HTTP-конвеєра
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();