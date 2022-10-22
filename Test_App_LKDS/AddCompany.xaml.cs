using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Test_App_LKDS
{
    /// <summary>
    /// Логика взаимодействия для AddCompany.xaml
    /// </summary>
    public partial class AddCompany : Window
    {
        readonly List<Company> CompanyList = new List<Company>(10);
        readonly string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;
        readonly ILogger _logger;
        public AddCompany(ILogger logger)
        {
            
            InitializeComponent();
            _logger = logger;
            for (int i = 0; i < 10; i++) 
            {
                Company company = new Company(null);
                CompanyList.Add(company);
            }
            GridAddCompany.ItemsSource = CompanyList;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GridAddCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in CompanyList) 
            {
                if (item.Name != null && item.Name.Length > 1)
                    CompanyInsert("INSERT INTO Organizations (Name) VALUES ('" + item.Name + "')");
            }
            Close();
        }
        private Task CompanyInsert(string command)
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
    }
}
