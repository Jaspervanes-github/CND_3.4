using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Queries;

namespace WebAPI.Controllers
{
    [Route("api/movie")]
    public class MovieController: ControllerBase
    {
        public AppDb Db { get; }

        public MovieController(AppDb db)
        {
            Db = db;
        }

        // GET api/movie
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/movie/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // GET api/movie/title/Tarzan
        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetOneFromTitle(string title)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsyncFromTitle(title);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // GET api/movie/director/James Cameron
        [HttpGet("director/{director}")]
        public async Task<IActionResult> GetOneFromDirector(string director)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsyncFromDirector(director);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // GET api/movie/releaseyear/1990
        [HttpGet("releaseyear/{releaseyear}")]
        public async Task<IActionResult> GetOneFromReleaseYear(int releaseyear)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsyncFromReleaseYear(releaseyear);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // GET api/movie/genre/Action
        [HttpGet("genre/{genre_type}")]
        public async Task<IActionResult> GetOneFromGenreType(string genre_type)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsyncFromGenreType(genre_type);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/movie
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/movie/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Movie body)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Title = body.Title;
            result.Director = body.Director;
            result.ReleaseYear = body.ReleaseYear;
            result.GenreType = body.GenreType;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/movie/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/movie
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }
    }
}
