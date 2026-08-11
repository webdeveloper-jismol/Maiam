using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Maiam_DAL.DAL
{
    public class MaiamDbContext : DbContext
    {
        public MaiamDbContext(DbContextOptions<MaiamDbContext> options)
            : base(options)
        {
        }
    }
}