using ApiVerifyEmailForgotPassword.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVerifyEmailForgotPassword.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> users => Set<User>();
    }
}
