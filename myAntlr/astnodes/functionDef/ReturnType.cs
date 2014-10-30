using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.functionDef
{
    public class ReturnType : ASTNode
    {
        String completeType;
        String baseType;
	
        public virtual void setCompleteType(String aCompleteType)
        {
            completeType = aCompleteType;
        }

        public virtual void setBaseType(String aBaseType)
        {
            baseType = aBaseType;
        }

    }

}
