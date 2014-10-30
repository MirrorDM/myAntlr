using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Atn;

using myAntlr.astnodes.expressions;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes
{
    public class DummyNameNode : Identifier
    {
        // public DummyNameNode(){ super(); }
	    public DummyNameNode() : base()
        { }
	
	    public override String getEscapedCodeStr()
	    {
		    return "<unnamed>";
	    }
    }
}
