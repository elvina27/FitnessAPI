using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Fitness.Common.Entity;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context;
using Fitness.Repositories;
using Fitness.Services;
using Fitness.Services.AutoMappers;
using Fitness.Api.AutoMappers;

namespace Fitness.API.Extensions
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Регистрирует все сервисы, репозитории и все что нужно для контекста
        /// </summary>
        public static void RegistrationSRC(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IDbWriterContext, DbWriterContext>();
            services.RegistrationService();
            services.RegistrationRepository();
            services.RegistrationContexts();
            services.AddAutoMapper(typeof(APIMappers), typeof(ServiceMapper));
        }

        /// <summary>
        /// Включает фильтры и ставит шрифт на перечесления
        /// </summary>
        /// <param name="services"></param>
        public static void RegistrationControllers(this IServiceCollection services)
        {
            services.AddControllers(x =>
            {
                x.Filters.Add<FitnessExceptionFilter>();
            })
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = false
                    });
                });
        }

        /// <summary>
        /// Настройки свагера
        /// </summary>
        public static void RegistrationSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Club", new OpenApiInfo { Title = "Клубы", Version = "v1" });
                c.SwaggerDoc("Coach", new OpenApiInfo { Title = "Тренеры", Version = "v1" });
                c.SwaggerDoc("Document", new OpenApiInfo { Title = "Документы", Version = "v1" });
                c.SwaggerDoc("Gym", new OpenApiInfo { Title = "Залы", Version = "v1" });
                c.SwaggerDoc("Study", new OpenApiInfo { Title = "Занятия", Version = "v1" });
                c.SwaggerDoc("TimeTableItem", new OpenApiInfo { Title = "Элементы расписания", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, "Fitness.API.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Настройки свагера
        /// </summary>
        public static void CustomizeSwaggerUI(this WebApplication web)
        {
            web.UseSwagger();
            web.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("Club/swagger.json", "Клубы");
                x.SwaggerEndpoint("Coach/swagger.json", "Тренеры");
                x.SwaggerEndpoint("Document/swagger.json", "Документы");
                x.SwaggerEndpoint("Gym/swagger.json", "Залы");
                x.SwaggerEndpoint("Study/swagger.json", "Занятия");
                x.SwaggerEndpoint("TimeTableItem/swagger.json", "Элементы расписания");
            });
        }
    }
}
