using System;
using System.Runtime.Serialization;


namespace BlockChainLib
{
    [DataContract]
    public class Transaction : BaseClass
    {
        private Node _source = null;
        private string _data; // usually a JSON description of the transaction
        private DateTime _transactionDate;
        private DateTime _locktime; // allows transactions to be post-dated, i.e. sent to the blockchain with a delayed execution

        [DataMember]
        public string Data { get => _data; set => _data = value; }
        
        [DataMember]
        public DateTime TransactionDate { get => _transactionDate; set => _transactionDate = value; }

        [DataMember] 
        internal Node Source { get => _source; set => _source = value; }
        
        [DataMember]
        public DateTime Locktime { get => _locktime; set => _locktime = value; }

        public Transaction()
        {
            //Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Transaction created.");
        }

        public Transaction(Node src, string data)
        {
            this.Source = src;
            this.Data = data;
            this.TransactionDate = DateTime.Now;

            //Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Transaction created.");
        }

        public Transaction(Node src, string data, DateTime dt)
        {
            this.Source = src;
            this.Data = data;
            this.TransactionDate = dt;

            //Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Transaction created.");
        }
    }
}
