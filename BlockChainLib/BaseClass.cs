using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChainLib
{
    public class BaseClass
    {
        protected System.Guid _identifier = System.Guid.Empty;
        public BaseClass()
        {
            this.Identifier = System.Guid.NewGuid(); // simple auto-increment ID - TODO: revise this code if we will persist the entire model
        }

        public BaseClass(System.Guid id)
        {
            this.Identifier = id;
        }

        public System.Guid Identifier { get => _identifier; set => _identifier = value; }
    }
}
