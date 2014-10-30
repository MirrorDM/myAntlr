using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astwalking;
using myAntlr.astnodes;
using myAntlr.astnodes.functionDef;
using myAntlr.astnodes.statements;

// add by zdm. virtual/override handled

namespace myAntlr.cfg
{
    public class StructuredFlowVisitor : ASTNodeVisitor {

        CFG returnCFG;
        Stack<CFGNode> loopStack = new Stack<CFGNode>();


        public virtual CFG getCFG()
        {
	        return returnCFG;
        }


        public override void visit(ParameterList paramList)
        {
	        CFG cfg = new CFG();
	        LinkedList<Parameter> parameters = paramList.getParameters();
	        CFGNode lastParamBlock = null;

            // for (Parameter parameter : parameters) {
	        foreach (Parameter parameter in parameters) {
	            addStatementForNode(parameter, cfg);
	            CFGNode thisBlock = cfg.getLastStatement();
	            if (lastParamBlock != null)
		            cfg.addEdge(lastParamBlock, thisBlock);
	            lastParamBlock = thisBlock;
	        }

	        returnCFG = cfg;
        }


        public override void visit(CompoundStatement content)
        {
	        CFG cfg = new CFG();
	        List<ASTNode> statements = content.getStatements();

	        appendChildCFGs(cfg, statements);

	        returnCFG = cfg;
        }


        public override void visit(ASTNode expression)
        {
	        returnCFG = defaultStatementConverter(expression);
        }


        public override void visit(ReturnStatement expression)
        {
	        returnCFG = defaultStatementConverter(expression);
	        returnCFG.addJumpStatement(returnCFG.getLastStatement());
        }


        public override void visit(GotoStatement expression)
        {
	        returnCFG = defaultStatementConverter(expression);
	        returnCFG.addJumpStatement(returnCFG.getLastStatement());
        }


        public virtual CFG defaultStatementConverter(ASTNode child)
        {
	        CFG cfg = new CFG();
	        addStatementForNode(child, cfg);
	        return cfg;
        }


        private void appendChildCFGs(CFG cfg, List<ASTNode> statements) {
	        // Iterator<ASTNode> it = statements.iterator();
            IEnumerator<ASTNode> it = statements.GetEnumerator();
            // while (it.hasNext()) 
            while (it.MoveNext()) 
            {
                // Statement child = (Statement) it.next();
	            Statement child = (Statement) it.Current;
	            CFG childCFG = convertStatement(child);
	            cfg.addEdge(cfg.getLastStatement(), childCFG.getFirstStatement());
	            cfg.addCFG(childCFG);
	        }
        }


        public virtual CFG convertStatement(Statement node)
        {
	        returnCFG = new CFG();
	        node.accept(this);

	        // if no statement is present, return
	        // a CFG containing an empty basic block.
	        if (returnCFG.getNumberOfStatements() == 0) {
	            returnCFG.addStatement(new CFGNode());
	        }

	        return returnCFG;
        }


        public override void visit(IfStatement node)
        {
	        CFG cfg = new CFG();

	        CFGNode conditionBlock = addConditionBlock(node, cfg, new CFGNode());
	        CFG statementCFG = addStatementBlock(node, cfg);
	        CFGNode ifExit;

	        cfg.addEdge(conditionBlock, statementCFG.getFirstStatement(), CFGEdge.TRUE_LABEL);

	        ElseStatement elseNode = node.getElseNode();
	        if (elseNode == null) {
	            ifExit = addEmptyCFGNode(cfg);
	            cfg.addEdge(conditionBlock, ifExit, CFGEdge.FALSE_LABEL);
	        } else {
	            Statement elseStatement = elseNode.getStatement();
	            CFG elseCFG = convertStatement(elseStatement);
	            cfg.addCFG(elseCFG);
	            cfg.addEdge(conditionBlock, elseCFG.getFirstStatement(), CFGEdge.FALSE_LABEL);
	            ifExit = addEmptyCFGNode(cfg);
	            cfg.addEdge(elseCFG.getLastStatement(), ifExit);
	        }

	        cfg.addEdge(statementCFG.getLastStatement(), ifExit);

	        returnCFG = cfg;
        }


