using System;
using System.Configuration;
using System.Data.SqlClient;


namespace Test_App_LKDS
{
    internal class InitialDB
    {
        ILogger _logger;
        public InitialDB(ILogger loger)
        {
            _logger = loger;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Initialize(connectionString);

        }

        private void Initialize(string connectionString)
        {
            try
            {
                InitializeDB(connectionString, "CREATE DATABASE companys_db");

            }
            catch (Exception e)
            {
                _logger.Log("InitializeDB error: " + e.Message.ToString());
            }

            string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;

            try
            {
                InitializeDB(NewconnectionString, "CREATE TABLE Organizations " +
                    "(Id INT PRIMARY KEY IDENTITY,Name NVARCHAR(100) NOT NULL)");

            }
            catch (Exception e)
            {
                _logger.Log("InitializeOrganizationTable error: " + e.Message.ToString());
            }
            try
            {
                InitializeDB(NewconnectionString, "CREATE TABLE Employees (Id INT PRIMARY KEY IDENTITY, " +
                    "Name NVARCHAR(100) NOT NULL, Surname NVARCHAR(100) NOT NULL, Photo NVARCHAR(100), " +
                    "OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Organizations(Id))");

            }
            catch (Exception e)
            {
                _logger.Log("InitializeEmployeeTable error: " + e.Message.ToString());
            }



        }
        private void InitializeDB(string connectionString, string sqlCommand)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = sqlCommand;
                command.Connection = connection;
                command.ExecuteNonQuery();
                connection.Close();
                _logger.Log(sqlCommand);
            }

        }
    }
}
