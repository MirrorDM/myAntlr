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

namespace myAntlr.astnodes.statements
{
    public class IdentifierDeclStatement : Statement
    {
	
	    ParserRuleContext typeNameContext;
	
	    public virtual List<ASTNode> getIdentifierDeclList()
	    {
            //return children;
            return children.ToList<ASTNode>();
	    }
	
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }

	    public virtual ParserRuleContext getTypeNameContext()
	    {
		    return typeNameContext;
	    }
	
	    public virtual void setTypeNameContext(ParserRuleContext ctx)
	    {
		    typeNameContext = ctx;
	    }

    }

}
