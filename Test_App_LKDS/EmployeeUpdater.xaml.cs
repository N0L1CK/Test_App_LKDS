using System.Configuration;
using System.Data.SqlClient;
using System.Windows;


namespace Test_App_LKDS
{
    /// <summary>
    /// Логика взаимодействия для EmployeeUpdater.xaml
    /// </summary>
    public partial class EmployeeUpdater : Window
    {
        readonly string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;
        readonly Employee Employee;

        readonly ILogger logger;
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
