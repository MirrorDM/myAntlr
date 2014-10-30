using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.statements
{
    public class BlockStarter : Statement
    {
        //Statement statement = null;
        protected Statement statement = null;
        //Condition condition = null;
        protected Condition condition = null;
	
	    public virtual Statement getStatement()
	    {
		    return statement;
	    }

        public virtual Condition getCondition()
	    {
		    return condition;
	    }
	
	    private void setStatement(Statement aStatement)
	    {
		    statement = aStatement;
	    }

        private void setCondition(Condition expression)
	    {
		    condition = expression;
	    }

	    // @Override
	    public override void addChild(ASTNode node)
	    {
            //if(node instanceof Condition)
            if(node is Condition)
			    setCondition((Condition) node);
            //else if(node instanceof Statement)
            else if(node is Statement)
			    setStatement((Statement)node);
            //super.addChild(node);
            base.addChild(node);
	    }
	
    }
}
