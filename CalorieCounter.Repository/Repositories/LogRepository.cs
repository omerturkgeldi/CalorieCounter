using CalorieCounter.Core.Repositories;
using CalorieCounter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Repository.Repositories
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        public LogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
