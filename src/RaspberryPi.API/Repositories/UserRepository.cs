using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Repositories
{
    public static class UserRepository
    {
        public static AspNetUser? Get(string username, string password)
        {
            var users = new List<AspNetUser>()
            {
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    Username = "root",
                    Password = "root",
                    Role = "root"
                },
                new AspNetUser
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