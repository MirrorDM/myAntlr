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

using myAntlr.astnodes;
using myAntlr.astnodes.statements;
using myAntlr.astnodes.expressions;
using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.declarations
{
    public class ClassDefStatement : Statement
    {
	
	    public Identifier name = new DummyNameNode();
	    public CompoundStatement content = new CompoundStatement();
	
	    public override void addChild(ASTNode expression)
	    { 
		    // if(expression instanceof Identifier)
            if(expression is Identifier) //Don't know
			    name = (Identifier) expression;
		    else
			    // super.addChild(expression);
                base.addChild(expression);
	    }
	
	    public virtual Identifier getName()
	    {
		    return name;
	    }

	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
	
    }

}
