using System.Net.NetworkInformation;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace WebAPI.Entities
{
    public class UserReview
    {
        public int Id { get; set; }
        public int ReviewListId { get; set; }
        public int ReviewId { get; set; }

        internal AppDb Db { get; set; }

        public UserReview()
        {
        }

        internal UserReview(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO userreview (reviewlist_id, review_id) VALUES (@reviewlist_id, @review_id);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `userreview` SET `reviewlist_id` = @reviewlist_id, `review_id` = @review_id WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `userreview` WHERE `id` = @id;";
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
                ParameterName = "@reviewlist_id",
                DbType = DbType.Int32,
                Value = ReviewListId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@review_id",
                DbType = DbType.Int32,
                Value = ReviewId,
            });
        }
    }
}