        public override void visit(ForStatement node)
        {
	        CFG cfg = new CFG();

	        CFGNode initBlock = addEmptyCFGNode(cfg);
	        initBlock.setASTNode(node.getForInitStatement());

	        CFGNode conditionBlock = addConditionBlock(node, cfg, new LoopBlock());

            // loopStack.push(conditionBlock);
	        loopStack.Push(conditionBlock);
	        CFG statementCFG = addStatementBlock(node, cfg);
            // loopStack.pop();
	        loopStack.Pop();

	        CFGNode exprBlock = addEmptyCFGNode(cfg);
	        exprBlock.setASTNode(node.getExpression());

	        CFGNode loopExit = addEmptyCFGNode(cfg);

	        cfg.addEdge(initBlock, conditionBlock);
	        cfg.addEdge(conditionBlock, statementCFG.getFirstStatement(), CFGEdge.TRUE_LABEL);
	        cfg.addEdge(conditionBlock, loopExit, CFGEdge.FALSE_LABEL);
	        cfg.addEdge(statementCFG.getLastStatement(), exprBlock);
	        cfg.addEdge(exprBlock, conditionBlock);

	        returnCFG = cfg;
        }


        public override void visit(WhileStatement node)
        {
	        CFG cfg = new CFG();

	        CFGNode conditionBlock = addConditionBlock(node, cfg, new LoopBlock());

            // loopStack.Push(conditionBlock);
	        loopStack.Push(conditionBlock);
	        CFG statementCFG = addStatementBlock(node, cfg);
	        // loopStack.push(conditionBlock);
            // loopStack.pop();
	        loopStack.Pop();

	        CFGNode loopExit = addEmptyCFGNode(cfg);

	        cfg.addEdge(conditionBlock, statementCFG.getFirstStatement(), CFGEdge.TRUE_LABEL);
	        cfg.addEdge(conditionBlock, loopExit, CFGEdge.FALSE_LABEL);
	        cfg.addEdge(statementCFG.getLastStatement(), conditionBlock);

	        returnCFG = cfg;

        }


        public override void visit(DoStatement node)
        {
	        CFG cfg = new CFG();

	        CFGNode loopEntry = addEmptyCFGNode(cfg);

            // loopStack.push(loopEntry);
	        loopStack.Push(loopEntry);
	        CFG statementCFG = addStatementBlock(node, cfg);
	        // loopStack.pop();
            loopStack.Pop();

	        CFGNode conditionBlock = addConditionBlock(node, cfg, new LoopBlock());

	        CFGNode loopExit = addEmptyCFGNode(cfg);

	        cfg.addEdge(loopEntry, statementCFG.getFirstStatement());
	        cfg.addEdge(statementCFG.getLastStatement(), conditionBlock);
	        cfg.addEdge(conditionBlock, loopEntry, CFGEdge.TRUE_LABEL);
	        cfg.addEdge(conditionBlock, loopExit, CFGEdge.FALSE_LABEL);

	        returnCFG = cfg;
        }


        public override void visit(SwitchStatement node)
        {
	        CFG cfg = new CFG();

	        CFGNode conditionBlock = addConditionBlock(node, cfg, new SwitchBlock());

            // loopStack.push(conditionBlock);
	        loopStack.Push(conditionBlock);
	        CFG statementCFG = addStatementBlock(node, cfg);
            // loopStack.pop();
            loopStack.Pop();

	        CFGNode switchExit = addEmptyCFGNode(cfg);

	        cfg.addEdge(conditionBlock, statementCFG.getFirstStatement());
	        cfg.addEdge(statementCFG.getLastStatement(), switchExit);

	        // HACK: We're adding an edge from the condition
	        // to the the end of the switch-statement here
	        // so that in the JumpStatementVisitor, we can
	        // derive the end of the switch-statement from
	        // the start of the switch statement
	        // This edge is removed by the JumpStatemetVisitor

	        cfg.addEdge(conditionBlock, switchExit);

	        returnCFG = cfg;

        }


