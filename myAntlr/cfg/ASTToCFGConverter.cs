using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes;
using myAntlr.astnodes.functionDef;
using myAntlr.astnodes.statements;

namespace myAntlr.cfg
{
    public class ASTToCFGConverter
    {

        private StructuredFlowVisitor structuredFlowVisitor = new StructuredFlowVisitor();
        private JumpStatementVisitor jumpStatementVisitor = new JumpStatementVisitor();

        public CFG convert(FunctionDef node)
        {
            CFG cfg = convertFunctionDef(node);
            markCFGNodes(cfg);
            return cfg;
        }

        private void markCFGNodes(CFG cfg)
        {
            // Vector<CFGNode> statements = cfg.getStatements();
            List<CFGNode> statements = cfg.getStatements();
            // for(CFGNode statement: statements)
            foreach (CFGNode statement in statements)
                statement.markAsCFGNode();
        }

        private CFG convertFunctionDef(FunctionDef node)
        {
            // create a CFG for the parameter list

            ParameterList parameterList = node.getParameterList();
            CFG cfg = convertParameterList(parameterList);
            CFGNode lastParamDefBlock = cfg.getLastStatement();

            // create a CFG for the compound statement

            CompoundStatement content = node.getContent();
            CFG compoundCFG = convertCompoundStatement(content);
            CFGNode firstCompoundStmtBlock = compoundCFG.getFirstStatement();

            // add compound statement cfg to parameter list CFG

            cfg.addCFG(compoundCFG);

            // create an edge from the last parameter to the first
            // statement from the compound statement if necessary.

            if (lastParamDefBlock != null && firstCompoundStmtBlock != null)
            {
                cfg.addEdge(lastParamDefBlock, firstCompoundStmtBlock);
            }

            return cfg;
        }

        private CFG convertParameterList(ParameterList parameterList)
        {
            parameterList.accept(structuredFlowVisitor);
            CFG cfg = structuredFlowVisitor.getCFG();
            return cfg;
        }

        public CFG convertCompoundStatement(CompoundStatement content)
        {
            content.accept(structuredFlowVisitor);
            CFG cfg = structuredFlowVisitor.getCFG();
            honorJumpStatements(cfg);
            return cfg;
        }

        private void honorJumpStatements(CFG cfg)
        {
            fixJumps(cfg);
            fixSwitchBlocks(cfg);
        }

        private void fixSwitchBlocks(CFG cfg)
        {
            SwitchLabels switchLabels = cfg.getSwitchLabels();
            //Set<Entry<Object, List<Object>>> entrySet = switchLabels.entrySet();
            HashSet<KeyValuePair<Object, List<Object>>> entrySet = switchLabels.entrySet();
            // Iterator<Entry<Object, List<Object>>> it = entrySet.iterator();
            IEnumerator<KeyValuePair<Object, List<Object>>> it = entrySet.GetEnumerator();
        
            // while(it.hasNext())
            while(it.MoveNext()){
                // Entry<Object, List<Object>> entry = it.next();
                KeyValuePair<Object, List<Object>> entry = it.Current;
                // CFGNode switchBlock = (CFGNode) entry.getKey();
                CFGNode switchBlock = (CFGNode) entry.Key;
                // List<Object> labeledBlocks = entry.getValue();
                List<Object> labeledBlocks = entry.Value;
            
                // for(Object labeledBlock : labeledBlocks)
                foreach (Object labeledBlock in labeledBlocks)
                    cfg.addEdge(switchBlock, (CFGNode) labeledBlock);
            }
        }

        // zdm change this. Don't know.
        // There are two Class extend from CFGNode, SwitchBlock and LoopBlock, and no Class extends from these two Class.
        // And there is no override function for SwitchBlock and LoopBlock, So it seems do the same thing like CFGNode.
        private void fixJumps(CFG cfg)
        {
            // Collection<? extends CFGNode> jumpStatements = cfg.getJumpStatements();
            List<CFGNode> jumpStatements = cfg.getJumpStatements();
            // Iterator<? extends CFGNode> it = jumpStatements.iterator();
            IEnumerator<CFGNode> it = jumpStatements.GetEnumerator();
        
            // if(jumpStatements.size() > 0){
                // add an exit-block
            
                CFGNode emptyStatement = new CFGNode();
                if(cfg.getLastStatement() != null)
                    cfg.addEdge(cfg.getLastStatement(), emptyStatement);
                cfg.addStatement(emptyStatement);
            // }
        
            // while(it.hasNext())
            while(it.MoveNext()){
                // CFGNode stmt = it.next();
                CFGNode stmt = it.Current;
                ASTNode statement = stmt.getASTNode();
            
                jumpStatementVisitor.setCFG(cfg);
                jumpStatementVisitor.setStatement(stmt);
            
                try{
                    statement.accept(jumpStatementVisitor);
                }
                // catch(RuntimeException ex){
                catch(SystemException ex){
                    // System.err.println("While fixing jumps: " + ex.getMessage());
                    System.Console.Error.WriteLine("While fixing jumps: " + ex.Message);
                }
        
            }
        }

    }

}
