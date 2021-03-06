using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.EntityFramework.Repository.Context;
using JPProject.Admin.EntityFramework.Repository.Repository;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Interfaces;
using JPProject.EntityFrameworkCore.EventSourcing;
using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextConfiguration
    {

        public static IJpProjectConfigurationBuilder ConfigureJpAdminStorageServices(this IJpProjectConfigurationBuilder services)
        {
            RegisterStorageServices(services.Services);
            return services;
        }

        public static IJpProjectConfigurationBuilder SetupDefaultIdentityServerContext<TContext>(
            this IJpProjectConfigurationBuilder services)
            where TContext : IPersistedGrantDbContext, IConfigurationDbContext
        {
            // Configure identityserver4 default database
            var operationalStoreOptions = new OperationalStoreOptions();
            var storeOptions = new ConfigurationStoreOptions();

            services.Services.AddSingleton(operationalStoreOptions);
            services.Services.AddSingleton(storeOptions);
            services.Services.TryAddScoped<IPersistedGrantDbContext>(x => x.GetRequiredService<TContext>());
            services.Services.TryAddScoped<IConfigurationDbContext>(x => x.GetRequiredService<TContext>());
            return services;
        }

        public static IJpProjectConfigurationBuilder AddJpAdminContext(this IJpProjectConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction, JpDatabaseOptions options = null)
        {
            if (options == null)
                options = new JpDatabaseOptions();

            RegisterStorageServices(services.Services);

            services.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();

            services.Services.AddDbContext<JpProjectAdminUiContext>(optionsAction);
            services.Services.AddScoped<IJpEntityFrameworkStore>(x => x.GetService<JpProjectAdminUiContext>());
            services.Services.AddScoped<IConfigurationDbContext>(x => x.GetService<JpProjectAdminUiContext>());
            services.Services.AddScoped<IPersistedGrantDbContext>(x => x.GetService<JpProjectAdminUiContext>());
            services.SetupDefaultIdentityServerContext<JpProjectAdminUiContext>();
            return services;
        }

        public static IJpProjectConfigurationBuilder ConfigureAdminContext<TContext>(this IJpProjectConfigurationBuilder services)
            where TContext : class, IPersistedGrantDbContext, IConfigurationDbContext, IJpEntityFrameworkStore, IEventStoreContext
        {
            RegisterStorageServices(services.Services);

            services.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.Services.TryAddScoped<IJpEntityFrameworkStore, TContext>(); services.Services.TryAddScoped<IPersistedGrantDbContext>(x => x.GetRequiredService<TContext>());
            services.Services.TryAddScoped<IConfigurationDbContext>(x => x.GetRequiredService<TContext>());
            services.Services.TryAddScoped<IEventStoreContext>(x => x.GetRequiredService<TContext>());

            return services;
        }

        public static IJpProjectConfigurationBuilder ConfigureAdminContext<TContext, TEventStore>(this IJpProjectConfigurationBuilder services)
            where TContext : class, IPersistedGrantDbContext, IConfigurationDbContext, IJpEntityFrameworkStore
            where TEventStore : class, IEventStoreContext
        {
            RegisterStorageServices(services.Services);
            services.Services.TryAddScoped<IEventStoreContext>(x => x.GetRequiredService<TEventStore>());
            services.Services.TryAddScoped<IConfigurationDbContext>(x => x.GetRequiredService<TContext>());
            services.Services.TryAddScoped<IPersistedGrantDbContext>(x => x.GetRequiredService<TContext>());
            services.Services.TryAddScoped<IJpEntityFrameworkStore>(x => x.GetRequiredService<TContext>());

            return services;
        }


        private static void RegisterStorageServices(IServiceCollection services)
        {
            services.AddScoped<IPersistedGrantRepository, PersistedGrantRepository>();
            services.AddScoped<IApiResourceRepository, ApiResourceRepository>();
            services.AddScoped<IIdentityResourceRepository, IdentityResourceRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.TryAddScoped<IEventStore, SqlEventStore>();
        }

        public static IJpProjectConfigurationBuilder AddEventStore<TEventStore>(this IJpProjectConfigurationBuilder services)
            where TEventStore : class, IEventStoreContext
        {
            services.Services.AddScoped<IEventStoreContext, TEventStore>();
            return services;
        }
    }
}
