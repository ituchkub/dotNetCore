using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<UserItem> _users = new List<UserItem>();

        [Route("")]
        [HttpGet]
        public ObjectResult Get()
        {
            return Ok(_users);
        }

        [Route("")]
        [HttpPost]
        public ObjectResult Create(UserItem user)
        {
            user.Id = Guid.NewGuid().ToString();
            _users.Add(user);

            return Ok(user);
        }

        [Route("{id}")]
        [HttpPatch]
        public ObjectResult Update(string id, UserItem user)
        {
            var obj = _users.FirstOrDefault(item => item.Id == id);

            if (obj == null)
            {
                return new NotFoundObjectResult(id);
            }

            obj.Name = user.Name;

            return Ok(obj);
        }

        [Route("{id}")]
        [HttpDelete]
        public ObjectResult Delete(string id)
        {
            _users.RemoveAll(item => item.Id == id);

            return Ok(id);
        }
    }
}
