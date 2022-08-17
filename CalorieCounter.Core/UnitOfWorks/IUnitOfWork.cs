using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task CommitAsyncWithUser(string id);
        void Commit();

    }
}
