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
// No Class extends from this class.

namespace myAntlr.astnodes
{
    public class CodeLocation
    {
        readonly static private int NOT_SET = -1;
    
        int startLine = NOT_SET;
        int startPos = NOT_SET;
        int startIndex = NOT_SET;
        int stopIndex = NOT_SET;
    
        public CodeLocation(){}
    
        public CodeLocation(ParserRuleContext ctx)
        {
            initializeFromContext(ctx);
        }

        private void initializeFromContext(ParserRuleContext ctx)
        {
            startLine = ctx.start.Line;
            startPos = NOT_SET; // Don't know.
            // startPos = ctx.start.getCharPositionInLine();
            startIndex = ctx.start.StartIndex;
            if(ctx.stop != null)
                stopIndex = ctx.stop.StopIndex;
            else
                stopIndex = NOT_SET;
        }
    
        //@Override
        public override String ToString()
        {
            return String.Format( "%d:%d:%d:%d", startLine, startPos, startIndex, stopIndex); 
        }
    }
}
