using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using MySqlConnector;
using WebAPI.Entities;

namespace WebAPI.Queries
{
    public class UserReviewQuery
    {
        public AppDb Db { get; }

        public UserReviewQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<UserReview> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `reviewlist_id`, `review_id` FROM `userreview` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<UserReview>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `reviewlist_id`, `review_id` FROM `userreview` ORDER BY `id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `userreview`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<UserReview>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<UserReview>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new UserReview(Db)
                    {
                        Id = reader.GetInt32(0),
                        ReviewListId = reader.GetInt32(1),
                        ReviewId = reader.GetInt32(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
