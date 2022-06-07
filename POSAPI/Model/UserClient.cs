using System.Text.Json.Serialization;

namespace POSAPI.Model
{
    public class UserClient
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public Client Client { get; set; }
    }
}
