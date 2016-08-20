using System;
using System.Data.SqlClient;
using System.Messaging;
using System.Transactions;
using VolatileResourceManager;

namespace NETTransactions
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*
                using (TransactionScope scope = new TransactionScope())
                {
                    Transaction t = Transaction.Current;
                    Console.WriteLine("Isolation: " + t.IsolationLevel);
                    Console.WriteLine("");

                    DBOperation1();
                    Console.WriteLine("Call to 1st Durable RM");
                    Console.WriteLine("Local Identifier: " + t.TransactionInformation.LocalIdentifier);
                    Console.WriteLine("Distributed Identifier: " + t.TransactionInformation.DistributedIdentifier);
                    Console.WriteLine("");

                    DBOperation2();
                    Console.WriteLine("Call to 2nd Durable RM");
                    Console.WriteLine("Local Identifier: " + t.TransactionInformation.LocalIdentifier);
                    Console.WriteLine("Distributed Identifier: " + t.TransactionInformation.DistributedIdentifier);
                    Console.WriteLine("");

                    scope.Complete();
                    Console.WriteLine("Transaction Completed...");
                }
                */

                /*
                using (TransactionScope scope = new TransactionScope())
                {
                    MSMQOperation();
                    scope.Complete();
                    Console.WriteLine("Transaction Completed...");
                }
                */

                Transactional<int[]> numbers1 = new Transactional<int[]>(new int[3]);
                Transactional<int[]> numbers2 = new Transactional<int[]>(new int[2]);

                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        VolatileOperation(numbers1);
                        VolatileOperation(numbers2);
                        scope.Complete();
                        Console.WriteLine("Transaction Completed...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Value is: " + numbers1.Value[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Console.ReadLine();
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

        private static void VolatileOperation(Transactional<int[]> numbers)
        {
            numbers.Value[0] = 11;
            numbers.Value[1] = 22;
            numbers.Value[2] = 33;
        }
    }
}
