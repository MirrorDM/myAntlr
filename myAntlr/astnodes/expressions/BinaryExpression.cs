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

namespace myAntlr.astnodes.expressions
{
    public class BinaryExpression : Expression
    {
        Expression[] subExpressions = new Expression[2];
	    private String _operator = "";
		
	
	    public virtual Expression getLeft() { return subExpressions[0]; }
        public virtual Expression getRight() { return subExpressions[1]; }
        public virtual void setLeft(Expression aLeft) { subExpressions[0] = aLeft; }
        public virtual void setRight(Expression aRight) { subExpressions[1] = aRight; }
	
	    //@Override
	    public override void addChild(ASTNode item)
	    {	
		    Expression expression = (Expression) item;
		    if(getLeft() == null)
			    setLeft(expression);
		    else if(getRight() == null)
			    setRight(expression);
		    else
                throw new SystemException("Error: attempting to add third child to binary expression");
			    //throw new RuntimeException("Error: attempting to add third child to binary expression");
	
		    base.addChild(item);
	    }
	
	    //@Override
	    public override int getChildCount()
	    {
		    int childCount = 0;
		    if(getLeft() != null) childCount++;
		    if(getRight() != null) childCount++;
		    return childCount;
	    }
	    // @Override
	    public override ASTNode getChild(int i)
	    {
		    return subExpressions[i];
	    }

	    // @Override
	    public override void initializeFromContext(ParserRuleContext ctx)
	    {
		    base.initializeFromContext(ctx);

            if (ctx.ChildCount == 3)
                setOperator(ctx.GetChild(1).GetText());
                //setOperator(ctx.getChild(1).getText()); 
	    }
        private void setOperator(String text)
	    {
		    _operator = text;
	    }

        public virtual String getOperator()
	    {
		    return _operator;
	    }
    }
}
