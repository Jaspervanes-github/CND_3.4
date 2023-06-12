using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Queries;

namespace WebAPI.Controllers
{
    [Route("api/reviewlist")]
    public class UserReviewController: ControllerBase
    {
        public AppDb Db { get; }

        public UserReviewController(AppDb db)
        {
            Db = db;
        }

        // GET api/reviewlist
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new UserReviewQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/reviewlist/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new UserReviewQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // GET api/reviewlist/list/1
        [HttpGet("list/{list_id}")]
        public async Task<IActionResult> GetOneFromListId(int list_id)
        {
            await Db.Connection.OpenAsync();
            var query = new UserReviewQuery(Db);
            var result = await query.FindOneAsyncFromListId(list_id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/reviewlist
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserReview body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/reviewlist/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] UserReview body)
        {
            await Db.Connection.OpenAsync();
            var query = new UserReviewQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.ReviewListId = body.ReviewListId;
            result.ReviewId = body.ReviewId;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/reviewlist/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new UserReviewQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/reviewlist
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new UserReviewQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
    }
}
