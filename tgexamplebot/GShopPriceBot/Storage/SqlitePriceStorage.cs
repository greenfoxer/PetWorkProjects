using System;
using System.Collections.Generic;
using System.Text;
using GShopPriceBot.Extensions;
using GShopPriceBot.Models;
using Microsoft.Data.Sqlite;
using Serilog;

namespace GShopPriceBot.Storage
{
    /// <summary>
    /// Sqlite price storage
    /// </summary>
    public class SqlitePriceStorage : IPriceStorage
    {
        private static readonly string _connectionString = @"Data Source=/app/data/prices.db3";
        private readonly ILogger _log;
        public SqlitePriceStorage(ILogger log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(SqlitePriceStorage));

            _log = log.ForContext<SqlitePriceStorage>();
        }

        /// <inheritdoc />
        public IEnumerable<PricePosition> Find(string namePart, string category, int limit)
        {
            var query =
                $@"SELECT 
                    `{nameof(PricePosition.Name)}`
                    , `{nameof(PricePosition.RetailPrice)}`
                    , `{nameof(PricePosition.BundlePrice)}`
                    , `{nameof(PricePosition.Category)}`
                    , `{nameof(PricePosition.SubCategory)}`
                    , `{nameof(PricePosition.CurrencyRate)}`
                    , `{nameof(PricePosition.IsPriceChanged)}`
                    , `{nameof(PricePosition.CreatedAt)}`
                FROM `{nameof(PricePosition)}`
                WHERE 1 = 1
                    AND `{nameof(PricePosition.Name)}` LIKE @pattern 
                ORDER BY `{nameof(PricePosition.CreatedAt)}` DESC
                    , `{nameof(PricePosition.Category)}`
                    , `{nameof(PricePosition.Name)}`
                LIMIT {limit}";

            var con = new SqliteConnection(_connectionString);
            var result = new List<PricePosition>();

            try
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@pattern", namePart.Prepare());

                _log.Debug("Executing query {query}", query);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PricePosition
                        {
                            Name = reader.GetString(0),
                            RetailPrice = reader.GetDecimal(1),
                            BundlePrice = reader.GetDecimal(2),
                            Category = reader.GetString(3),
                            SubCategory = reader.GetString(4),
                            CurrencyRate = reader.GetDecimal(5),
                            IsPriceChanged = reader.GetBoolean(6),
                            CreatedAt = reader.GetDateTime(7)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Query error.");
            }
            finally
            {
                con.Dispose();
            }

            return result;
        }

        /// <inheritdoc />
        public void Init()
        {
            _log.Warning("Initializing SQLITE storage...");

            // create PricePosition table
            var createPricePositionsTableQuery =
                $@"CREATE TABLE IF NOT EXISTS `{nameof(PricePosition)}` (
                    `{nameof(PricePosition.Name)}` TEXT NOT NULL,
                    `{nameof(PricePosition.RetailPrice)}` REAL NOT NULL,
                    `{nameof(PricePosition.BundlePrice)}` REAL NOT NULL,
                    `{nameof(PricePosition.Category)}` TEXT NOT NULL,
                    `{nameof(PricePosition.SubCategory)}` TEXT,
                    `{nameof(PricePosition.CurrencyRate)}` REAL NOT NULL,
                    `{nameof(PricePosition.IsPriceChanged)}` INTEGER NOT NULL,
                    `{nameof(PricePosition.CreatedAt)}` TEXT NOT NULL
                )";

