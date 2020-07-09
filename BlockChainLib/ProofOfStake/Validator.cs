using BlockChainLib.WARChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


namespace BlockChainLib
{
    namespace ProofOfStake
    {
        public class Validator : BlockChainLib.Node
        {
            private uint _stake = 0;
            static uint TRANSACTIONS_IN_BLOCK = 10;     // number of allowed transactions in a block - TODO: configure this to be 500 as in Bitcoin
            static uint DEFAULT_STAKE = 10;
            
            private Ledger _ledger = null;
            private Block _currentBlock = null;         // this block is populated with transactions and then appended to the ledger upon reaching consensus

            private Dictionary<BigInteger, BigInteger> _urlToDoc = new Dictionary<BigInteger, BigInteger>();
            private Dictionary<BigInteger, BigInteger> _docToUrl = new Dictionary<BigInteger, BigInteger>();
            private uint _modifiedDocs = 0;
            private uint _duplicateDocs = 0;

            public uint Stake { get => _stake; set => _stake = value; }
            public uint ModifiedDocs { get => _modifiedDocs; set => _modifiedDocs = value; }
            public uint DuplicateDocs { get => _duplicateDocs; set => _duplicateDocs = value; }

            public Validator()
            {
                Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Created.");
            }

            protected void CheckLedger()
            {
                // TODO: save & load ledger to/from storage
                if (_ledger is null)
                {
                    _ledger = new Ledger(this, new List<Block>());
                }

                if (_currentBlock is null)
                {
                    _currentBlock = new Block(System.Guid.NewGuid(), this, DateTime.Now, 0, "", 0, _ledger.GetLastBlock());
                }
            }

            public Block GetLastBlock()
            {
                CheckLedger();
                return _ledger.GetLastBlock();
            }

            public Boolean ValidateBlock(Block bl)
            {
                CheckLedger();

                Console.WriteLine("Guid: " + this.Identifier.ToString() + ". Block validated with Guid: " + bl.Identifier.ToString());

                return true; // TODO: implement more elaborate logic based on stakes
            }

            /// <summary>
            /// Tell the other nodes in the system that a new valid block was created and ask them to append to their ledger's
            /// </summary>
            /// <param name="bl">Valid block</param>
            public void AdvertiseBlock(Block bl)
            {
                CheckLedger();

                if (bl.Validator.Identifier != this.Identifier)
                {
                    this._ledger.AddValidBlock(bl);
                }
            }

            /// <summary>
            /// Add a transaction to the current block. Perform this only in this node and do not advertise prior to putting together an entire block and validating it.
            /// </summary>
            /// <param name="tr">Transaction to add</param>
            public void AddTransaction(Transaction tr)
            {
                CheckLedger();

                if (tr.GetType() == typeof(BlockChainLib.WARChain.WARCTransaction))
                {
                    WARCTransaction wtr = (WARCTransaction)tr;
                    if (this._urlToDoc.ContainsKey(wtr.UrlHash))
                    {
                        if (_urlToDoc[wtr.UrlHash] != wtr.DocHash)
                        {
                            this.ModifiedDocs++;
                        }
                    }

                    if (this._docToUrl.ContainsKey(wtr.DocHash))
                    {
                        if (_docToUrl[wtr.DocHash] != wtr.UrlHash)
                        {
                            this.DuplicateDocs++;
                        }
                    }

                    _urlToDoc[wtr.UrlHash] = wtr.DocHash;
                    _docToUrl[wtr.DocHash] = wtr.UrlHash; // might need a multi-map for this purpose
                }

                _currentBlock.AddTransaction(tr); // add the transaction

            }

            public void FinalizeBlock()
            {
                uint approved = 0;
                uint rejected = 0;
                foreach (KeyValuePair<System.Guid, Node> kvp in base._peers)
                {
                    if (kvp.Value.GetType() == typeof(BlockChainLib.ProofOfStake.Validator))
                    {
                        Validator vali = (Validator)kvp.Value;
                        if (vali.ValidateBlock(_currentBlock))
                            approved++;
                        else
                            rejected++;
                    }
                }

                if ((approved > rejected) || (this._peers.Count == 0)) // TODO: improve this simple majority vote-based consensus solution (e.g. by a vote of 6 as in Bitcoin)
                {
                    _ledger.AddValidBlock(_currentBlock); // TODO: add this validator's signature to the block before putting it into the ledger

                    foreach (KeyValuePair<System.Guid, Node> kvp in base._peers)
                    {
                        if (kvp.Value.GetType() == typeof(BlockChainLib.ProofOfStake.Validator))
                        {
                            ((Validator)kvp.Value).AdvertiseBlock(_currentBlock);
                        }
                    }

                    this._currentBlock = new Block(System.Guid.NewGuid(), this, DateTime.Now, 0, "", 0, _ledger.GetLastBlock());
                }
            }
        }
    }
}
