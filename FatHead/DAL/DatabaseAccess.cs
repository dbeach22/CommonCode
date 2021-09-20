using FatHead.Converters.Interfaces;
using FatHead.DAL.Interfaces;
using FatHead.Enums;
using FatHead.Loggers;
using FatHead.Loggers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FatHead.DAL
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private IDbConnection _dbConnection;
        private IDbCommand _dbCommand;
        private IDbDataAdapter _dbDataAdapter;
        private IDataConverter _dataConverter;
        private ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnection">System.Data.IDbConnection</param>
        /// <param name="dbCommand">System.Data.IDbCommand</param>
        /// <param name="logger">FatHead.Loggers.Interfaces.ILogger</param>
        public DatabaseAccess(IDbConnection dbConnection, IDbCommand dbCommand, ILogger logger)
        {
            _dbConnection = dbConnection;
            _dbCommand = dbCommand;
            _logger = logger;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnection">System.Data.IDbConnection</param>
        /// <param name="dbCommand">System.Data.IDbCommand</param>
        /// <param name="dataAdapter">System.Data.IDbDataAdapter</param>
        /// <param name="logger">FatHead.Loggers.Interfaces.ILogger</param>
        /// <param name="dataConverter">FatHead.Converters.Interfaces.ILogger</param>
        public DatabaseAccess(IDbConnection dbConnection, IDbCommand dbCommand, IDbDataAdapter dataAdapter, ILogger logger, IDataConverter dataConverter)
        {
            _dbConnection = dbConnection;
            _dbCommand = dbCommand;
            _dbDataAdapter = dataAdapter;
            _logger = logger;
            _dataConverter = dataConverter;
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <returns>List of T</returns>
        public IList<T> QueryList<T>() where T : class, new()
        {
            DataSet ds = new DataSet();
            IList<T> records = new List<T>();

            try
            {
                using (_dbConnection)
                {
                    _dbCommand.Connection = _dbConnection;
                    _dbDataAdapter.SelectCommand = _dbCommand;
                    _dbDataAdapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            if (ds.Tables.Count > 0)
            {
                records = _dataConverter.ConvertModelFromDataTable<T>(ds.Tables[0]);
            }

            return records;
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable QueryDataTable()
        {
            DataSet ds = new DataSet();

            try
            {
                using (_dbConnection)
                {
                    _dbCommand.Connection = _dbConnection;
                    _dbDataAdapter.SelectCommand = _dbCommand;
                    _dbDataAdapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }

        /// <summary>
        /// Executes a query async
        /// </summary>
        /// <returns>List of T</returns>
        public async Task<IList<T>> QueryListAsync<T>() where T : class, new()
        {
            DataSet ds = new DataSet();
            IList<T> records = new List<T>();

            try
            {
                await Task.Run(() =>
                {
                    using (_dbConnection)
                    {
                        _dbCommand.Connection = _dbConnection;
                        _dbDataAdapter.SelectCommand = _dbCommand;
                        _dbDataAdapter.Fill(ds);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            if (ds.Tables.Count > 0)
            {
                records = _dataConverter.ConvertModelFromDataTable<T>(ds.Tables[0]);
            }

            return records;
        }

        /// <summary>
        /// Executes a query async
        /// </summary>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryDataTableAsync()
        {
            DataSet ds = new DataSet();

            try
            {
                await Task.Run(() =>
                {
                    using (_dbConnection)
                    {
                        _dbCommand.Connection = _dbConnection;
                        _dbDataAdapter.SelectCommand = _dbCommand;
                        _dbDataAdapter.Fill(ds);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }

        /// <summary>
        /// Executes a non query command
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public int Execute()
        {
            int result = 0;

            try
            {
                using (_dbConnection)
                {
                    _dbConnection.Open();
                    _dbCommand.Connection = _dbConnection;
                    result = _dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }
            finally
            {
                _dbConnection.Close();
            }

            return result;
        }

        /// <summary>
        /// Executes a non query command async
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public async Task<int> ExecuteAsync()
        {
            int result = 0;

            try
            {
                await Task.Run(() =>
                {
                    using (_dbConnection)
                    {
                        _dbConnection.Open();

                        _dbCommand.Connection = _dbConnection;
                        result = _dbCommand.ExecuteNonQuery();
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }
            finally
            {
                _dbConnection.Close();
            }

            return result;
        }
    }
}
