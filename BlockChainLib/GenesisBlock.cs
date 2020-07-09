using System;
using System.Numerics;
using System.Collections.Generic;
using System.Text;

namespace BlockChainLib
{
    public class GenesisBlock : BlockChainLib.Block
    {
        public GenesisBlock(System.Guid id, BlockChainLib.Node validator, DateTime dt, long hash, string data, long sign) : base(id, validator, dt, hash, "", sign, null)
        {

        }
    }
}
