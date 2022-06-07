namespace POSAPI.Model
{
    public class Client
    {
        public int Id { get; set; }
        public UserProfil Profil { get; set; }
        public bool Subscribe { get; set; }
        public Client()
        {

        }
        public Client(UserProfil profil)
        {
            this.Profil = profil;
        }
    }
}
