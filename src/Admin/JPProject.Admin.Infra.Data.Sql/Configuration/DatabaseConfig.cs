using JPProject.Admin.Infra.Data.Configuration;
using JPProject.Admin.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseConfig
    {
        public static IServiceCollection WithSqlServer(this IServiceCollection services, string connectionString, JpDatabaseOptions options = null)
        {
            var migrationsAssembly = typeof(DatabaseConfig).GetTypeInfo().Assembly.GetName().Name;
            services.AddAdminContext(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }
        public static IServiceCollection WithSqlServer(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, JpDatabaseOptions options = null)
        {
            services.AddAdminContext(optionsAction, options);

            return services;
        }
    }
}