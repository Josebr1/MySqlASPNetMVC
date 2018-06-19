using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;

namespace MySqlASPNetMVC.Repositorio
{
    public class Contexto : IDisposable
    {
        private MySqlConnection connection;

        public Contexto() 
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PessoaDB"].ConnectionString;
            connection = new MySqlConnection(connectionString);
        }

        public int ExecuteCommand(string sql, Dictionary<string, object> paramsQSL){
            var result = 0;
            if (string.IsNullOrEmpty(sql)) 
            {
                throw new ArgumentException("O comando não pode ser executado");
            }
            try 
            {
                OpenConnection();
                var command = CreateConnection(sql, paramsQSL);
                result = command.ExecuteNonQuery();
            }
            finally 
            {
                CloseConnection();
            }
            return result;
        }

        public List<Dictionary<string, string>> ExecuteCommandSQL(string sql, Dictionary<string, object> paramsSQL = null)
        {
            List<Dictionary<string, string>> lines = null;

            if(string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("O comando não pode ser executado");
            }
            try 
            {
                OpenConnection();
                var command = CreateConnection(sql, paramsSQL);
                using (var reader = command.ExecuteReader())
                {
                    var line = new Dictionary<string, string>();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var nomeColumn = reader.GetName(i);
                        var valueColumn = reader.IsDBNull(i) ? null : reader.GetName(i);
                        line.Add(nomeColumn, valueColumn);
                    }
                    lines.Add(line);
                }
            }
            finally 
            {
                CloseConnection();
            }
            return lines;
        }

        private MySqlCommand CreateConnection(string sql, Dictionary<string, object> paramsSQL)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            AddParams(command, paramsSQL);
            return command;
        }

        private static void AddParams(MySqlCommand commandSQL, Dictionary<string, object> paramsSQL)
        {
            if (paramsSQL == null)
                return;

            foreach (var item in paramsSQL) 
            {
                var param = commandSQL.CreateParameter();
                param.ParameterName = item.Key;
                param.Value = item.Value ?? DBNull.Value;
                commandSQL.Parameters.Add(param);
            }
        }
        private void OpenConnection() {
            if (connection.State == ConnectionState.Open) return;

            connection.Open();
        }

        private void CloseConnection() 
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public void Dispose()
        {
            if (connection == null) return;

            connection.Dispose();
            connection = null;
        }
    }
}
