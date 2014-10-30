using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.expressions
{
    public class SizeofExpr : Expression {
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }	
    }

}
