using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes.functionDef;
using myAntlr.astnodes.statements;

// add by zdm. virtual/override handled

namespace myAntlr.astwalking
{
    public class FunctionNodeVisitor : ASTNodeVisitor
    {
	    ASTNodeVisitor functionNodeVisitor;
	
	    public virtual void setFunctionNodeVisitor(ASTNodeVisitor aNodeVisitor)
	    {
		    functionNodeVisitor = aNodeVisitor;
	    }
	
	    public virtual ASTNodeVisitor getFunctionNodeVisitor()
	    {
		    return functionNodeVisitor;
	    }
	
	    public override void visit(FunctionDef node)
	    {
		    node.accept(functionNodeVisitor);
	    }
	
	    public override void visit(IdentifierDeclStatement statementItem)
	    {
		    // don't expand identifier declarations
	    }
	
    }
}
