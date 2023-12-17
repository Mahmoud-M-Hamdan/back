using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<UserModel> Users { get; set; }
    }
}