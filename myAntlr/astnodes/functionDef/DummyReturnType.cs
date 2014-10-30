using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.functionDef
{
    public class DummyReturnType : ReturnType
    {
	    // public DummyReturnType()
        public DummyReturnType() : base()
        { 
            // super(); 
        }
	
	    public override String getEscapedCodeStr(){ return "<none>"; }
    }

}
