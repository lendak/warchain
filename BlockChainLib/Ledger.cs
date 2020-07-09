using BlockChainLib.ProofOfStake;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChainLib
{
    public class Ledger // aka blockchain
    {
        static private GenesisBlock _genesisBlock;

        static internal GenesisBlock GenesisBlock { get => _genesisBlock; set => _genesisBlock = value; }

        private List<Block> _blocks = new List<Block>();
        protected Validator _validator = null;

        public Ledger(Validator val, List<Block> inblocks) {
            this._validator = val;
            this._blocks = inblocks;
        }

        public void AddValidBlock(Block bl)
        {
            this._blocks.Add(bl);
        }

        public Block GetLastBlock()
        {
            if (_blocks.Count == 0)
            {
                _genesisBlock = new GenesisBlock(System.Guid.NewGuid(), this._validator, DateTime.Now, 0, "", 0);
                this._blocks.Add(_genesisBlock);
            }

            return _blocks[_blocks.Count - 1];
        }
    }
}
