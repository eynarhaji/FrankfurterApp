using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FrankfurterApp.ErrorHandling.SwaggerSamples;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FrankfurterApp.Injections
{
    public static class SwaggerInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            services.AddSwaggerGen(options =>
            {
                var runtimeVersion = executingAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                options.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = executingAssembly.GetName().Name, Version = runtimeVersion?.InformationalVersion
                    });

                var xmlFile = $"{executingAssembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });

                options.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblies(executingAssembly);
            services.AddSwaggerExamplesFromAssemblyOf<GenericErrorSwaggerExample>();

            return services;
        }

        public static WebApplication AddSwagger(this WebApplication app)
        {
            if (app.Environment.EnvironmentName is "Local" or "Tests")
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FrankfurterApp - v1");
                });
            }

            return app;
        }
    }
}