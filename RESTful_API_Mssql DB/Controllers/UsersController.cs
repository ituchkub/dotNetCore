using Microsoft.AspNetCore.Mvc;
using UsersApi;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<UserItem> _users = new List<UserItem>();
   
    [Route("")]
    [HttpGet]
    public ObjectResult Get()
    {

     
         ITuChDB.SetConnection("DESKTOP-TGQTG1J\\MSSQLSERVERS", "disney", "root", "0000");
                IConnection Conn = ITuChDB.GetConnection();
        return Ok(_users);
    }

    [Route("")]
    [HttpPost]
    public ObjectResult Create(UserItem user)
    {
        user.id = Guid.NewGuid().ToString();

        _users.Add(user);

        return Ok(user);
    }

    [Route("{id}")]
    [HttpPatch]
    public ObjectResult Update(string id, UserItem user)
    {
        var obj = _users.FirstOrDefault(item => item.id == id);

        if (obj == null)
        {
            return new NotFoundObjectResult(id);
        }

        obj.name = user.name;
        obj.Age = user.Age;
        obj.address = user.address;
        obj.tel = user.tel;

        return Ok(obj);
    }

    [Route("{id}")]
    [HttpDelete]
    public ObjectResult Delete(string id)
    {
        _users.RemoveAll(item => item.id == id);

        return Ok(id);
    }
}