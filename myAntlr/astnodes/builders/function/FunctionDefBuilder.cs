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

using myAntlr.astnodes.functionDef;
using myAntlr.astnodes.expressions;
using myAntlr.astnodes.statements;
using myAntlr.parsing;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.builders.function
{
    public class FunctionDefBuilder : ASTNodeBuilder {

	    FunctionDef thisItem;
	    ParameterListBuilder paramListBuilder = new ParameterListBuilder();
	
	    // @Override
	    public override void createNew(ParserRuleContext ctx)
	    {
		    item = new FunctionDef();
		    item.initializeFromContext(ctx);
		    thisItem = (FunctionDef) item;
	    }

        // Add ModuleParser
        public virtual void setName(ModuleParser.Function_nameContext ctx,
			    Stack<ASTNodeBuilder> itemStack)
	    {
		    thisItem.name = new Identifier();
		    thisItem.name.initializeFromContext(ctx);
	    }

        // Add ModuleParser
        public virtual void setReturnType(ModuleParser.Return_typeContext ctx,
			    Stack<ASTNodeBuilder> itemStack)
	    {
		    thisItem.returnType = new ReturnType();
		    ReturnType returnType = thisItem.returnType;
		
		    returnType.initializeFromContext(ctx);
		    returnType.setBaseType(ParseTreeUtils.childTokenString(ctx.type_name()));
		    returnType.setCompleteType(ParseTreeUtils.childTokenString(ctx));
	    }

        // Add ModuleParser
        public virtual void setParameterList(ModuleParser.Function_param_listContext ctx,
								     Stack<ASTNodeBuilder> itemStack)
	    {
		    paramListBuilder.createNew(ctx);
		    thisItem.setParameterList((ParameterList) paramListBuilder.getItem());
	    }

        // Add ModuleParser
        public virtual void addParameter(ModuleParser.Parameter_declContext ctx,
							     Stack<ASTNodeBuilder> itemStack)
	    {
		    paramListBuilder.addParameter(ctx, itemStack);
	    }

        public virtual void setContent(CompoundStatement functionContent)
	    {
		    thisItem.setContent(functionContent);
	    }
	
    }

}
