using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes.expressions;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.statements
{
    public class ExpressionHolder : Expression
    {	
	    public override String getEscapedCodeStr()
	    {
		    if(codeStr != null)
			    return codeStr;
		
		    Expression expr = getExpression();
		    if(expr == null) return "";
		
		    codeStr = expr.getEscapedCodeStr();
		    return codeStr;
	    }
	
	    public virtual Expression getExpression()
	    {
		    if(children == null) return null;
		    // return (Expression) children.get(0);
            return (Expression)children.ElementAt(0);
	    }
	
    }

}
