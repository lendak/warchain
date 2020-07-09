using BlockChainLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChainLib;

namespace BlockChainEXE
{
    public class ValidatorImpl : IValidator
    {
        private BlockChainLib.ProofOfStake.Validator _val = new BlockChainLib.ProofOfStake.Validator();

        public void AddTransaction(Transaction tr)
        {
            _val.AddTransaction(tr);
        }

        public void AdvertiseBlock(Block block)
        {
            _val.AdvertiseBlock(block);
        }

        public void FinalizeBlock()
        {
            _val.FinalizeBlock();

        }

        public Boolean ValidateBlock(Block block)
        {
            return _val.ValidateBlock(block);
        }
    }
}
