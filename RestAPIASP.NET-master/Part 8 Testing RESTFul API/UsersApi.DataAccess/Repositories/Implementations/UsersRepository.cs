using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsersApi.Common;
using UsersApi.DataAccess.DatabaseContexts;
using UsersApi.DataAccess.Repositories.Interfaces;

namespace UsersApi.DataAccess.Repositories.Implementations
{
    public class UsersRepository : IUsersRespository
    {
        private DbUsersContext _DbUsersContext;

        public UsersRepository(DbUsersContext dbUsersContext)
        {
            _DbUsersContext = dbUsersContext;
        }

        public User CreateUser(User obj)
        {
            obj.Id = Guid.NewGuid().ToString();
            obj.CreationDate = DateTime.Now;

            _DbUsersContext.UsersTable.Add(obj);
            _DbUsersContext.SaveChanges();
            return obj;
        }

        public void Delete(string id)
        {

            var items = _DbUsersContext.UsersTable.Where(item => item.Id == id);

            if (items.Any())
            {
                _DbUsersContext.Remove(items.First());
                _DbUsersContext.SaveChanges();
            }
        }

        public List<User> GetUsers()
        {
            return _DbUsersContext.UsersTable.ToList();
        }

        public User UpdateUser(User obj)
        {
            var items = _DbUsersContext.UsersTable.Where(item => item.Id == obj.Id);

            if (items.FirstOrDefault() == null)
            {
                return null;
            }

            var user = items.FirstOrDefault();
            user.Name = obj.Name;

            _DbUsersContext.SaveChanges();

            return user;
        }
    }
}
