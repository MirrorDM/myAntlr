using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.statements
{
    public class IfStatement : BlockStarter {
	
	    private ElseStatement elseNode = null;
	
	    public override int getChildCount()
	    {
		    int childCount = base.getChildCount();
		
		    if(getElseNode() != null) childCount++;
		    return childCount;
	    }
	
	    public override ASTNode getChild(int i)
	    {
		    if(i == 0)
			    return condition;
		    else if (i == 1)
			    return statement;
		    else if(i == 2)
			    return getElseNode();
            //throw new RuntimeException("Invalid IfItem");
            throw new SystemException("Invalid IfItem");
	    }

	    public virtual ElseStatement getElseNode() {
		    return elseNode;
	    }

	    public virtual void setElseNode(ElseStatement elseNode) {
		    this.elseNode = elseNode;
	    }

	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
    }

}
