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

namespace myAntlr.astnodes
{
    abstract public class ASTNodeBuilder
    {
        protected ASTNode item;
        
        public virtual ASTNode getItem()
        {
            complete();
            return item;
        }

        public virtual void complete() { }

        abstract public void createNew(ParserRuleContext ctx);
    
    }
}