            // create UserQuery table
            var createUserQueryTableQuery =
                $@"CREATE TABLE IF NOT EXISTS `{nameof(UserQuery)}` (
                    `{nameof(UserQuery.UserId)}` INTEGER NOT NULL,
                    `{nameof(UserQuery.UserName)}` TEXT,
                    `{nameof(UserQuery.FirstName)}` TEXT,
                    `{nameof(UserQuery.LastName)}` TEXT,
                    `{nameof(UserQuery.Query)}` TEXT
                )";

            var con = new SqliteConnection(_connectionString);

            try
            {
                con.Open();

                var cmd = con.CreateCommand();
                
                _log.Debug("Creating table {query}", createPricePositionsTableQuery);
                cmd.CommandText = createPricePositionsTableQuery;
                cmd.ExecuteNonQuery();

                _log.Debug("Creating table {query}", createUserQueryTableQuery);
                cmd.CommandText = createUserQueryTableQuery;
                cmd.ExecuteNonQuery();

                _log.Warning("Storage was successfuly initialized.");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Storage initialization error.");
            }
            finally
            {
                con.Dispose();
            }
        }

        /// <inheritdoc />
        public void InsertAll_old(PricePosition[] positions)
        {
            _log.Information("Inserting a new positions...");

            var sb = new StringBuilder(8192);

            sb.AppendLine($"INSERT INTO `{nameof(PricePosition)}` VALUES ");

            for (var positionId = 0; positionId < positions.Length; positionId++)
            {
                if (positionId != 0)
                    sb.Append(",  ");

                var position = positions[positionId];
                sb.AppendLine($@"(
                                '{position.Name.Sanitize()}'
                                , {position.RetailPrice}
                                , {position.BundlePrice}
                                , '{position.Category.Sanitize()}'
                                , '{(position.SubCategory?.Sanitize() ?? "NULL")}'
                                , {position.CurrencyRate}
                                , {(position.IsPriceChanged ? 1 : 0)}
                                , '{position.CreatedAt}'
                );");
            }

            var con = new SqliteConnection(_connectionString);

            try
            {
                con.Open();

                var cmd = con.CreateCommand();
                var tr = con.BeginTransaction();

                cmd.CommandText = sb.ToString();
                cmd.Transaction = tr;

                var inserted = cmd.ExecuteNonQuery();
                tr.Commit();

                _log.Information("Rows inserted {inserted}", inserted);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Insertion error");
            }
            finally
            {
                con.Dispose();
            }
        }

        /// <inheritdoc />
        public void AddPositions(PricePosition[] positions)
        {
            if (positions.Length == 0)
            {
                _log.Warning("No positions to insert");
                return;
            }

            _log.Information("Inserting a new positions...");

            var con = new SqliteConnection(_connectionString);

            try
            {
                con.Open();

                var cmd = con.CreateCommand();

                var sb = new StringBuilder(8192);
                sb.AppendLine("BEGIN TRANSACTION;");
                sb.AppendLine($"INSERT INTO `{nameof(PricePosition)}`");
                sb.AppendLine("VALUES ");

                var inserted = 0;
                foreach (var position in positions)
                {
                    inserted++;

                    sb.AppendLine($@"('{position.Name.Sanitize()}'
                                , {position.RetailPrice}
                                , {position.BundlePrice}
                                , '{position.Category.Sanitize()}'
                                , '{(position.SubCategory?.Sanitize() ?? "NULL")}'
                                , {position.CurrencyRate}
                                , {(position.IsPriceChanged ? 1 : 0)}
                                , '{position.CreatedAt}')");

                    // flush
                    if (inserted % 50 == 0)
                    {
                        sb.Append(';');
                        sb.AppendLine("COMMIT TRANSACTION;");
                        cmd.CommandText = sb.ToString();

                        _log.Debug("Inserting {query}", cmd.CommandText);
                        cmd.ExecuteNonQuery();

                        sb.Clear();
                        sb.AppendLine("BEGIN TRANSACTION;");
                        sb.AppendLine($"INSERT INTO `{nameof(PricePosition)}`");
                        sb.AppendLine("VALUES ");
                    }
                    else
                    {
                        sb.Append(',');
                    }
                }

                // insert last positions
                if (inserted < positions.Length)
                {
                    // remove last comma
                    sb[sb.Length - 1] = ';';
                    sb.Append(";");
                    sb.AppendLine("COMMIT TRANSACTION;");
                    cmd.CommandText = sb.ToString();

                    _log.Debug("Inserting {query}", cmd.CommandText);
                    cmd.ExecuteNonQuery();
                }

                _log.Information("All {count} positions inserted", positions.Length);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Insertion error");
            }
            finally
            {
                con.Dispose();
            }
        }

        /// <inheritdoc />
        public void AddUserQuery(UserQuery query)
        {
            _log.Information("Inserting the user query...");

            var con = new SqliteConnection(_connectionString);

            try
            {
                con.Open();

                var cmd = con.CreateCommand();

                var sb = new StringBuilder(1024);

                sb.AppendLine($"INSERT INTO `{nameof(UserQuery)}`");
                sb.AppendLine($@"VALUES (
                            {query.UserId}
                            , '{(query.UserName?.Sanitize() ?? "")}'
                            , '{(query.FirstName?.Sanitize() ?? "")}'
                            , '{(query.LastName?.Sanitize() ?? "")}'
                            , '{(query.Query?.Sanitize() ?? "")}'
                );");

                cmd.CommandText = sb.ToString();
                _log.Debug("Inserting {query}", cmd.CommandText);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Insertion error");
            }
            finally
            {
                con.Dispose();
            }
        }

        /// <inheritdoc />
        public void ClearPrices()
        {
            _log.Information("Removing all prices...");

            var con = new SqliteConnection(_connectionString);

            try
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = $"DELETE FROM `{nameof(PricePosition)}`";

                _log.Debug("Deleting {query}", cmd.CommandText);
                cmd.ExecuteNonQuery();
                _log.Information("All prices deleted.");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Deletion error");
            }
            finally
            {
                con.Dispose();
            }
        }

        /// <inheritdoc />
        public IEnumerable<int> GetUniqueUserIds()
        {
            var query =
                $@"SELECT DISTINCT
                    `{nameof(UserQuery.UserId)}`
                FROM `{nameof(UserQuery)}`";

            var con = new SqliteConnection(_connectionString);
            var result = new List<int>();

            try
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = query;

                _log.Debug("Executing query {query}", query);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetInt32(0));
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Query error.");
            }
            finally
            {
                con.Dispose();
            }

            return result;
        }
    }
}
