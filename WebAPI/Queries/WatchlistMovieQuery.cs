using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using MySqlConnector;
using WebAPI.Entities;

namespace WebAPI.Queries
{
    public class WatchlistMovieQuery
    {
        public AppDb Db { get; }

        public WatchlistMovieQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<WatchlistMovie> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `watchlist_id`, `movie_id` FROM `watchlistmovie` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<WatchlistMovie>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `watchlist_id`, `movie_id` FROM `watchlistmovie` ORDER BY `id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `wacthlistmovie`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<WatchlistMovie>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<WatchlistMovie>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new WatchlistMovie(Db)
                    {
                        Id = reader.GetInt32(0),
                        WatchlistId = reader.GetInt32(1),
                        MovieId = reader.GetInt32(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
