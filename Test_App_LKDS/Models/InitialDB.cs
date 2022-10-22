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
                InitializeDB(connectionString);
                _logger.Log("InitializeDB");
            }
            catch (Exception e) 
            { 
                _logger.Log("InitializeDB error: " + e.Message.ToString());
            }
            
            string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;

            try
            {
                InitializeOrganizationTable(NewconnectionString);
                _logger.Log("InitializeOrganizationTable");
            }
            catch (Exception e)
            {
                _logger.Log("InitializeOrganizationTable error: " + e.Message.ToString());
            }
            try
            {
                InitializeEmployeeTable(NewconnectionString);
                _logger.Log("InitializeEmployeeTable");
            }
            catch (Exception e) 
            {
                _logger.Log("InitializeEmployeeTable error: " + e.Message.ToString());
            }
            

            
        }
        private void InitializeDB(string connectionString)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE DATABASE companys_db";
                command.Connection = connection;
                command.ExecuteNonQuery();
                Console.WriteLine("База дынных создана");
                connection.Close();
                
            }
           
        }
        private void InitializeOrganizationTable(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE TABLE Organizations (Id INT PRIMARY KEY IDENTITY,Name NVARCHAR(100) NOT NULL)";
                command.Connection = connection;
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица Organizations создана");
                connection.Close();
            }
        }
        private void InitializeEmployeeTable(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE TABLE Employees (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100) NOT NULL, Surname NVARCHAR(100) NOT NULL, Photo NVARCHAR(100), OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Organizations(Id))";
                command.Connection = connection;
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица Employees создана");
                connection.Close();
            }
        }
    }
}
