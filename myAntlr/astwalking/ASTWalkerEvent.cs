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

using myAntlr.astnodes;

// add by zdm. virtual/override handled

namespace myAntlr.astwalking
{
    public class ASTWalkerEvent
    {

        public enum eventID
        {
            BEGIN,
            START_OF_UNIT,
            END_OF_UNIT,
            PROCESS_ITEM,
            END
        };

        public ASTWalkerEvent(eventID aId)
        {
            id = aId;
        }

        public eventID id;
        public ParserRuleContext ctx;
        public String filename;
        public Stack<ASTNodeBuilder> itemStack;
        public ASTNode item;
    }

}
