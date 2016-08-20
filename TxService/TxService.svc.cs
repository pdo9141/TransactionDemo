using System;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Transactions;

namespace TxService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class TxService : ITxService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool TxOperation()
        {
            NetTcpBinding tcp = (NetTcpBinding)OperationContext.Current.Host.Description.Endpoints[0].Binding;
            TransactionProtocol pro = tcp.TransactionProtocol;

            WSHttpBinding ws = (WSHttpBinding)OperationContext.Current.Host.Description.Endpoints[1].Binding;

            TransactionInformation info = Transaction.Current.TransactionInformation;
            string localId = String.Empty;
            Guid distributedId = Guid.Empty;
            localId = info.LocalIdentifier;
            distributedId = info.DistributedIdentifier;

            /*
            DBOperation1();

            localId = info.LocalIdentifier;
            distributedId = info.DistributedIdentifier;
            */

            DBOperation2();            

            localId = info.LocalIdentifier;
            distributedId = info.DistributedIdentifier;

            return true;
        }

        private void DBOperation1()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection("Data Source=WayTooGonzo;Initial Catalog=Demos;Integrated Security=True");
            cmd = new SqlCommand("INSERT INTO tblDemo VALUES ('Phillip Do', 'Handsome guy', 900)", cnn);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private void DBOperation2()
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
    }
}
