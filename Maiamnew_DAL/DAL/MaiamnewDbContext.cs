using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Maiamnew_DAL.DAL
{
    public class MaiamnewDbContext : DbContext
    {
        public MaiamnewDbContext(DbContextOptions<MaiamnewDbContext> options)
            : base(options)
        {
        }
    }
}
