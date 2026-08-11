using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maiam_DAL.DAL
{
    public class Data_Layer
    {
        private readonly MaiamDbContext _context;

        public Data_Layer(MaiamDbContext context)
        {
            _context = context;
        }
    }
}