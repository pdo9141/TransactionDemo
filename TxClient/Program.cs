using System;
using System.Data.SqlClient;
using System.Messaging;
using System.Transactions;
using TxClient.TxServiceBridge;

namespace TxClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    /*
                    * To enable WSAtomic protocol, enable WS-AT in DTC properties
                    * Run regasm.exe /codebase wsatui.dll
                    * Resolve cert issue with client authentication access
                    * TxServiceClient proxy = new TxServiceClient("WSHttpBinding_ITxService");                    
                    */
                    TxServiceClient proxy = new TxServiceClient("NetTcpBinding_ITxService");

                    TransactionInformation info = Transaction.Current.TransactionInformation;

                    string localId = String.Empty;
                    Guid distributedId = Guid.Empty;

                    localId = info.LocalIdentifier;
                    distributedId = info.DistributedIdentifier;

                    DBOperation1();

                    proxy.TxOperation();

                    localId = info.LocalIdentifier;
                    distributedId = info.DistributedIdentifier;

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            
        }

        private static void DBOperation1()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection("Data Source=WayTooGonzo;Initial Catalog=Demos;Integrated Security=True");
            cmd = new SqlCommand("INSERT INTO tblDemo VALUES ('Phillip Do', 'Handsome guy', 900)", cnn);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private static void DBOperation2()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection(@"Data Source = WayTooGonzo\SQLEXPRESS; Initial Catalog = Demos; Integrated Security = True");
            cmd = new SqlCommand("INSERT INTO tblDemo VALUES ('Phi Do', 'Funny guy', 1800)", cnn);

            cmd.Connection.Open();
            //throw new Exception("test");
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private static void MSMQOperation()
        {
            MessageQueue queue = null;
            queue = new MessageQueue();
            queue.Path = @".\Private$\DemoQueue";
            queue.Send("Message Body", "Message Label", MessageQueueTransactionType.Automatic);
        }
    }
}
