using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maiamnew_DAL.DAL
{
    public class Data_Layer
    {
        private readonly MaiamnewDbContext _context;

        public Data_Layer(MaiamnewDbContext context)
        {
            _context = context;
        }
    }
}
