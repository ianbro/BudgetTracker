using System;

namespace BudgetTracker.TestUtils.Transactions
{
    public class TransactionBuilderFactory
    {
        public TransactionBuilder GetBuilder()
        {
            return new TransactionBuilder();
        }

        public TransactionMessageBuilder GetMessageBuilder()
        {
            return new TransactionMessageBuilder();
        }
    }
}
