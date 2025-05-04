using AppZK9500.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZK9500.Data
{
    public class MiDbContext:DbContext
    {
        public DbSet<PersonModel> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Settings.Default.BDServer);
        }
    }
}
