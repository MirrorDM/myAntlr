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
using myAntlr.parsing;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.builders.function
{
    public class ParameterListBuilder : ASTNodeBuilder {

	    ParameterList thisItem;
	
	    // @Override
	    public override void createNew(ParserRuleContext ctx)
	    {
		    item = new ParameterList();
		    thisItem = (ParameterList) item;
		    thisItem.initializeFromContext(ctx);
	    }
	    
        // Add ModuleParser
	    public virtual void addParameter(ModuleParser.Parameter_declContext aCtx,
			    Stack<ASTNodeBuilder> itemStack)
	    {
            // Parameter_declContext ctx = (Parameter_declContext) aCtx;
            // Parameter_idContext parameter_id = ctx.parameter_id();
            ModuleParser.Parameter_declContext ctx = (ModuleParser.Parameter_declContext)aCtx;
            ModuleParser.Parameter_idContext parameter_id = ctx.parameter_id();
		
		    Parameter param = new Parameter();
		    param.initializeFromContext(ctx);
		
		    String baseType = ParseTreeUtils.childTokenString(ctx.param_decl_specifiers());
		    String completeType = determineCompleteType(parameter_id, baseType);
		
		    param.type.setBaseType(baseType);
		    param.type.setCompleteType(completeType);
		
		    thisItem.addParameter(param);
	    }

        // Add ModuleParser
        public virtual String determineCompleteType(ModuleParser.Parameter_idContext parameter_id,
									      String baseType)
	    {
		    String retType = baseType;
		
		    // TODO: use a string-builder here and clean this up.
		
		    // iterate until nesting level is reached
		    // where type is given.
		
		    while(parameter_id.parameter_name() == null){
			
			    String newCompleteType = "";
			
			    newCompleteType += "(";
			
			    if(parameter_id.ptrs() != null)
				    newCompleteType += ParseTreeUtils.childTokenString(parameter_id.ptrs()) + " ";
			    if(parameter_id.type_suffix() != null)
				    newCompleteType += ParseTreeUtils.childTokenString(parameter_id.type_suffix()) + " ";
			
			    newCompleteType += retType;
			    newCompleteType += ")";
			    retType = newCompleteType;
			    parameter_id = parameter_id.parameter_id();
		    }
		
		    if(parameter_id.ptrs() != null)
			    retType += " " + ParseTreeUtils.childTokenString(parameter_id.ptrs());
		    if(parameter_id.type_suffix() != null)
			    retType += " " + ParseTreeUtils.childTokenString(parameter_id.type_suffix());
		
		    return retType;
	    }
	
	
    }

}
