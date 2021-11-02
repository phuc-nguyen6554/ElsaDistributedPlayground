using Elsa.Activities.MassTransit.Consumers;
using Elsa.Activities.MassTransit.Extensions;
using Elsa.Services;
using ElsaPlayground.Messages;
using ElsaPlayground.Workflows;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaPlayground
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private Type CreateWorkflowConsumer(Type messageType) => typeof(WorkflowConsumer<>).MakeGenericType(messageType);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ElsaPlayground", Version = "v1" });
            });

            services.AddMassTransit(config =>
            {
                config.AddConsumer(CreateWorkflowConsumer(typeof(TestMessage)));

                config.SetKebabCaseEndpointNameFormatter();
                config.UsingRabbitMq((context, config) => {
                    config.Host("host.docker.internal", 5672, "/", h =>
                    {
                        h.Username("admin");
                        h.Password("admin");
                    });
                    config.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
            services.AddElsa(options =>
            {
                options
                .AddConsoleActivities()
                .AddMassTransitActivities()
                .AddWorkflow<HelloWorld>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElsaPlayground v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var scope = app.ApplicationServices.CreateScope();
            var s = scope.ServiceProvider.GetRequiredService<IBuildsAndStartsWorkflow>();
           // s.BuildAndStartWorkflowAsync<HelloWorld>();
        }
    }
}
