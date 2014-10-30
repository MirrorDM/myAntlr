using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.statements
{
    public class CompoundStatement : Statement
    {
	    // protected static final List<ASTNode> emptyList = new LinkedList<ASTNode>();
	    protected static readonly List<ASTNode> emptyList = new LinkedList<ASTNode>().ToList<ASTNode>();

	    public virtual void addStatement(ASTNode stmt)
	    {
            //super.addChild(stmt);
            base.addChild(stmt);
	    }
	
	    public virtual List<ASTNode> getStatements()
	    {
		    if(children == null)
			    return emptyList;
		    return children.ToList<ASTNode>();
	    }

	    public override String getEscapedCodeStr() { return ""; }
	
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
    }

}
