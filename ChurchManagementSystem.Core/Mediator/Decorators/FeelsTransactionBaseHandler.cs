using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Mediator;

using System;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Mediator.Decorators
{
    public class DataContextAttribute : Attribute
    {
    }

    // DEV NOTE: DO NOT alter the below classes unless you really understand the architecture.
    public class ChurchManagementSystemTransactionBaseHandler<TRequest, TResult> where TResult : Response, new()
    {
        private readonly DataContext _context;

        public ChurchManagementSystemTransactionBaseHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<TResult> HandleAsync(TRequest request, Func<Task<TResult>> processRequest)
        {
            if (_context.Database.CurrentTransaction != null)
            {
                return await processRequest();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var result = await processRequest();
                if (result.HasErrors)
                {
                    transaction.Rollback();
                    return result;
                }

                transaction.Commit();
                return result;
            }
        }
    }  
}