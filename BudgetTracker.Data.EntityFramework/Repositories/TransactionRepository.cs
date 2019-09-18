using BudgetTracker.Business.Transactions;
using BudgetTracker.Data.EntityFramework;
using BudgetTracker.Data.EntityFramework.Converters;
using BudgetTracker.Data.EntityFramework.Models;
using BudgetTracker.Business.Ports.Exceptions;
using BudgetTracker.Business.Ports.Repositories;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Data.EntityFramework.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly BudgetTrackerContext _dbContext;

        public TransactionRepository(BudgetTrackerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            TransactionModel newTransaction = TransactionConverter.Convert(transaction);

            await _dbContext.Transactions.AddAsync(newTransaction);
            int recordSaved = await _dbContext.SaveChangesAsync();

            if(recordSaved < 1)
            {
                throw new RepositoryException("Could not save transaction to database");
            }
            return TransactionConverter.Convert(newTransaction);
        }
    }
}
