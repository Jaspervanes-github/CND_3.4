using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Queries;

namespace WebAPI.Controllers
{
    [Route("api/watchlist")]
    public class WatchlistMovieController: ControllerBase
    {
        public AppDb Db { get; }

        public WatchlistMovieController(AppDb db)
        {
            Db = db;
        }

        // GET api/watchlist
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new WatchlistMovieQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/watchlist/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new WatchlistMovieQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/watchlist
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WatchlistMovie body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/watchlist/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] WatchlistMovie body)
        {
            await Db.Connection.OpenAsync();
            var query = new WatchlistMovieQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.WatchlistId = body.WatchlistId;
            result.MovieId = body.MovieId;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/watchlist/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new WatchlistMovieQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/watchlist
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new WatchlistMovieQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
    }
}