        public override void visit(Label node)
        {
	        CFG cfg = new CFG();
	        addStatementForNode(node, cfg);
	        String label = node.getEscapedCodeStr();
            // label = label.substring(0, label.length() - 2);
	        label = label.Substring(0, label.Length - 2);
	        cfg.labelBlock(label, cfg.getFirstStatement());

	        CFGNode surroundingSwitch = getSurroundingSwitch();
	        if (surroundingSwitch != null) {
	            cfg.addSwitchLabel(surroundingSwitch, cfg.getFirstStatement());
	        }

	        returnCFG = cfg;
        }


        public override void visit(ContinueStatement expression)
        {
	        returnCFG = defaultStatementConverter(expression);

            // add by zdm. fix break and continue.
            returnCFG.addJumpStatement(returnCFG.getLastStatement());
            // add by zdm. fix break and continue.

	        CFGNode surroundingLoop = getSurroundingLoop();

	        if (surroundingLoop == null) {
                // System.err.println("Warning: no surrounding loop found for continue-statement");
	            System.Console.Error.WriteLine("Warning: no surrounding loop found for continue-statement");
	            return;
	        }

            // returnCFG.loopStart.put(returnCFG.getFirstStatement(), surroundingLoop);
	        returnCFG.loopStart.Add(returnCFG.getFirstStatement(), surroundingLoop);

        }


        public override void visit(BreakStatement expression)
        {
	        returnCFG = defaultStatementConverter(expression);

            // add by zdm. fix break and continue.
            returnCFG.addJumpStatement(returnCFG.getLastStatement());
            // add by zdm. fix break and continue.

	        CFGNode surroundingBlock = getSurroundingBlock();

	        if (surroundingBlock == null) {
	            // System.err.println("Warning: no surrounding block found for break-statement");
                System.Console.Error.WriteLine("Warning: no surrounding block found for break-statement");
	            return;
	        }

	        // returnCFG.loopStart.put(returnCFG.getFirstStatement(), surroundingBlock);
            returnCFG.loopStart.Add(returnCFG.getFirstStatement(), surroundingBlock);
        }


        private CFGNode getSurroundingLoop() {
            // for (int i = loopStack.size() - 1; i >= 0; i--)
	        for (int i = loopStack.Count() - 1; i >= 0; i--) {
	            // CFGNode statement = loopStack.get(i);
                 CFGNode statement = loopStack.ElementAt(i);
                // if (!(statement instanceof SwitchBlock))
	            if (!(statement is SwitchBlock)) {
		            return statement;
	            }
	        }
	        return null;
        }


        private CFGNode getSurroundingSwitch() {
            // for (int i = loopStack.size() - 1; i >= 0; i--)
	        for (int i = loopStack.Count() - 1; i >= 0; i--) {
                // CFGNode statement = loopStack.get(i);
	            CFGNode statement = loopStack.ElementAt(i);
                // if ((statement instanceof SwitchBlock)) {
	            if ((statement is SwitchBlock)) {
		            return statement;
	            }
	        }
	        return null;
        }


        private CFGNode getSurroundingBlock() {
            // if (loopStack.size() == 0)
	        if (loopStack.Count() == 0)
	            return null;
            // return loopStack.peek();
	        return loopStack.Peek();
        }


        private CFGNode addEmptyCFGNode(CFG cfg) {
	        CFGNode emptyBlock = new CFGNode();
	        cfg.addStatement(emptyBlock);
	        return emptyBlock;
        }


        private CFGNode addConditionBlock(BlockStarter node, CFG cfg, CFGNode container) {
	        Condition condition = node.getCondition();

	        if (condition != null)
	            container.setASTNode(condition);

	        cfg.addStatement(container);
	        return container;
        }


        private CFG addStatementBlock(BlockStarter node, CFG cfg) {
	        Statement statement = node.getStatement();
	        CFG statementCFG = convertStatement(statement);
	        cfg.addCFG(statementCFG);
	        return statementCFG;
        }


        private void addStatementForNode(ASTNode child, CFG cfg) {
	        CFGNode statement = new CFGNode();
	        statement.setASTNode(child);
	        cfg.addStatement(statement);
        }

    }

}
