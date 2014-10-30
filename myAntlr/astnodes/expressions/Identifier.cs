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

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.expressions
{
    public class Identifier : Expression {

	    public virtual ParserRuleContext getParseTreeNodeContext()
	    {
		    return parseTreeNodeContext;
	    }

	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }	
    }
}
