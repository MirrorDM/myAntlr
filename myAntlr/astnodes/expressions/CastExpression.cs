using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.expressions
{
    public class CastExpression : Expression
    {
        Expression castTarget = null;
	    Expression castExpression = null;
	
	    // @Override
	    public override void addChild(ASTNode expression)
	    {
		    if(castTarget == null){
			    castTarget = (Expression) expression;
		    }else{
			    castExpression = (Expression) expression;
		    }
	    }

	    // @Override
	    public override int getChildCount()
	    {
		    int childCount = 0;
		    if(castTarget != null) childCount++;
		    if(castExpression != null) childCount++;
		    return childCount;
	    }

	    // @Override
	    public override ASTNode getChild(int i)
	    {
		    if(i == 0) return castTarget;
		    return castExpression;
	    }

	    public virtual ASTNode getCastTarget()
	    {
		    return castTarget;
	    }
    }
}
