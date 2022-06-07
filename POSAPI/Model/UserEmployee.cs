using System.Text.Json.Serialization;

namespace POSAPI.Model
{
    public class UserEmployee
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public Employee Employee { get; set; }
    }
}
