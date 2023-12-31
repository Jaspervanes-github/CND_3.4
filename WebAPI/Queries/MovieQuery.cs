﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using MySqlConnector;
using WebAPI.Entities;

namespace WebAPI.Queries
{
    public class MovieQuery
    {
        public AppDb Db { get; }

        public MovieQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Movie> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `title`, `director`, `releaseyear`, `genre_type` FROM `movie` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Movie> FindOneAsyncFromTitle(string title)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `title`, `director`, `releaseyear`, `genre_type` FROM `movie` WHERE `title` = @title";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = title,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Movie> FindOneAsyncFromDirector(string director)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `title`, `director`, `releaseyear`, `genre_type` FROM `movie` WHERE `director` = @director";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@director",
                DbType = DbType.String,
                Value = director,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Movie> FindOneAsyncFromReleaseYear(int releaseyear)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `title`, `director`, `releaseyear`, `genre_type` FROM `movie` WHERE `releaseyear` = @releaseyear";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@releaseyear",
                DbType = DbType.Int32,
                Value = releaseyear,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Movie> FindOneAsyncFromGenreType(string genre_type)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `title`, `director`, `releaseyear`, `genre_type` FROM `movie` WHERE `genre_type` = @genre_type";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@genre_type",
                DbType = DbType.Int32,
                Value = genre_type,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Movie>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `title`, `director`, `releaseyear`, `genre_type` FROM `movie` ORDER BY `id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `movie`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Movie>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Movie>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Movie(Db)
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Director = reader.GetString(2),
                        ReleaseYear = reader.GetInt32(3),
                        GenreType = reader.GetString(4),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
