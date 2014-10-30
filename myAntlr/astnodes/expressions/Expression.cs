using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.expressions
{
    public class Expression : ASTNode
    {
	    public virtual void replaceFirstChild(ASTNode node)
	    {
            //children.removeFirst();
            children.RemoveFirst();
            //children.addFirst(node);
            children.AddFirst(node);
	    }

    }

}
