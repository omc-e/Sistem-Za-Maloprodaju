﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.Internal.DataAccess
{
    internal class SQLDataAccess : IDisposable
    {

        public string GetConnectionString (string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

         public List<T> LoadData <T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T> (storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList ();

                return rows; 
            }
        }

        public void SaveData <T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure);

                
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            
                _connection.Execute(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure, transaction: _transaction);
 
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
           
                List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

                return rows;
            
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            CommitTransaction();
        }

        //Open connect/start transaction method
        //Load using the transaction 
        //Save using the transaction
        //Close connection/stop transaction method
        //Dispose 

    }
}
