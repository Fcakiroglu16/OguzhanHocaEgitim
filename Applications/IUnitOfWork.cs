using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Applications
{
    public interface IUnitOfWork
    {
        int Commit();

        void BeginTransaction();


        public void CommitTransaction();


        public void RollbackTransaction();
    }
}
