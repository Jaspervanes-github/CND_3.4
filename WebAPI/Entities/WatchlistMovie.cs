using System.Net.NetworkInformation;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace WebAPI.Entities
{
    public class WatchlistMovie
    {
        public int Id { get; set; }
        public int WatchlistId { get; set; }
        public int MovieId { get; set; }

        internal AppDb Db { get; set; }

        public WatchlistMovie()
        {
        }

        internal WatchlistMovie(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO watchlistmovie (watchlist_id, movie_id) VALUES (@watchlist_id, @movie_id);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `watchlistmovie` SET `watchlist_id` = @watchlist_id, `movie_id` = @movie_id WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `watchlistmovie` WHERE `id` = @id;";
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
                ParameterName = "@watchlist_id",
                DbType = DbType.Int32,
                Value = WatchlistId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@movie_id",
                DbType = DbType.Int32,
                Value = MovieId,
            });
        }
    }
}