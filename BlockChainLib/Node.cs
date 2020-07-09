using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BlockChainLib
{
    [DataContract]
    public class Node : BaseClass
    {
        private DateTime CreationDateValue;
        private string _address;
        private string _privateKey;
        private string _publicKey;
        protected Dictionary<System.Guid, BlockChainLib.Node> _peers = new Dictionary<System.Guid, BlockChainLib.Node>();

        [DataMember] 
        public DateTime CreationDate { get => CreationDateValue; set => CreationDateValue = value; }
        [DataMember] 
        public string Address { get => _address; set => _address = value; }
        [DataMember] 
        public string PublicKey { get => _publicKey; }

        public Node()
        {

        }

        /// <summary>
        /// Add peer nodes with which this blockchain node communicates
        /// Note: the underlying dictionary handles any duplicate add
        /// </summary>
        /// <param name="val">Neighbor node to add</param>
        public void AddPeer(BlockChainLib.Node val)
        {
            Console.WriteLine("Guid " + this.Identifier.ToString() + ". Peer added with Guid " + val.Identifier.ToString());
            if (val.Identifier != this.Identifier) // do not add this
                this._peers.Add(val.Identifier, val);
        }

        /// <summary>
        /// Add list of nodes as peers
        /// </summary>
        /// <param name="vals">Peers to add - can contain 'this'</param>
        public void AddPeers(System.Collections.Generic.List<BlockChainLib.ProofOfStake.Validator> vals)
        {
            // TODO: implement a more fine-grained peering solution - either hub nodes with higher probability of a random selection of (up to 10) nodes
            foreach (BlockChainLib.ProofOfStake.Validator val in vals)
            {
                this.AddPeer(val); // will implicity skip itself
            }
        }
    }
}
