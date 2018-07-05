using Microsoft.AspNetCore.Mvc;

namespace DotnetAspCoreResourceGatewayExample.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        // GET api/wishlist/5
        [HttpGet("{id}")]
        public ActionResult<WishList> GetWishList(int id)
        {
            return new WishList();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string wishListName)
        {
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            
        }

        [HttpGet("{id}")]
        [Route("/item")]
        public ActionResult<string> GetItem(int id)
        {
            return "item1";
        }
    }
}