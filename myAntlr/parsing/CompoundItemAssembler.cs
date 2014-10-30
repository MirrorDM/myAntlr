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

using myAntlr.astwalking;
using myAntlr.astnodes;
using myAntlr.astnodes.statements;

// add by zdm. virtual/override handled

namespace myAntlr.parsing
{
    public class CompoundItemAssembler : ASTWalker 
    {

        private CompoundStatement compoundItem;
    
        public CompoundStatement getCompoundItem(){ return compoundItem; }
    
        // @Override
        public override void startOfUnit(ParserRuleContext ctx, String filename)
        {
            compoundItem = new CompoundStatement();
        }

        // @Override
        public override void endOfUnit(ParserRuleContext ctx, String filename) { }

        // @Override
        public override void processItem(ASTNode item, Stack<ASTNodeBuilder> itemStack)
        {
            compoundItem.addStatement(item);
        }

        //@Override 
        public override void begin() { }
        //@Override
        public override void end() { }

    }

}
