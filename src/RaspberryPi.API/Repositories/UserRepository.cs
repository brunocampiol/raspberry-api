using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Repositories
{
    public static class UserRepository
    {
        public static User? Get(string username, string password)
        {
            var users = new List<User>()
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "root",
                    Password = "root",
                    Role = "root"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Password = "admin",
                    Role = "admin"
                }
            };

            return users.Find(x => x.Username.ToUpperInvariant() == username.ToUpperInvariant() && x.Password == password);
        }
    }
}