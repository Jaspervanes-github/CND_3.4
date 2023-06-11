using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Queries;

namespace WebAPI.Controllers
{
    [Route("api/review")]
    public class ReviewController: ControllerBase
    {
        public AppDb Db { get; }

        public ReviewController(AppDb db)
        {
            Db = db;
        }

        // GET api/review
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/review/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/review/Tarzan
        [HttpPost("{title}")]
        public async Task<IActionResult> Post(string title, [FromBody] Review body)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsync(title);
            if (result is null)
                return new NotFoundResult();
            body.Db = Db;
            body.MovieId = result.Id;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/review/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Review body)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.MovieId = body.MovieId;
            result.Rating = body.Rating;
            result.Content = body.Content;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/review/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/review
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
    }
}
