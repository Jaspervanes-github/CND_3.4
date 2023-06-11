using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace WebAPI
{
    public class GenreQuery
    {
        public AppDb Db { get; }

        public GenreQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Genre> FindOneAsync(string type)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `type` FROM `genre` WHERE `type` = @type";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@type",
                DbType = DbType.String,
                Value = type,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Genre>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `type` FROM `genre`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `genre`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Genre>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Genre>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Genre(Db)
                    {
                        Type = reader.GetString(0),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
