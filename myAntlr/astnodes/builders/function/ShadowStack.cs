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
    public class ShadowStack
    {

        private Stack<StackItem> stack = new Stack<StackItem>();
        private Stack<ASTNode> itemStack;

        private class StackItem
        {

            public ASTNode parentCompound;
            public ASTNode ifOrDo;

            public StackItem(ASTNode item, ASTNode parent)
            {
                ifOrDo = item;
                parentCompound = parent;
            }

        }

        public ShadowStack(Stack<ASTNode> aItemStack)
        {
            itemStack = aItemStack;
        }

        public void push(ASTNode statementItem)
	    {
            // if(statementItem instanceof IfStatement || statementItem instanceof DoStatement){
		    if(statementItem is IfStatement || statementItem is DoStatement){
			    ASTNode parentCompound = parentCompoundFromItemStack(itemStack);
			
			    // stack.push(new StackItem(statementItem, parentCompound));
                stack.Push(new StackItem(statementItem, parentCompound));
		    }
	    }

        public void pop()
        {
            // ASTNode topOfItemStack = itemStack.peek();
            ASTNode topOfItemStack = itemStack.Peek();

            // while (stack.size() > 0 && stack.peek().parentCompound == topOfItemStack)
            while (stack.Count() > 0 && stack.Peek().parentCompound == topOfItemStack)
            {
                // stack.pop();
                stack.Pop();
            }
        }

        public IfStatement getIfInElseCase()
        {
            // if (stack.size() < 2)
            if (stack.Count() < 2)
                return null;

            // StackItem topItem = stack.pop();
            StackItem topItem = stack.Pop();
            // StackItem returnItem = stack.pop();
            StackItem returnItem = stack.Pop();
            // stack.push(topItem);
            stack.Push(topItem);
            return (IfStatement)returnItem.ifOrDo;
        }

        public IfStatement getIf()
        {
            IfStatement retval;
            StackItem item = null;

            try
            {
                // item = stack.pop();
                item = stack.Pop();
                retval = (IfStatement)item.ifOrDo;
            }
            // catch (EmptyStackException ex)
            catch (InvalidOperationException ex)
            {
                return null;
            }
            // catch (ClassCastException ex)
            catch (InvalidCastException ex)
            {
                // stack.push(item);
                stack.Push(item);
                return null;
            }

            return retval;
        }

        public DoStatement getDo()
        {
            DoStatement retval;
            StackItem item = null;

            try
            {
                //item = stack.pop();
                item = stack.Pop();
                retval = (DoStatement)item.ifOrDo;

                // if (itemStack.contains(retval))
                if (itemStack.Contains(retval))
                {
                    // stack.push(item);
                    stack.Push(item);
                    return null;
                }

            }
            // catch (EmptyStackException ex)
            catch (InvalidOperationException ex)
            {
                return null;
            }
            // catch (ClassCastException ex)
            catch (InvalidCastException ex)
            {
                // stack.push(item);
                stack.Push(item);
                return null;
            }

            return retval;
        }


        private ASTNode parentCompoundFromItemStack(Stack<ASTNode> itemStack)
	    {
		    // Watchout: we are assuming that this function is never
		    // called when 0 compound statements are on the stack.
		    // If this ever happens, null is returned.
		
		    ASTNode parentCompound = null;
		    // walk stack from top to bottom

            // for(int i = itemStack.size() -1; i >= 0; i--){
		    for(int i = itemStack.Count() -1; i >= 0; i--){
                // if(itemStack.get(i) instanceof CompoundStatement){
			    if(itemStack.ElementAt(i) is CompoundStatement){
				    // parentCompound = itemStack.get(i);
                    parentCompound = itemStack.ElementAt(i);
				    break;
			    }
		    }
		    return parentCompound;
	    }

    }

}
