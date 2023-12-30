using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Fitness.General
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExstension
    {
        /* /// <summary>
         /// Регистрация с помощью маркерных интерфейсов
         /// </summary>
         public static void RegistrationOnInterface<TInterface>(this IServiceCollection serviceDescriptors,
             ServiceLifetime lifetime)
         {
             var servicetype = typeof(TInterface);
             var allTypes = servicetype.Assembly.GetTypes()
                 .Where(x => servicetype.IsAssignableFrom(x)
                 && !(x.IsInterface || x.IsAbstract));

             foreach (var type in allTypes)
             {
                 serviceDescriptors.TryAdd(new ServiceDescriptor(type, type, lifetime));
                 var interfaces = type.GetTypeInfo().ImplementedInterfaces
                     .Where(x => x != typeof(IDisposable) && x.IsPublic && x != servicetype);

                 foreach (Type interfaceType in interfaces)
                 {
                     serviceDescriptors.TryAdd(new ServiceDescriptor(interfaceType, provider =>
                     provider.GetRequiredService(type), lifetime));
                 }
             }
         }  */

        public static void AssemblyInterfaceAssignableTo<TInterface>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            var serviceType = typeof(TInterface);
            var types = serviceType.Assembly.GetTypes()
                .Where(x => serviceType.IsAssignableFrom(x) && !(x.IsAbstract || x.IsInterface));
            foreach (var type in types)
            {
                services.TryAdd(new ServiceDescriptor(type, type, lifetime));
                var interfaces = type.GetTypeInfo().ImplementedInterfaces
                .Where(i => i != typeof(IDisposable) && i.IsPublic && i != serviceType);
                foreach (var interfaceType in interfaces)
                {
                    services.TryAdd(new ServiceDescriptor(interfaceType,
                        provider => provider.GetRequiredService(type),
                        lifetime));
                }
            }
        }

        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(provider =>
            {
                var profiles = provider.GetServices<Profile>();
                var mapperConfig = new MapperConfiguration(mc =>
                {
                    foreach (var profile in profiles)
                    {
                        mc.AddProfile(profile);
                    }
                });
                var mapper = mapperConfig.CreateMapper();
                return mapper;
            });
        }

        /// <summary>
        /// Регистрирует <see cref="Profile"/> автомапера
        /// </summary>
        public static void RegisterAutoMapperProfile<TProfile>(this IServiceCollection services) where TProfile : Profile
            => services.AddSingleton<Profile, TProfile>();
    }
}