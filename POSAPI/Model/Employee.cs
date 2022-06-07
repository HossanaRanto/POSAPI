namespace POSAPI.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public UserClient User { get; set; }
        public bool Permission { get; set; }//true: admin; false: simple employee

    }
}
