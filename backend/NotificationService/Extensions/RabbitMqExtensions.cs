using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ;
using RabbitMQ.Client;
//using Newtonsoft.Json;
using System.Text;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Consumers;


namespace NotificationService.Extensions;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddCommunication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            // Додаємо обидва консюмери
            busConfigurator.AddConsumer<TicketCreatedConsumer>();
            busConfigurator.AddConsumer<TicketEmailSenderConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                // Підписуємо обидва консюмери на одну чергу
                configurator.ReceiveEndpoint("ReviewStatsQueue", e =>
                {
                    e.ConfigureConsumer<TicketCreatedConsumer>(context);
                    e.ConfigureConsumer<TicketEmailSenderConsumer>(context);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

