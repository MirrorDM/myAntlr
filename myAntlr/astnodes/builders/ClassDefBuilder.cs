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

using myAntlr.astnodes.declarations;
using myAntlr.astnodes.expressions;
using myAntlr.astnodes.statements;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.builders
{
    public class ClassDefBuilder : ASTNodeBuilder
    {
        ClassDefStatement thisItem;
	
	    //@Override
	    public override void createNew(ParserRuleContext ctx)
	    {
		    item = new ClassDefStatement();
		    thisItem = (ClassDefStatement) item;
		    thisItem.initializeFromContext(ctx);
	    }

	    // TODO: merge the following two by introducing a wrapper
        //public void setName(Class_nameContext ctx)
        public virtual void setName(ModuleParser.Class_nameContext ctx)
	    {
		    thisItem.name = new Identifier();
		    thisItem.name.initializeFromContext(ctx);
	    }

	    public virtual void setName(
			    FunctionParser.Class_nameContext ctx)
	    {
		    thisItem.name = new Identifier();
		    thisItem.name.initializeFromContext(ctx);
	    }
	
	    public virtual void setContent(CompoundStatement content)
	    {
		    thisItem.content = content;
	    }

    }
}
