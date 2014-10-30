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

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.functionDef
{
    public class ParameterType : ASTNode
    {
	    String completeType = "";
	    String baseType = "";

	    public override String getEscapedCodeStr()
	    {
		    if(codeStr != null)
			    return codeStr;
		    codeStr = completeType;
		    return codeStr;
	    }
	
	    public virtual void setCompleteType(String aCompleteType)
	    {
		    completeType = aCompleteType;
	    }

        public virtual void setBaseType(String aBaseType)
	    {
		    baseType = aBaseType;
	    }
	
	    public override void initializeFromContext(ParserRuleContext aCtx)
	    {	
		    // use entire parameter as location. It's the best
		    // we can do right now.

		    // super.initializeFromContext(aCtx);
            base.initializeFromContext(aCtx);
	    }	

    }

}
