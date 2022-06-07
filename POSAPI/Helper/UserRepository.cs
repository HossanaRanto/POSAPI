namespace POSAPI.Data
{
    public class UserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }
    }
}
