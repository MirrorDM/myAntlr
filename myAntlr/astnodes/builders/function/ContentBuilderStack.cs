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

using myAntlr.astnodes.statements;

// add by zdm. virtual/override handled
// No Class extends from this class.

namespace myAntlr.astnodes.builders.function
{
    public class ContentBuilderStack
    {
        //private Stack<ASTNode> itemStack = new Stack<ASTNode>();
        //private ShadowStack shadowStack = new ShadowStack(itemStack);
        private Stack<ASTNode> itemStack;
        private ShadowStack shadowStack;
        public ContentBuilderStack()
        {
            itemStack = new Stack<ASTNode>();
            shadowStack = new ShadowStack(itemStack);
        }

        public void push(ASTNode statementItem)
        {
            shadowStack.push(statementItem);
            //itemStack.push(statementItem);
            itemStack.Push(statementItem);
        }

        public ASTNode pop()
        {
            shadowStack.pop();
            //return itemStack.pop();
            return itemStack.Pop();
        }

        public int size()
        {
            //return itemStack.size();
            return itemStack.Count();
        }

        public ASTNode peek()
        {
            //return itemStack.peek();
            return itemStack.Peek();
        }

        public IfStatement getIfInElseCase()
        {
            return shadowStack.getIfInElseCase();
        }

        public IfStatement getIf()
        {
            return shadowStack.getIf();
        }

        public DoStatement getDo()
        {
            return shadowStack.getDo();
        }
    }
}
