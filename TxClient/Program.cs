using System;
using System.Transactions;
using TxClient.TxServiceBridge;

namespace TxClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                TxServiceClient proxy = new TxServiceClient();

                TransactionInformation info = Transaction.Current.TransactionInformation;

                string localId = String.Empty;
                Guid distributedId = Guid.Empty;

                localId = info.LocalIdentifier;
                distributedId = info.DistributedIdentifier;

                proxy.TxOperation();

                localId = info.LocalIdentifier;
                distributedId = info.DistributedIdentifier;

                scope.Complete();
            }
        }
    }
}
