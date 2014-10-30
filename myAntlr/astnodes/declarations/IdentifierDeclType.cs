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

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.declarations
{
    public class IdentifierDeclType : ASTNode
    {
	    public String baseType;
	    public String completeType;

	    public override String getEscapedCodeStr()
	    {
		    return completeType;
	    }
	
    }
}
