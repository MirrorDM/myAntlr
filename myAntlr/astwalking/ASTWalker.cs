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
using myAntlr.misc;

// add by zdm. virtual/override handled

namespace myAntlr.astwalking
{
    // public abstract class ASTWalker implements Observer
    public abstract class ASTWalker : MyObserver
    {

        //public void update(Observable obj, Object arg)
	    public override void update(MyObservable obj, Object arg)
	    {
		    ASTWalkerEvent myevent = (ASTWalkerEvent) arg;
		    switch(myevent.id){
                // Add ASTWalkerEvent.eventID.
                case ASTWalkerEvent.eventID.BEGIN: begin(); break;
                case ASTWalkerEvent.eventID.START_OF_UNIT: startOfUnit(myevent.ctx, myevent.filename); break;
                case ASTWalkerEvent.eventID.END_OF_UNIT: endOfUnit(myevent.ctx, myevent.filename); break;
                case ASTWalkerEvent.eventID.PROCESS_ITEM: processItem(myevent.item, myevent.itemStack); break;
                case ASTWalkerEvent.eventID.END: end(); break;
		    };
	    }
	
	    public abstract void startOfUnit(ParserRuleContext ctx, String filename);
	    public abstract void endOfUnit(ParserRuleContext ctx, String filename);
	    public abstract void processItem(ASTNode node, Stack<ASTNodeBuilder> nodeStack);
	    public abstract void begin();
	    public abstract void end();
    }

}
