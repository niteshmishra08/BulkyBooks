using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
   public interface IunitOfWork: IDisposable
    {
        ICategoryRepository Category { get; }

        IProductRepository Product { get; }

        ISP_Call SP_Call { get; }

        ICoverTypeRepository CoverType { get; }

        void Save();
    }
}
