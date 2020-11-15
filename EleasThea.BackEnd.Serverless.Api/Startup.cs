using EleasThea.BackEnd.Serverless.Api;
using EleasThea.BackEnd.Serverless.Api.Utitlities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Mail;

[assembly: FunctionsStartup(typeof(Startup))]

namespace EleasThea.BackEnd.Serverless.Api
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
                    EnableSsl = true,
                    Timeout = 15000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_config["EmailFrom"], _config["EmailPassword"])
                };
            });

            builder.Services.AddSingleton<EmailUtility>();
        }
    }
}
