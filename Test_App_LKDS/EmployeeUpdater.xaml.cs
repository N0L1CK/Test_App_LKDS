﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Test_App_LKDS
{
    /// <summary>
    /// Логика взаимодействия для EmployeeUpdater.xaml
    /// </summary>
    public partial class EmployeeUpdater : Window
    {
        string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;
        Employee Employee;

        ILogger logger = null;
        public EmployeeUpdater(Employee employee, ILogger _logger)
        {
            logger = _logger;
            InitializeComponent();
            Employee = employee;
            NameEmployee.Text = Employee.Name;
            SurnameEmployee.Text = Employee.Surname;
            PhotoEmployee.Text = Employee.Photo;
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            OperationsEmployee("UPDATE Employees SET Name = '" + NameEmployee.Text 
                + "', Surname = '" + SurnameEmployee.Text 
                + "', Photo = '" + PhotoEmployee.Text 
                + "' WHERE Id = " + Employee.Id);


            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            logger.Log("Cancel update Employee");
            Close();
        }

        private void ButtonDelene_Click(object sender, RoutedEventArgs e)
        {
            OperationsEmployee("DELETE FROM Employees WHERE Id = " + Employee.Id);
  
            Close();
        }
        private void OperationsEmployee(string command)
        {
            using (SqlConnection connection = new SqlConnection(NewconnectionString))
            {
                connection.Open();
                SqlCommand someCommand = new SqlCommand(command, connection);
                someCommand.ExecuteNonQuery();
                logger.Log(command);
            }
        }
    }
}