
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;


namespace Test_App_LKDS
{
    /// <summary>
    /// Логика взаимодействия для AddEmployees.xaml
    /// </summary>
    public partial class AddEmployees : Window
    {
        readonly int IdComp = 0;
        readonly List<Employee> EmployeeList = new List<Employee>(100);
        readonly string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;
        readonly ILogger _logger;
        public AddEmployees(int idComp, ILogger logger)
        {

            InitializeComponent();
            _logger = logger;
            for (int i = 0; i < 100; i++)
            {
                Employee employee = new Employee(null, null, null);
                EmployeeList.Add(employee);
            }
            GridEmployeesAdd.ItemsSource = EmployeeList;
            IdComp = idComp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in EmployeeList)
            {
                try
                {
                    if (item.Name != null && item.Name.Length > 0 ||
                        item.Surname != null && item.Surname.Length > 0)
                        EmployeesInsert("INSERT INTO Employees (Name, Surname, Photo, OrganizationId) " +
                            "VALUES ('" + item.Name + "', '" + item.Surname + "', '" +
                            item.Photo + "', '" + IdComp + "')");

                }
                catch (Exception ex)
                {
                    _logger.Log(ex.Message.ToString());
                }
            }
            EmployeeList.Clear();
            Close();
        }
        private Task EmployeesInsert(string command)
        {
            using (SqlConnection connection = new SqlConnection(NewconnectionString))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(command, connection);
                int number = com.ExecuteNonQuery();
                _logger.Log(command);

            }

            return Task.CompletedTask;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmployeeList.Clear();
            Close();
        }
    }
}
