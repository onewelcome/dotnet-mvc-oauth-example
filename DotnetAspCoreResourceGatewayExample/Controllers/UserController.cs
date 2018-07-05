using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAspCoreResourceGatewayExample.Model;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAspCoreResourceGatewayExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET api/user
        [HttpGet]
        public ActionResult<User> User()
        {
            return new User()
            {
                FirstName = "Kees",
                LastName = "Jansen",
                Email = "kees.jansen@example.com",
                BirthDate = new DateTime(1994, 4, 20)
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}