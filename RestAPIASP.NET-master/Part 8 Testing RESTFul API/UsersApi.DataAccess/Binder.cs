using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UsersApi.DataAccess.DataAccess.Implementations;
using UsersApi.DataAccess.DataAccess.Interfaces;
using UsersApi.DataAccess.DatabaseContexts;
using UsersApi.DataAccess.Repositories.Implementations;
using UsersApi.DataAccess.Repositories.Interfaces;

namespace UsersApi.DataAccess
{
    public static class Binder
    {
        public static void Setup(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IUsersDataAccess, UsersDataAccess>();
            services.AddTransient<IUsersRespository, UsersRepository>();

            var connectionString = config.GetConnectionString("UsersDb");
            services.AddDbContextPool<DbUsersContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
