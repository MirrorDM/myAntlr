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

using myAntlr.astnodes.expressions;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.declarations
{
    public class IdentifierDecl : ASTNode
    {
	    private IdentifierDeclType type;
	    private Identifier name;

	
	    public override void addChild(ASTNode node)
	    { 
            // if(node instanceof Identifier){
		    if(node is Identifier){
			    setName((Identifier) node);
			    return;
		    }
            // super to base
		    base.addChild(node);
	    }


        public virtual void setName(Identifier name)
	    {
		    this.name = name;
            // super to base
		    base.addChild(name);
	    }

        public virtual void setType(IdentifierDeclType type)
	    {
		    this.type = type;
            // super to base
		    base.addChild(type);
	    }

        public virtual Identifier getName()
	    {
		    return name;
	    }

        public virtual IdentifierDeclType getType()
	    {
		    return type;
	    }
	
    }
}
