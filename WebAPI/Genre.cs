using System.Net.NetworkInformation;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace WebAPI
{
    public class Genre
    {
        public string Type { get; set; }

        internal AppDb Db { get; set; }

        public Genre()
        {
        }

        internal Genre(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO genre (type) VALUES (@type);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Type = "Drama";
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `genre` SET `type` = @type WHERE `type` = @type;";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `BlogPost` WHERE `type` = @type;";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@type",
                DbType = DbType.String,
                Value = Type,
            });
        }
    }
}