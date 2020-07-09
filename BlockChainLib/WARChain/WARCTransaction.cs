using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace BlockChainLib
{
    namespace WARChain
    {
        public class WARCTransaction : BlockChainLib.Transaction
        {
            BigInteger _urlHash;
            BigInteger _docHash;

            public BigInteger UrlHash { get => _urlHash; set => _urlHash = value; }
            public BigInteger DocHash { get => _docHash; set => _docHash = value; }

            public WARCTransaction() : base()
            {

            }
            public WARCTransaction(Node src, string data) : base(src, data)
            {

            }

            public WARCTransaction(Node src, string data, DateTime dt) : base(src, data, dt)
            {

            }

            public WARCTransaction(Node src, string data, DateTime dt, BigInteger url, BigInteger document) : base(src, data, dt)
            {
                this.UrlHash = url;
                this.DocHash = document;
            }
        }
    }
}