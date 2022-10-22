

namespace Test_App_LKDS
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }

        public Employee() { }
        public Employee(string name, string surname, string photo)
        {
            Name = name;
            Surname = surname;
            if (photo != null)
            {
                Photo = photo;
            }
            else
            {
                Photo = null;
            }
        }

    }
}
