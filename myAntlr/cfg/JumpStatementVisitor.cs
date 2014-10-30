using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes.statements;
using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.cfg
{
    public class JumpStatementVisitor : ASTNodeVisitor {

        CFG thisCFG;
        CFGNode thisStatement;


        public virtual void setCFG(CFG cfg) {
            thisCFG = cfg;
        }


        public virtual void setStatement(CFGNode statement)
        {
            thisStatement = statement;
        }


        public override void visit(ReturnStatement expression)
        {
            // Edges edges = thisCFG.getEdges();
            // edges.removeAllEdgesFrom(thisStatement);
            thisCFG.removeAllEdgesFrom(thisStatement);
            CFGNode exitBlock = thisCFG.getLastStatement();
            if (exitBlock == null)
                // throw new RuntimeException("error attaching return to exitBlock: no exitBlock");
                throw new SystemException("error attaching return to exitBlock: no exitBlock");
            // edges.addEdge(thisStatement, exitBlock);
            thisCFG.addEdge(thisStatement, exitBlock);
        }


        public override void visit(GotoStatement expression)
        {
            String target = expression.getTarget();
            CFGNode blockByLabel = thisCFG.getBlockByLabel(target);
            if (blockByLabel == null) {
                // throw new RuntimeException("cannot find label " + target);
                throw new SystemException("cannot find label " + target);
            }

            // thisCFG.getEdges().removeAllEdgesFrom(thisStatement);
            // thisCFG.getEdges().addEdge(thisStatement, blockByLabel);
            thisCFG.removeAllEdgesFrom(thisStatement);
            thisCFG.addEdge(thisStatement, blockByLabel);
        }


        public override void visit(ContinueStatement expression)
        {
            // thisCFG.getEdges().removeAllEdgesFrom(thisStatement);
            thisCFG.removeAllEdgesFrom(thisStatement);
            CFGNode outerLoop = thisCFG.getOuterLoop(thisStatement);
            thisCFG.addEdge(thisStatement, outerLoop);
        }


        public override void visit(BreakStatement expression)
        {
            // thisCFG.getEdges().removeAllEdgesFrom(thisStatement);
            thisCFG.removeAllEdgesFrom(thisStatement);
            CFGNode outerLoop = thisCFG.getOuterLoop(thisStatement);

            // List<Object> edgesFrom = thisCFG.edges.getEdgesFrom(outerLoop);
            // CFGNode endOfLoop = (CFGNode) edgesFrom.get(1);
            List<CFGEdge> edgesFrom = thisCFG.getAllEdgesFrom(outerLoop);
            // CFGNode endOfLoop = edgesFrom.get(1).getDestination();
            CFGNode endOfLoop = edgesFrom.ElementAt(1).getDestination();
            thisCFG.addEdge(thisStatement, endOfLoop);
        }
    }
}
