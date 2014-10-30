using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes.expressions;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Atn;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.functionDef
{
    public class Parameter : ASTNode
    {
	    public ParameterType type = new ParameterType();
	    public Identifier name = new Identifier();
	
	    // @Override
	    public override void initializeFromContext(ParserRuleContext ctx)
	    {
            // Parameter_declContext paramCtx = (Parameter_declContext) ctx;
            ModuleParser.Parameter_declContext paramCtx = (ModuleParser.Parameter_declContext)ctx;
            // Parameter_nameContext paramName = getNameOfParameter(paramCtx);
            ModuleParser.Parameter_nameContext paramName = getNameOfParameter(paramCtx);
		
		    type.initializeFromContext(ctx);
		    name.initializeFromContext(paramName);
		    // super.initializeFromContext(ctx);
            base.initializeFromContext(ctx);
	
		    // super.addChild(type);
            base.addChild(type);
		    // super.addChild(name);
            base.addChild(name);
	    }

        private ModuleParser.Parameter_nameContext getNameOfParameter(ModuleParser.Parameter_declContext param_ctx)
	    {
            // Parameter_idContext parameter_id = param_ctx.parameter_id();
            ModuleParser.Parameter_idContext parameter_id = param_ctx.parameter_id();
		    
		    while(parameter_id.parameter_name() == null){
			    parameter_id = parameter_id.parameter_id();
		    }
		    return parameter_id.parameter_name();
	    }

	
    }

}
