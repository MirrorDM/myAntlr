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

namespace myAntlr
{
    class DummyTreeNode : ParserRuleContext
    {
        public DummyTreeNode() : base() { }
        public DummyTreeNode(ParserRuleContext parent, int invokingStateNumber) : base(parent, invokingStateNumber) { }
    }
}
