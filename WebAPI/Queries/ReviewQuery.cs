using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using MySqlConnector;
using WebAPI.Entities;

namespace WebAPI.Queries
{
    public class ReviewQuery
    {
        public AppDb Db { get; }

        public ReviewQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Review> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `movie_id`, `rating`, `content` FROM `review` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Review>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `movie_id`, `rating`, `content` FROM `review` ORDER BY `id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `review`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Review>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Review>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Review(Db)
                    {
                        Id = reader.GetInt32(0),
                        MovieId = reader.GetInt32(1),
                        Rating = reader.GetInt32(2),
                        Content = reader.GetString(3)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
