namespace POSAPI.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }
        public int UserProfilId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public bool Subscribe { get; set; }
    }
}
