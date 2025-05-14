using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TSettings ExtractSettings<TSettings>(this IConfiguration configuration, string sectionName)
            where TSettings : class, new()
        {
            var settings = new TSettings();
            configuration.Bind(sectionName, settings);
            
            var context = new ValidationContext(settings);
            Validator.ValidateObject(settings, context, validateAllProperties: true);
            
            return settings;
        }

        public static IServiceCollection AddSettingsToOptions<TSettings>(this IServiceCollection services,
            IConfiguration configuration, string sectionName, out TSettings settings)
            where TSettings : class, new()
        {
            settings = new TSettings();
            configuration.Bind(sectionName, settings);
            services.Configure<TSettings>(configuration.GetSection(sectionName));

            var context = new ValidationContext(settings);
            Validator.ValidateObject(settings, context, validateAllProperties: true);

            return services;
        }
    }
}