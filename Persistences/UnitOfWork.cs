using System;
using System.Collections.Generic;
using System.Text;
using Applications;

namespace Persistences
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public int Commit()
        {
            return context.SaveChanges();
        }


        public void BeginTransaction()
        {
            context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            context.Database.RollbackTransaction();
        }
    }
}
