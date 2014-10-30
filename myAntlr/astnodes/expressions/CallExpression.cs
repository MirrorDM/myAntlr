using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.expressions
{
    public class CallExpression : PostfixExpression {

	    public virtual Expression getTarget()
	    {
		    if(children == null) return null;
		    // return (Expression) children.get(0);
            return (Expression)children.ElementAt(0);
	    }

	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }

    }

}
