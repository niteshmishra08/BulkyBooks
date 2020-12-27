using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
   public interface IunitOfWork: IDisposable
    {
        ICategoryRepository Category { get; }

        ISP_Call SP_Call { get; }

        void Save();
    }
}
