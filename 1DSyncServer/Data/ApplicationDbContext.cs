using _1DSync.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1DSyncServer.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<PseudoDynamicEntity> PseudoDynamicEntities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
    }
}
