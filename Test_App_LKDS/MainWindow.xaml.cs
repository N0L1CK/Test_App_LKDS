using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Test_App_LKDS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlDataAdapter adapter;
        DataTable table;
        private int SelectCompanyIndex = 0;
        const string path = "log.txt";
        readonly string NewconnectionString = ConfigurationManager.ConnectionStrings["NewConnection"].ConnectionString;
        readonly TextLogger logger = new TextLogger(path);

        public MainWindow()
        {

            InitializeComponent();
            logger.Log("Start");
            GridCompanys.IsReadOnly = true;
            GridEmploees.IsReadOnly = true;
            _ = new InitialDB(logger);
            try
            {
                LoadOrganization("Select * FROM Organizations");

            }
            catch (Exception ex)
            {
                logger.Log(ex.Message.ToString());
            }

        }



        /// <summary>
        /// Загрузка списка компаний
        /// </summary>
        private void LoadOrganization(string command)
        {
            using (SqlConnection connection = new SqlConnection(NewconnectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(command, connection);
                table = new DataTable();
                adapter.Fill(table);
                GridCompanys.ItemsSource = table.DefaultView;
                logger.Log(command);
            }
        }


        /// <summary>
        /// Загрузка списка сотрудников
        /// </summary>
        private Task LoadEmployees(string command)
        {
            using (SqlConnection connection = new SqlConnection(NewconnectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(command, connection);
                table = new DataTable();
                adapter.Fill(table);
                GridEmploees.ItemsSource = table.DefaultView;
                logger.Log(command);
            }

            return Task.CompletedTask;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// Вызов View для добавления компаний
        /// </summary>
        private void AddCompanys_Click(object sender, RoutedEventArgs e)
        {
            AddCompany addCompany = new AddCompany(logger);
            addCompany.Show();
            addCompany.Closed += AddCompany_Closed;
        }

        private void AddEmploees_Closed(object sender, EventArgs e)
        {
            LoadEmployees("Select * FROM Employees WHERE Employees.OrganizationId = " + SelectCompanyIndex.ToString());
        }

        /// <summary>
        /// Обновление списка компании при закрытии View добавления компаний
        /// </summary>
        private void AddCompany_Closed(object sender, EventArgs e)
        {
            LoadOrganization("Select * FROM Organizations");
        }


        /// <summary>
        /// Поиск сотрудника по Имени или Фамилии в выбраной компании
        /// </summary>
        private void SearchEmploee_Click(object sender, RoutedEventArgs e)
        {

            if (SearchName.Text.Length > 1)
            {
                LoadEmployees("Select * FROM Employees WHERE "
                    + "Employees.OrganizationId = " + SelectCompanyIndex.ToString()
                    + " AND Employees.Name LIKE '%" + SearchName.Text.ToString()
                    + "%' OR Employees.Surname LIKE '%" + SearchName.Text.ToString() + "%'");

            }
            else
            {
                LoadEmployees("Select * FROM Employees WHERE Employees.OrganizationId = " + SelectCompanyIndex.ToString());

            }
            SearchName.Text = "";
        }


        /// <summary>
        /// Выбор компании
        /// </summary>
        private void GridCompanys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                SelectCompanyIndex = (int)((DataRowView)GridCompanys.SelectedItems[0]).Row["Id"];
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message.ToString());
            }
            LoadEmployees("Select * FROM Employees WHERE Employees.OrganizationId = " + SelectCompanyIndex.ToString());
        }

        /// <summary>
        /// Выбор сотрудника
        /// </summary>
        private void GridEmpoyee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textName.Text = "";
            textSurname.Text = "";
            PhotoEmploee.Source = null;
            try
            {
                int SelectEmploeeIndex = (int)((DataRowView)GridEmploees.SelectedItems[0]).Row["Id"];
                textName.Text = (string)((DataRowView)GridEmploees.SelectedItems[0]).Row["Name"];
                textSurname.Text = (string)((DataRowView)GridEmploees.SelectedItems[0]).Row["Surname"];
            }
            catch (Exception ex)
            {

                logger.Log(ex.Message.ToString());
            }

            GridEmployees_GetPhoto();

        }



        /// <summary>
        /// Вызов View для добавления сотрудников в выбранную компанию
        /// </summary>
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployees addEmployees = new AddEmployees(SelectCompanyIndex, logger);
            addEmployees.Show();
            addEmployees.Closed += AddEmploees_Closed;
        }




        /// <summary>
        /// Загрузка Фото Выбраного сотрудника 
        /// </summary>
        private void GridEmployees_GetPhoto()
        {
            string photo;
            try
            {
                photo = (string)((DataRowView)GridEmploees.SelectedItems[0]).Row["Photo"];
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message.ToString());
                photo = null;
            }
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            try
            {
                myBitmapImage.UriSource = new Uri(photo);
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message.ToString());
                photo = "Image/1241.jpg";
                myBitmapImage.UriSource = new Uri(photo, UriKind.Relative);
            }
            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();
            PhotoEmploee.Source = myBitmapImage;

        }

        /// <summary>
        /// Выбор сотрудника для редактирования
        /// </summary>
        private void GridEmploees_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee employee = new Employee();
            try
            {
                employee.Id = (int)((DataRowView)GridEmploees.SelectedItems[0]).Row["Id"];
                employee.Name = (string)((DataRowView)GridEmploees.SelectedItems[0]).Row["Name"];
                employee.Surname = (string)((DataRowView)GridEmploees.SelectedItems[0]).Row["Surname"];
                employee.Photo = (string)((DataRowView)GridEmploees.SelectedItems[0]).Row["Photo"];
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message.ToString());
            }

            EmployeeUpdater employeeUpdater = new EmployeeUpdater(employee, logger);
            employeeUpdater.Show();
            employeeUpdater.Closed += AddEmploees_Closed;
        }




        /// <summary>
        /// Вызов View  для изменения компании
        /// </summary>
        private void GridCompany_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectCompanyIndex = (int)((DataRowView)GridCompanys.SelectedItems[0]).Row["Id"];
            string company = (string)((DataRowView)GridCompanys.SelectedItems[0]).Row["Name"];
            CompanyUpdater companyUpdater = new CompanyUpdater(company, SelectCompanyIndex, logger);
            companyUpdater.Show();
            companyUpdater.Closed += AddCompany_Closed;
        }
    }
}
