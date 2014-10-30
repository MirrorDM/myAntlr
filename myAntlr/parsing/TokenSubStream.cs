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

namespace myAntlr.parsing
{
    public class TokenSubStream : BufferedTokenStream
    {
	    protected int stopIndex = -1;
	    protected int startIndex = 0;
	
	    // protected Stack<Integer> stopIndexStack = new Stack<Integer>();
	    // protected Stack<Integer> startIndexStack = new Stack<Integer>();
	    protected Stack<int> stopIndexStack = new Stack<int>();
	    protected Stack<int> startIndexStack = new Stack<int>();

	
	    // public TokenSubStream(TokenSource tokenSource)
        public TokenSubStream(ITokenSource tokenSource) : base(tokenSource)
	    {
		    //super(tokenSource);
	    }
		
	    public void restrict(int aStartIndex, int aStopIndex)
	    {
		    // startIndexStack.push(index());
            startIndexStack.Push(Index);
		    // stopIndexStack.push(stopIndex);
            stopIndexStack.Push(stopIndex);

		    startIndex = aStartIndex;
		    stopIndex = aStopIndex;
		    // seek(aStartIndex);
            Seek(aStartIndex);
	    }
	
	    public void resetRestriction()
	    {
		    // stopIndex = stopIndexStack.pop();
            stopIndex = stopIndexStack.Pop();
            // startIndex = startIndexStack.pop();
		    startIndex = startIndexStack.Pop();
		    //seek(startIndex);
            Seek(startIndex);
	    }
	
	    // @Override
	    public override void Reset()
	    {
            // seek(startIndex);
		    Seek(startIndex);
	    }
	
	    // @Override
	    public override IToken Lt(int k)
	    {
		    // lazyInit();
            LazyInit();
            if ( k == 0 ) 
                return null;
            if (k < 0)
                // return LB(-k);
                return Lb(-k);
		
            int i = p + k - 1;
            // sync(i);
            Sync(i);

            // if ( i >= tokens.size() || (stopIndex != -1 && i >= stopIndex))
            if ( i >= tokens.Count() || (stopIndex != -1 && i >= stopIndex)) { // return EOF token
	            // EOF must be last token
	            // return (Token) tokens.get(tokens.size()-1);
                return (IToken)tokens.ElementAt(tokens.Count() - 1);
            }
            // return (Token) tokens.get(i);
            return (IToken) tokens.ElementAt(i);	
	    }
	
    }

}
