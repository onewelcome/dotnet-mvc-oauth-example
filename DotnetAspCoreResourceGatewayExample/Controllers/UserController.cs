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
        
        //Sample data
        List<User> users = new List<User>()
        {
            new User()
            {
                FirstName = "Test",
                LastName = "de Tester",
                Email = "william.loosman@onegini.com",
                BirthDate = new DateTime(1993, 4, 23)
            },
            new User()
            {
                FirstName = "Kees",
                LastName = "Jansen",
                Email = "kees.jansen@example.com",
                BirthDate = new DateTime(1994, 3, 20)
            },
            new User()
            {
                FirstName = "Test",
                LastName = "de Tester 2",
                Email = "test.tester2@example.com",
                BirthDate = new DateTime(2001, 7, 12)
            },
        };

        [HttpGet]
        [Route("/")]
        public ActionResult<string> Home()
        {
            return "Welcome home";
        } 
        
        // GET api/user
        [HttpGet]
        public ActionResult<Result> User()
        {
            Result r = new Result();
            
            
            
            return r;
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
        
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(users);
        }
    }
}