using System;
using System.Collections.Generic;
using System.Numerics;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text.Json;

namespace BlockChainLib
{
    [DataContract]
    public class Block : BaseClass
    {
        private DateTime _Timestamp;                // block creation time
        //private BigInteger _PreviousHash;         // hash value in the previous block
        private long _HashVal;                      // block hash value
        private BlockChainLib.Node _Validator;      // reference to the node validating this block
        private string _BlockData;                  // arbitrary data stored in the block - usually JSON
        private long _Signature;                     // signed and encrypted hash by the validator
        private BlockChainLib.Block _PreviousBlock; // reference to the preceding block - NULL for the genesis block
        private Dictionary<System.Guid, Transaction> _transactions = new Dictionary<System.Guid, Transaction>(); // transaction incorporated into the block - usually in JSON

        [DataMember]
        public DateTime Timestamp { get => _Timestamp; set => _Timestamp = value; }
        //public BigInteger PreviousHash { get => _PreviousHash; set => _PreviousHash = value; }
        
        [DataMember]
        public long HashValue { get => _HashVal; set => _HashVal = value; }
        
        [DataMember]
        public string BlockData { get => _BlockData; set => _BlockData = value; }

        [DataMember] 
        public long Signature { get => _Signature; set => _Signature = value; }

        [DataMember] 
        public Block PreviousBlock { get => _PreviousBlock; set => _PreviousBlock = value; }

        [DataMember] 
        internal Node Validator { get => _Validator; set => _Validator = value; }

        [DataMember] 
        public Dictionary<Guid, Transaction> Transactions { get => _transactions; }

        public Block() {
            //Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Block created.");
        }

        public Block(System.Guid id, BlockChainLib.Node validator, DateTime dt, long hash, string data, long sign, BlockChainLib.Block prevBl)
        {
            base.Identifier = id;
            this.Validator = validator;
            this.Timestamp = dt;
            this.HashValue = hash;
            this.BlockData = data;
            this.Signature = sign;
            this.PreviousBlock = prevBl;

            //Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Block created.");
        }

        public void AddTransaction(BlockChainLib.Transaction tr)
        {
            if (!this._transactions.ContainsKey(tr.Identifier)) // disallow overriding transactions with identical identifiers
            {
                this.Transactions.Add(tr.Identifier, tr);
            }
        }

        public string ToJson()
        {
            string json = JsonSerializer.Serialize(this);
            return json;
        }


    }
}
