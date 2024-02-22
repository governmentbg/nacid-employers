using FileStorageNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resc.Application;
using Resc.Application.Common.Configurations;
using Resc.Application.Common.Extensions;
using Resc.Application.Common.Interfaces;
using Resc.Application.Ems.Models;
using Resc.Hosting.BackgroundServices;
using Resc.Hosting.BackgroundServices.Emails;
using Resc.Hosting.BackgroundServices.RabbitMqConsumers.Configurations;
using Resc.Hosting.Extensions;
using Resc.Hosting.Infrastructure.Configurations;
using Resc.Hosting.Infrastructure.Middlewares;
using Resc.Infrastructure;
using Resc.Persistence;
using Resc.Persistence.Extensions;

namespace Resc.Hosting
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public Startup(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = services.ConfigureApplicationConfiguration(environment);

            services
                .AddControllers(options => {
                    options.OutputFormatters.Add(new HttpNoContentOutputFormatter());
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                //.AddFluentValidation()
                .AddJson();
            ;

            var authConfig = configuration.GetSection("AuthConfiguration").Get<AuthConfiguration>();
            services.ConfigureJwtAuthService(authConfig.SecretKey, authConfig.Issuer, authConfig.Audience);

            services
                .AddPersistence<IAppDbContext, AppDbContext>(configuration.GetSection("DbConfiguration:ConnectionString").Value, environment.IsDevelopment())
                .AddPersistence<IAppLogContext, AppLogContext>(configuration.GetSection("DbConfiguration:LogConnectionString").Value, environment.IsDevelopment())
                .AddApplication(typeof(IUserContext).Assembly)
                .AddApiServices();
            ;

            services.AddFileStorage(configuration.GetSection("DbConfiguration:Descriptors"), configuration.GetSection("DbConfiguration:Encryption"));

            services.ConfigureAuthorization();

            var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            if (emailConfiguration.JobEnabled)
            {
                services.AddHostedService<EmailJob>();
            }

            var emsConfiguration = configuration.GetSection("EmsConfiguration").Get<EmsConfiguration>();
            services.AddEmsService(emsConfiguration.EmsUrl);

            var rndConsumerConfiguration = configuration.GetSection("RndConsumerConfiguration").Get<RndConsumerConfiguration>();
            if (rndConsumerConfiguration.IsConsumerEnabled)
            {
                services.AddConsumers();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RedirectionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions {
                OnPrepareResponse = context => {
                    if (context.File.Name == "index.html")
                    {
                        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                        context.Context.Response.Headers.Add("Expires", "-1");
                    }
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints
                .MapControllers()
                .RequireAuthorization()
            );
        }
    }
}
