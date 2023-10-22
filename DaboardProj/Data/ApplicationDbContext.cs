using Microsoft.EntityFrameworkCore;
using DaboardProj.Models;
using Newtonsoft.Json.Linq;

namespace DaboardProj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Visiter> Visiters { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string json = System.IO.File.ReadAllText("appsettings.json");
            JObject jsonObject = JObject.Parse(json);
            var connectionString = jsonObject["ConnectionStrings"]["ApplicationDbContextConnection"].ToString();
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
