﻿using EleasThea.BackEnd.Serverless.Services;
using EleasThea.BackEnd.Serverless.Services.Utitlities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Mail;

[assembly: FunctionsStartup(typeof(Startup))]

namespace EleasThea.BackEnd.Serverless.Services
{
    class Startup : FunctionsStartup
    {
        private readonly IConfigurationRoot _config;

        public Startup()
        {
            _config = new ConfigurationBuilder().AddEnvironmentVariables()
                                                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(factory =>
            {
                return new SmtpClient()
                {
                    Host = _config["EmailHost"],
                    Port = Convert.ToInt32(_config["EmailPort"]),
                    Timeout = 10000,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(_config["EmailFrom"], _config["EmailPassword"])
                };
            });

            builder.Services.AddSingleton<IEmailUtility>(Factory =>
            {
                return new EmailUtility(builder.Services.BuildServiceProvider().GetRequiredService<SmtpClient>(), _config["EmailFrom"]);
            });
        }
    }
}
