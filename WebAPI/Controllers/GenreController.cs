using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Queries;
using WebAPI.Entities;

namespace WebAPI.Controllers
{
    [Route("api/genre")]
    public class GenreController: ControllerBase
    {
        public AppDb Db { get; }

        public GenreController(AppDb db)
        {
            Db = db;
        }

        // GET api/genre
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new GenreQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/genre/Action
        [HttpGet("{type}")]
        public async Task<IActionResult> GetOne(string type)
        {
            await Db.Connection.OpenAsync();
            var query = new GenreQuery(Db);
            var result = await query.FindOneAsync(type);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/genre
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Genre body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // DELETE api/genre/Action
        [HttpDelete("{type}")]
        public async Task<IActionResult> DeleteOne(string type)
        {
            await Db.Connection.OpenAsync();
            var query = new GenreQuery(Db);
            var result = await query.FindOneAsync(type);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/genre
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new GenreQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
    }
}
