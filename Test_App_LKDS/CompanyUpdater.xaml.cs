using System.Data.SqlClient;
using System.Windows;
using System.Configuration;
using System;

namespace Test_App_LKDS
{
    /// <summary>
    /// Логика взаимодействия для CompanyUpdater.xaml
    /// </summary>
    public partial class CompanyUpdater : Window
    {
        readonly string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;
        readonly int Id;
        readonly string NameCompany;
        readonly ILogger _logger;
        public CompanyUpdater(string nameCompany, int id, ILogger logger)
        {
            InitializeComponent();
            NameCompany = nameCompany;
            CompanyName.Text = NameCompany;
            Id = id;
            _logger = logger;
        }

        /// <summary>
        /// Изменение название компании
        /// </summary>

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OperationsCompany("UPDATE Organizations SET Name = '" + CompanyName.Text + "' WHERE Id = " + Id);
            Close();
        }
        /// <summary>
        /// Отмена
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Удаление компании и сотрудников компании
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                OperationsCompany("DELETE FROM Employees WHERE OrganizationId = " + Id);

                OperationsCompany("DELETE FROM Organizations WHERE Id = " + Id);
                _logger.Log("Company " + CompanyName.Text + " deleted");
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            Close();
        }


        /// <summary>
        /// Функция исполения sql запросов
        /// </summary>
        private void OperationsCompany(string command) 
        {
            using (SqlConnection connection = new SqlConnection(NewconnectionString))
            {
                connection.Open();
                SqlCommand someCommand = new SqlCommand(command, connection);
                someCommand.ExecuteNonQuery();
                _logger.Log(command);
            }
        }   
    }
}
