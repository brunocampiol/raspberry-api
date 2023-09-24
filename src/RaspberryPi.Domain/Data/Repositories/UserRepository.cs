using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Data.Repositories
{
    public static class UserRepository
    {
        public static AspNetUser? Get(string email, string password)
        {
            var users = new List<AspNetUser>()
            {
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    Email = "root",
                    Password = "root",
                    Role = "root"
                },
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Password = "admin",
                    Role = "admin"
                }
            };

            return users.Find(x => x.Email.ToUpperInvariant() == email.ToUpperInvariant() && x.Password == password);
        }
    }
}