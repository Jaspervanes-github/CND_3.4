using System.Net.NetworkInformation;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace WebAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public string GenreType { get; set; }

        internal AppDb Db { get; set; }

        public Movie()
        {
        }

        internal Movie(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO movie (title, director, releaseyear, genre_type) VALUES (@title, @director, @releaseyear, @genre_type);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `movie` SET `title` = @title, `director` = @director, `releaseyear` = @releaseyear, `genre_type` = @genre_type WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `movie` WHERE `id` = @id;";
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
                ParameterName = "@title",
                DbType = DbType.String,
                Value = Title,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@director",
                DbType = DbType.String,
                Value = Director,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@releaseyear",
                DbType = DbType.Int32,
                Value = ReleaseYear,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@genre_type",
                DbType = DbType.String,
                Value = GenreType,
            });
        }
    }
}