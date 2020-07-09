using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BlockChainLib;

namespace BlockChainEXE
{
    [ServiceContract]
    public interface IValidator
    {
        [OperationContract]
        void AddTransaction(Transaction tr);

        [OperationContract]
        void FinalizeBlock();

        [OperationContract]
        Boolean ValidateBlock(Block block);

        [OperationContract]
        void AdvertiseBlock(Block block);

    }
}
