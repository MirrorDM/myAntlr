using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes;

// add by zdm. virtual/override handled

namespace myAntlr.cfg
{
    public class CFGNode
    {

        public ASTNode astNode;

        public virtual void setASTNode(ASTNode node)
        {
            astNode = node;
        }

        public virtual ASTNode getASTNode()
        {
            return astNode;
        }

        public virtual String getEscapedCodeStr()
        {
            if (astNode == null)
                return "";

            return astNode.getEscapedCodeStr();
        }

        public virtual void markAsCFGNode()
        {
            if (astNode == null) return;
            astNode.markAsCFGNode();
        }

    }

}
