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

using myAntlr.astnodes.statements;
using myAntlr.astnodes.expressions;
using myAntlr.astnodes.declarations;
using myAntlr.parsing;

//using myAntlr.FunctionParser;
//using myAntlr.FunctionParser.Or_expressionContext;
//using myAntlr.FunctionParser.GotoStatementContext;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.builders.function
{
    public class FunctionContentBuilder : ASTNodeBuilder
    {
        //ContentBuilderStack stack = new ContentBuilderStack();
        //NestingReconstructor nesting = new NestingReconstructor(stack);
        ContentBuilderStack stack;
	    NestingReconstructor nesting;
        public FunctionContentBuilder()
        {
            stack = new ContentBuilderStack();
	        nesting = new NestingReconstructor(stack);
        }
	
	    // exitStatements is called when the entire
	    // function-content has been walked

	    //public virtual void exitStatements(StatementsContext ctx)
        public virtual void exitStatements(FunctionParser.StatementsContext ctx)
	    {
		    if(stack.size() != 1)
			    // throw new RuntimeException("Broken stack while parsing");
                throw new SystemException("Broken stack while parsing");
		
	    }
	
	    // For all statements, begin by pushing a Statement Object
	    // onto the stack.

        //public virtual void enterStatement(StatementContext ctx)
        public virtual void enterStatement(FunctionParser.StatementContext ctx)
	    {
		    ASTNode statementItem = new Statement();
		    statementItem.initializeFromContext(ctx);
		    stack.push(statementItem);
	    }

	    // Mapping of grammar-rules to CodeItems.

        // public virtual void enterOpeningCurly(Opening_curlyContext ctx)
        public virtual void enterOpeningCurly(FunctionParser.Opening_curlyContext ctx)
	    {
		    replaceTopOfStack(new CompoundStatement());
	    }

        // public virtual void enterClosingCurly(Closing_curlyContext ctx)
        public virtual void enterClosingCurly(FunctionParser.Closing_curlyContext ctx)
	    {
		    replaceTopOfStack(new BlockCloser());
	    }

        // public virtual void enterBlockStarter(Block_starterContext ctx)
        public virtual void enterBlockStarter(FunctionParser.Block_starterContext ctx)
	    {
		    replaceTopOfStack(new BlockStarter());
	    }

        // public virtual void enterExprStatement(Expr_statementContext ctx)
        public virtual void enterExprStatement(FunctionParser.Expr_statementContext ctx)
	    {
		    replaceTopOfStack(new ExpressionStatement());
	    }

        // public virtual void enterIf(If_statementContext ctx)
        public virtual void enterIf(FunctionParser.If_statementContext ctx)
	    {
		    replaceTopOfStack(new IfStatement());
	    }

        // public virtual void enterFor(For_statementContext ctx)
        public virtual void enterFor(FunctionParser.For_statementContext ctx)
	    {
		    replaceTopOfStack(new ForStatement());
	    }

        // public virtual void enterWhile(While_statementContext ctx)
        public virtual void enterWhile(FunctionParser.While_statementContext ctx)
	    {
		    replaceTopOfStack(new WhileStatement());
	    }

        // public virtual void enterDo(Do_statementContext ctx)
        public virtual void enterDo(FunctionParser.Do_statementContext ctx)
	    {
		    replaceTopOfStack(new DoStatement());
	    }

        // public virtual void enterElse(Else_statementContext ctx)
        public virtual void enterElse(FunctionParser.Else_statementContext ctx)
	    {
		    replaceTopOfStack(new ElseStatement());
	    }

        // public virtual void exitStatement(StatementContext ctx)
        public virtual void exitStatement(FunctionParser.StatementContext ctx)
	    {
		    if(stack.size() == 0)
			    // throw new RuntimeException();
                throw new SystemException();

		    ASTNode itemToRemove = stack.peek();
		    itemToRemove.initializeFromContext(ctx);

            //if(itemToRemove instanceof BlockCloser){
            if(itemToRemove is BlockCloser){
			    closeCompoundStatement();
			    return;
		    }

		    // We keep Block-starters and compound items
		    // on the stack. They are removed by following
		    // statements.

            //if(itemToRemove instanceof BlockStarter ||
            //        itemToRemove instanceof CompoundStatement)
            if(itemToRemove is BlockStarter ||
				    itemToRemove is CompoundStatement)
			    return;

		    nesting.consolidate();	
	    }

        private void closeCompoundStatement()
	    {
		    stack.pop(); // remove 'CloseBlock'
		
		    CompoundStatement compoundItem = (CompoundStatement) stack.pop();
		    nesting.consolidateBlockStarters(compoundItem);		
	    }

	    // Expression handling

        //public virtual void enterExpression(ExprContext ctx)
        public virtual void enterExpression(FunctionParser.ExprContext ctx)
	    {
		    Expression expression = new Expression();
		    stack.push(expression);
	    }

        // public virtual void exitExpression(ExprContext ctx)
        public virtual void exitExpression(FunctionParser.ExprContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // public virtual void enterAssignment(Assign_exprContext ctx)
        public virtual void enterAssignment(FunctionParser.Assign_exprContext ctx)
	    {	
		    AssignmentExpr expr = new AssignmentExpr();
		    stack.push(expr);
	    }

        // public virtual void exitAssignment(Assign_exprContext ctx)
        public virtual void exitAssignment(FunctionParser.Assign_exprContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterConditionalExpr(FunctionParser.Conditional_expressionContext ctx)
	    {
		    ConditionalExpression expr = new ConditionalExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitConditionalExpr(FunctionParser.Conditional_expressionContext ctx)
	    {
		    introduceCndNodeForCndExpr();
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterOrExpression(FunctionParser.Or_expressionContext ctx)
	    {
		    OrExpression expr = new OrExpression();
		    stack.push(expr);
	    }
        
        // Add "FunctionParser."
        public virtual void exitrOrExpression(FunctionParser.Or_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterAndExpression(FunctionParser.And_expressionContext ctx)
	    {
		    AndExpression expr = new AndExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitAndExpression(FunctionParser.And_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterInclusiveOrExpression(FunctionParser.Inclusive_or_expressionContext ctx)
	    {
		    InclusiveOrExpression expr = new InclusiveOrExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitInclusiveOrExpression(FunctionParser.Inclusive_or_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterExclusiveOrExpression(FunctionParser.Exclusive_or_expressionContext ctx)
	    {
		    ExclusiveOrExpression expr = new ExclusiveOrExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitExclusiveOrExpression(FunctionParser.Exclusive_or_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterBitAndExpression(FunctionParser.Bit_and_expressionContext ctx)
	    {
		    BitAndExpression expr = new BitAndExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void enterEqualityExpression(FunctionParser.Equality_expressionContext ctx)
	    {
		    EqualityExpression expr = new EqualityExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitEqualityExpression(FunctionParser.Equality_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void exitBitAndExpression(FunctionParser.Bit_and_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterRelationalExpression(FunctionParser.Relational_expressionContext ctx)
	    {
		    RelationalExpression expr = new RelationalExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitRelationalExpression(FunctionParser.Relational_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterShiftExpression(FunctionParser.Shift_expressionContext ctx)
	    {
		    ShiftExpression expr = new ShiftExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitShiftExpression(FunctionParser.Shift_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterAdditiveExpression(FunctionParser.Additive_expressionContext ctx)
	    {
		    AdditiveExpression expr = new AdditiveExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitAdditiveExpression(FunctionParser.Additive_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterMultiplicativeExpression(FunctionParser.Multiplicative_expressionContext ctx)
	    {
		    MultiplicativeExpression expr = new MultiplicativeExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitMultiplicativeExpression(FunctionParser.Multiplicative_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterCastExpression(FunctionParser.Cast_expressionContext ctx)
	    {
		    CastExpression expr = new CastExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitCastExpression(FunctionParser.Cast_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterCast_target(FunctionParser.Cast_targetContext ctx)
	    {
		    CastTarget expr = new CastTarget();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitCast_target(FunctionParser.Cast_targetContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterFuncCall(FunctionParser.FuncCallContext ctx)
	    {
		    CallExpression expr = new CallExpression();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitFuncCall(FunctionParser.FuncCallContext ctx)
	    {
		    introduceCalleeNode();
		    nesting.consolidateSubExpression(ctx);
	    }

        // Add "FunctionParser."
        public virtual void enterSizeof(FunctionParser.SizeofContext ctx)
	    {
		    Sizeof expr = new Sizeof();
		    stack.push(expr);
	    }

        // Add "FunctionParser."
        public virtual void exitSizeof(FunctionParser.SizeofContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }
	
	    private void introduceCalleeNode()
	    {
		    CallExpression expr;
		    try{
			    expr = (CallExpression) stack.peek();
		    }
            // catch(EmptyStackException ex){
            catch (InvalidOperationException ex)
            {
			    return;
		    }

		    ASTNode child = expr.getChild(0);
		    if(child == null) return;

		    Callee callee = new Callee(); 
		    callee.addChild(child);
		    expr.replaceFirstChild(callee);
	    }

	    private void introduceCndNodeForCndExpr()
	    {
		    ConditionalExpression expr;
		    try{
			    expr = (ConditionalExpression) stack.peek();
		    }
            // catch(EmptyStackException ex){
            catch (InvalidOperationException ex)
            {
			    return;
		    }
		
		    ASTNode child = expr.getChild(0);
		    if(child == null) return;
		    Condition cnd = new Condition(); 
		    cnd.addChild(child);		
		    expr.replaceFirstChild(cnd);
		
	    }

        public virtual void enterArgumentList(FunctionParser.Function_argument_listContext ctx)
	    {
		    ArgumentList expr = new ArgumentList();
		    stack.push(expr);
	    }

        public virtual void exitArgumentList(FunctionParser.Function_argument_listContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterCondition(FunctionParser.ConditionContext ctx)
	    {
		    Condition expr = new Condition();
		    stack.push(expr);
	    }

        public virtual void exitCondition(FunctionParser.ConditionContext ctx)
	    {	
		    Condition cond = (Condition) stack.pop();
		    cond.initializeFromContext(ctx);
		    nesting.addItemToParent(cond);
	    }

        public virtual void enterDeclByClass(FunctionParser.DeclByClassContext ctx)
	    {
		    ClassDefBuilder classDefBuilder = new ClassDefBuilder();
		    classDefBuilder.createNew(ctx);
		    classDefBuilder.setName(ctx.class_def().class_name());
		    replaceTopOfStack(classDefBuilder.getItem());
	    }

	    public virtual void exitDeclByClass()
	    {
		    nesting.consolidate();
	    }

        public virtual void enterInitDeclSimple(FunctionParser.InitDeclSimpleContext ctx)
	    {				
		    ASTNode identifierDecl = buildDeclarator(ctx);
		    stack.push(identifierDecl);	
	    }

	    public virtual void exitInitDeclSimple()
	    {
		    IdentifierDecl identifierDecl = (IdentifierDecl) stack.pop();
		    ASTNode stmt =  stack.peek();
		    stmt.addChild(identifierDecl);
	    }

        public virtual void enterInitDeclWithAssign(FunctionParser.InitDeclWithAssignContext ctx)
	    {
		    IdentifierDecl identifierDecl = buildDeclarator(ctx);				
		    stack.push(identifierDecl);	
	    }

        public virtual void exitInitDeclWithAssign(FunctionParser.InitDeclWithAssignContext ctx)
	    {
		    IdentifierDecl identifierDecl = (IdentifierDecl) stack.pop();

		    Expression lastChild = (Expression) identifierDecl.popLastChild();
		    AssignmentExpr assign = new AssignmentExpr();
		    assign.initializeFromContext(ctx);

		    // watchout here, we're not making a copy.
		    // This is also a bit of a hack. As we go up,
		    // we introduce an artificial assignment-node.

		    assign.addChild(identifierDecl.getName());
		    assign.addChild(lastChild);

		    identifierDecl.addChild(assign);

		    ASTNode stmt =  stack.peek();
		    stmt.addChild(identifierDecl);
	    }

        public virtual void enterInitDeclWithCall(FunctionParser.InitDeclWithCallContext ctx)
	    {
		    ASTNode identifierDecl = buildDeclarator(ctx);
		    stack.push(identifierDecl);	
	    }

	    public virtual void exitInitDeclWithCall()
	    {
		    IdentifierDecl identifierDecl = (IdentifierDecl) stack.pop();
		    ASTNode stmt =  stack.peek();
		    stmt.addChild(identifierDecl);
	    }

	    private IdentifierDecl buildDeclarator(ParserRuleContext ctx)
	    {
		    InitDeclContextWrapper wrappedContext = new InitDeclContextWrapper(ctx);
		    ParserRuleContext typeName = getTypeFromParent();
		    IdentifierDeclBuilder declBuilder = new IdentifierDeclBuilder();
		    declBuilder.createNew(ctx);
		    declBuilder.setType(wrappedContext, typeName);
		    IdentifierDecl identifierDecl = (IdentifierDecl) declBuilder.getItem();
		    return identifierDecl;
	    }

	    private ParserRuleContext getTypeFromParent()
	    {
		    ASTNode parentItem =  stack.peek();
		    ParserRuleContext typeName;
            // if(parentItem instanceof IdentifierDeclStatement)
		    if(parentItem is IdentifierDeclStatement)
			    typeName = ((IdentifierDeclStatement) parentItem).getTypeNameContext();
            //else if (parentItem instanceof ClassDefStatement)
            else if (parentItem is ClassDefStatement)
			    typeName = ((ClassDefStatement) parentItem).getName().getParseTreeNodeContext();
		    else
                // throw new RuntimeException("No matching declaration statement/class definiton for init declarator");
			    throw new SystemException("No matching declaration statement/class definiton for init declarator");
		    return typeName;
	    }

        public virtual void enterIncDec(FunctionParser.Inc_decContext ctx)
	    {
		    IncDec expr = new IncDec();
		    stack.push(expr);
	    }

        public virtual void exitIncDec(FunctionParser.Inc_decContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterArrayIndexing(FunctionParser.ArrayIndexingContext ctx)
	    {
		    ArrayIndexing expr = new ArrayIndexing();
		    stack.push(expr);
	    }

        public virtual void exitArrayIndexing(FunctionParser.ArrayIndexingContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterMemberAccess(FunctionParser.MemberAccessContext ctx)
	    {
		    MemberAccess expr = new MemberAccess();
		    stack.push(expr);
	    }

        public virtual void exitMemberAccess(FunctionParser.MemberAccessContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterIncDecOp(FunctionParser.IncDecOpContext ctx)
	    {
		    IncDecOp expr = new IncDecOp();
		    stack.push(expr);
	    }

        public virtual void exitIncDecOp(FunctionParser.IncDecOpContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterPrimary(FunctionParser.Primary_expressionContext ctx)
	    {
		    PrimaryExpression expr = new PrimaryExpression();
		    stack.push(expr);
	    }

        public virtual void exitPrimary(FunctionParser.Primary_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterUnaryExpression(FunctionParser.Unary_expressionContext ctx)
	    {
		    UnaryExpression expr = new UnaryExpression();
		    stack.push(expr);
	    }

        public virtual void exitUnaryExpression(FunctionParser.Unary_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterIdentifier(FunctionParser.IdentifierContext ctx)
	    {
		    Identifier expr = new Identifier();
		    stack.push(expr);
	    }

        public virtual void exitIdentifier(FunctionParser.IdentifierContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterArgument(FunctionParser.Function_argumentContext ctx)
	    {
		    Argument expr = new Argument();
		    stack.push(expr);
	    }

        public virtual void exitArgument(FunctionParser.Function_argumentContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterInitializerList(FunctionParser.Initializer_listContext ctx)
	    {
		    InitializerList expr = new InitializerList();
		    stack.push(expr);
	    }

        public virtual void exitInitializerList(FunctionParser.Initializer_listContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterPtrMemberAccess(FunctionParser.PtrMemberAccessContext ctx)
	    {
		    PtrMemberAccess expr = new PtrMemberAccess();
		    stack.push(expr);
	    }

        public virtual void exitPtrMemberAccess(FunctionParser.PtrMemberAccessContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterInitFor(FunctionParser.For_init_statementContext ctx)
	    {
		    ForInit expr = new ForInit();
		    stack.push(expr);
	    }

        public virtual void exitInitFor(FunctionParser.For_init_statementContext ctx)
	    {
		    ASTNode node = stack.pop();
		    node.initializeFromContext(ctx);
		    ForStatement forStatement = (ForStatement) stack.peek();
		    forStatement.addChild(node);
	    }

        public virtual void enterSwitchStatement(FunctionParser.Switch_statementContext ctx)
	    {
		    replaceTopOfStack(new SwitchStatement());
	    }

        public virtual void enterLabel(FunctionParser.LabelContext ctx)
	    {
		    replaceTopOfStack(new Label());
	    }

        public virtual void enterReturnStatement(FunctionParser.ReturnStatementContext ctx)
	    {
		    replaceTopOfStack(new ReturnStatement());
	    }

        public virtual void enterBreakStatement(FunctionParser.BreakStatementContext ctx)
	    {
		    replaceTopOfStack(new BreakStatement());
	    }

        public virtual void enterContinueStatement(FunctionParser.ContinueStatementContext ctx)
	    {
		    replaceTopOfStack(new ContinueStatement());
	    }

        public virtual void enterGotoStatement(FunctionParser.GotoStatementContext ctx)
	    {
		    replaceTopOfStack(new GotoStatement());
	    }

	    // @Override
	    public override void createNew(ParserRuleContext ctx)
	    {
		    item = new CompoundStatement();
		    CompoundStatement rootItem = (CompoundStatement) item;
		    item.initializeFromContext(ctx);
		    stack.push(rootItem);
	    }

	    public virtual void addLocalDecl(IdentifierDecl decl)
	    {
		    IdentifierDeclStatement declStmt = (IdentifierDeclStatement) stack.peek();
		    declStmt.addChild(decl);
	    }

        // Add FunctionParser.
	    public virtual void enterDeclByType(ParserRuleContext ctx, FunctionParser.Type_nameContext type_nameContext)
	    {
		    IdentifierDeclStatement declStmt = new IdentifierDeclStatement();
		    declStmt.initializeFromContext(ctx);
		    declStmt.setTypeNameContext(type_nameContext);

            // if(stack.peek() instanceof Statement)
		    if(stack.peek() is Statement)
			    replaceTopOfStack(declStmt);
		    else
			    stack.push(declStmt);
	    }

	    public virtual void exitDeclByType()
	    {
		    nesting.consolidate();
	    }

	    protected void replaceTopOfStack(ASTNode item)
	    {
		    stack.pop();
		    stack.push(item);
	    }

        public virtual void enterSizeofExpr(FunctionParser.Sizeof_expressionContext ctx)
	    {
		    SizeofExpr expr = new SizeofExpr();
		    stack.push(expr);
	    }

        public virtual void exitSizeofExpr(FunctionParser.Sizeof_expressionContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterSizeofOperand2(FunctionParser.Sizeof_operand2Context ctx)
	    {
		    SizeofOperand expr = new SizeofOperand();
		    stack.push(expr);
	    }

        public virtual void enterSizeofOperand(FunctionParser.Sizeof_operandContext ctx)
	    {
		    SizeofOperand expr = new SizeofOperand();
		    stack.push(expr);
	    }

        public virtual void exitSizeofOperand2(FunctionParser.Sizeof_operand2Context ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void exitSizeofOperand(FunctionParser.Sizeof_operandContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterUnaryOpAndCastExpr(FunctionParser.Unary_op_and_cast_exprContext ctx)
	    {
		    UnaryOp expr = new UnaryOp();
		    stack.push(expr);
	    }

        public virtual void exitUnaryOpAndCastExpr(FunctionParser.Unary_op_and_cast_exprContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }

        public virtual void enterUnaryOperator(FunctionParser.Unary_operatorContext ctx)
	    {
		    UnaryOperator expr = new UnaryOperator();
		    stack.push(expr);
	    }

        public virtual void exitUnaryOperator(FunctionParser.Unary_operatorContext ctx)
	    {
		    nesting.consolidateSubExpression(ctx);
	    }
	
    }

}
