using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CQRS_Simple.API.Modules;
using CQRS_Simple.Domain.Products.Request;
using CQRS_Simple.EntityFrameworkCore;
using CQRS_Simple.Infrastructure;
using CQRS_Simple.Infrastructure.MQ;
using CQRS_Simple.Modules;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace CQRS_Simple.API
{
    public class Startup
    {
        public IConfigurationRoot _configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        public ILoggerFactory _LoggerFactory { get; private set; }

        private const string SqlServerConnection = "ConnectionStrings:Default";

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMQOptions>(_configuration.GetSection("RabbitMQ"));

            services.AddHostedService<MyListener>();

            services.AddControllersWithViews(option =>
                {
                    option.AllowEmptyInputInBodyModelBinding = true; // false as Default
                })
                .AddNewtonsoftJson(c => { c.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });

            services.AddFluentValidation(new[]
            {
                typeof(Startup).Assembly,
                typeof(ProductsRequestInput).GetTypeInfo().Assembly
            });

            services.AddDbContext<SimpleDbContext>(options =>
                options.UseSqlServer(_configuration[SqlServerConnection]));

            AddSwagger(services);
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 类型注入
            builder.Register(c => new CallLogger(Console.Out));

            builder.RegisterModule(new IocManagerModule());

            builder.RegisterModule(new LoggerModule());

            builder.RegisterModule(new InfrastructureModule(_configuration[SqlServerConnection]));

            builder.RegisterModule(new MediatorModule());

            builder.RegisterModule(new AutoMapperModule());
        }

        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            //loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            ConfigureSwagger(app);
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample CQRS API V1"); });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://somall.top/about")
                });
                options.DocInclusionPredicate((docName, description) => true);
            });
        }
    }
}