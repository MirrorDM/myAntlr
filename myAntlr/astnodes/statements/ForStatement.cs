using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.statements
{
    public class ForStatement : BlockStarter
    {
	    private ASTNode forInitStatement = null;
	    private ASTNode expression = null;
	
	    public virtual ASTNode getForInitStatement()
	    {
		    return forInitStatement;
	    }

	    public virtual void setForInitStatement(ASTNode forInitStatement)
	    {
		    this.forInitStatement = forInitStatement;
	    }

	    public virtual ASTNode getExpression()
	    {
		    return expression;
	    }

	    public virtual void setExpression(ASTNode expression)
	    {
		    this.expression = expression;
	    }
	
	    // @Override
	    public override void addChild(ASTNode item)
	    {	
		    if(forInitStatement == null)
			    forInitStatement = item;
		    else if(expression == null && condition != null){
			    expression = item;
		    }
		
		    base.addChild(item);
	    }

	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
    }

}
