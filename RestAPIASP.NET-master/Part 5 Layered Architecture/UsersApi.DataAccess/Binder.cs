using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UsersApi.DataAccess.DataAccess.Implementations;
using UsersApi.DataAccess.DataAccess.Interfaces;
using UsersApi.DataAccess.Repositories.Implementations;
using UsersApi.DataAccess.Repositories.Interfaces;

namespace UsersApi.DataAccess
{
    public static class Binder
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddTransient<IUsersDataAccess, UsersDataAccess>();
            services.AddTransient<IUsersRespository, UsersRepository>();
        }
    }
}
