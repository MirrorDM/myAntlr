using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.cfg;
using myAntlr.astnodes.statements;
using myAntlr.astnodes;
using myAntlr.tests.parseTreeToAST;

namespace myAntlr.tests.cfgCreation
{
    public class CFGCreatorTest
    {
	    ASTToCFGConverter converter;
	
	    // @Before
	    public void init()
	    {
		    converter = new ASTToCFGConverter();
	    }
	
	    public CFG getCFGForCode(String input)
	    {
		    CompoundStatement contentItem = (CompoundStatement) FunctionContentTestUtil.parseAndWalk(input);
		    return converter.convertCompoundStatement(contentItem);
	    }
	
	    protected ASTNode getConditionNode(CFG cfg)
	    {
		    //Vector<CFGNode> statements = cfg.getStatements();
            List<CFGNode> statements = cfg.getStatements();
            // CFGNode conditionBlock = statements.get(0);
		    CFGNode conditionBlock = statements.ElementAt(0);
		    ASTNode astNode = conditionBlock.getASTNode();
		    return astNode;
	    }
	
    }
}
