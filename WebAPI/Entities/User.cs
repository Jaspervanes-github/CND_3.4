using System.Net.NetworkInformation;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace WebAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WatchlistId { get; set; }
        public int ReviewlistId { get; set; }

        internal AppDb Db { get; set; }

        public User()
        {
        }

        internal User(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO user (name, watchlist_id, reviewlist_id) VALUES (@name, @watchlist_id, @reviewlist_id);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `user` SET `name` = @name, `watchlist_id` = @watchlist_id, `reviewlist_id` = @reviewlist_id WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `user` WHERE `id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@watchlist_id",
                DbType = DbType.Int32,
                Value = WatchlistId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reviewlist_id",
                DbType = DbType.Int32,
                Value = ReviewlistId,
            });
        }
    }
}