using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.statements
{
    public class GotoStatement : JumpStatement
    {
	    public virtual String getTarget()
	    {
		    return getChild(0).getEscapedCodeStr();
	    }
	
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
    }

}
