using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;
using myAntlr.astnodes.statements;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.expressions
{
    public class Argument : ExpressionHolder {
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
    }
}
