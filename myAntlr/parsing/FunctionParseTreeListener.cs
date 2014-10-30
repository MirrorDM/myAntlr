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

using myAntlr.astnodes.builders.function;

namespace myAntlr.parsing
{
    public class FunctionParseTreeListener : FunctionBaseListener
    {
        ANTLRParserDriver p;
    
        public FunctionParseTreeListener(ANTLRParserDriver aP)
        {
            p = aP;
        }
    
        //@override
        public override void EnterStatements(FunctionParser.StatementsContext ctx)
        {
            FunctionContentBuilder builder = new FunctionContentBuilder();
            builder.createNew(ctx);
            // p.builderStack.push(builder);
            p.builderStack.Push(builder);
        }
    
        //@override
        public override void ExitStatements(FunctionParser.StatementsContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitStatements(ctx);
        }
    
        //@override
        public override void EnterStatement(FunctionParser.StatementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterStatement(ctx);
        }
    
        //@override
        public override void ExitStatement(FunctionParser.StatementContext ctx)
        {    
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitStatement(ctx);
        }
    
        //@override
        public override void EnterElse_statement(FunctionParser.Else_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterElse(ctx);
        }
    
        //@override
        public override void EnterIf_statement(FunctionParser.If_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterIf(ctx);
        }
    
        //@override
        public override void EnterFor_statement(FunctionParser.For_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterFor(ctx);
        }
    
        //@override
        public override void EnterFor_init_statement(FunctionParser.For_init_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterInitFor(ctx);
        }
    
        //@override
        public override void ExitFor_init_statement(FunctionParser.For_init_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitInitFor(ctx);
        }
    
        //@override
        public override void EnterWhile_statement(FunctionParser.While_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterWhile(ctx);
        }
    
        //@override
        public override void EnterDo_statement(FunctionParser.Do_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterDo(ctx);
        }
    
        //@override
        public override void EnterSwitch_statement(FunctionParser.Switch_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterSwitchStatement(ctx);
        }
    
        //@override
        public override void EnterLabel(FunctionParser.LabelContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterLabel(ctx);
        }
    
        //@override
        public override void EnterBlock_starter(FunctionParser.Block_starterContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterBlockStarter(ctx);
        }
    
        //@override
        public override void EnterOpening_curly(FunctionParser.Opening_curlyContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterOpeningCurly(ctx);
        }
    
        //@override
        public override void EnterClosing_curly(FunctionParser.Closing_curlyContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterClosingCurly(ctx);
        }
    
        //@override
        public override void EnterExpr_statement(FunctionParser.Expr_statementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterExprStatement(ctx);
        }
    
        //@override
        public override void EnterReturnStatement(FunctionParser.ReturnStatementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterReturnStatement(ctx);
        }
    
        //@override
        public override void EnterBreakStatement(FunctionParser.BreakStatementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterBreakStatement(ctx);
        }
    
        //@override
        public override void EnterContinueStatement(FunctionParser.ContinueStatementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterContinueStatement(ctx);
        }
    
        //@override
        public override void EnterGotoStatement(FunctionParser.GotoStatementContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterGotoStatement(ctx);
        }
    
        //@override
        public override void EnterDeclByType(FunctionParser.DeclByTypeContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterDeclByType(ctx, ctx.type_name());
        }
    
        //@override
        public override void ExitDeclByType(FunctionParser.DeclByTypeContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitDeclByType();
        }
    
        //@override
        public override void EnterDeclByClass(FunctionParser.DeclByClassContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterDeclByClass(ctx);
        }
    
        //@override
        public override void ExitDeclByClass(FunctionParser.DeclByClassContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitDeclByClass();
        }
    
        //@override
        public override void EnterInitDeclSimple(FunctionParser.InitDeclSimpleContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterInitDeclSimple(ctx);
        }
    
        //@override
        public override void ExitInitDeclSimple(FunctionParser.InitDeclSimpleContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitInitDeclSimple();
        }
    
        //@override
        public override void EnterInitDeclWithAssign(FunctionParser.InitDeclWithAssignContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterInitDeclWithAssign(ctx);
        }
    
        //@override
        public override void ExitInitDeclWithAssign(FunctionParser.InitDeclWithAssignContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitInitDeclWithAssign(ctx);
        }
    
        //@override
        public override void EnterInitDeclWithCall(FunctionParser.InitDeclWithCallContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterInitDeclWithCall(ctx);
        }    
        //@override
        public override void ExitInitDeclWithCall(FunctionParser.InitDeclWithCallContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitInitDeclWithCall();
        }
    
        //@override
        public override void EnterCondition(FunctionParser.ConditionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterCondition(ctx);
        }
    
        //@override
        public override void ExitCondition(FunctionParser.ConditionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitCondition(ctx);
        }
    
    
        //@override
        public override void EnterExpr(FunctionParser.ExprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterExpression(ctx);
        }
    
        //@override
        public override void ExitExpr(FunctionParser.ExprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitExpression(ctx);
        }
    
        //@override
        public override void EnterAssign_expr(FunctionParser.Assign_exprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterAssignment(ctx);
        }
    
        //@override
        public override void ExitAssign_expr(FunctionParser.Assign_exprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitAssignment(ctx);
        }

    
        //@override
        public override void EnterCndExpr(FunctionParser.CndExprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterConditionalExpr(ctx);

        }

        //@override
        public override void ExitCndExpr(FunctionParser.CndExprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitConditionalExpr(ctx);
        }    
    
        //@override
        public override void EnterOr_expression(FunctionParser.Or_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterOrExpression(ctx);
        }
    
        //@override
        public override void ExitOr_expression(FunctionParser.Or_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitrOrExpression(ctx);
        }
    
        //@override
        public override void EnterAnd_expression(FunctionParser.And_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterAndExpression(ctx);
        }
    
        //@override
        public override void ExitAnd_expression(FunctionParser.And_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitAndExpression(ctx);
        }
    

        //@override
        public override void EnterInclusive_or_expression(FunctionParser.Inclusive_or_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterInclusiveOrExpression(ctx);
        }
    
        //@override
        public override void ExitInclusive_or_expression(FunctionParser.Inclusive_or_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitInclusiveOrExpression(ctx);
        }

        //@override
        public override void EnterExclusive_or_expression(FunctionParser.Exclusive_or_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterExclusiveOrExpression(ctx);
        }
    
        //@override
        public override void ExitExclusive_or_expression(FunctionParser.Exclusive_or_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitExclusiveOrExpression(ctx);
        }

        //@override
        public override void EnterBit_and_expression(FunctionParser.Bit_and_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterBitAndExpression(ctx);
        }
    
        //@override
        public override void ExitBit_and_expression(FunctionParser.Bit_and_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitBitAndExpression(ctx);
        }
    
        //@override
        public override void EnterEquality_expression(FunctionParser.Equality_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterEqualityExpression(ctx);
        }
    
        //@override
        public override void ExitEquality_expression(FunctionParser.Equality_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitEqualityExpression(ctx);
        }
    
    
        //@override
        public override void EnterRelational_expression(FunctionParser.Relational_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterRelationalExpression(ctx);
        }
    
        //@override
        public override void ExitRelational_expression(FunctionParser.Relational_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitRelationalExpression(ctx);
        }
    
        //@override
        public override void EnterShift_expression(FunctionParser.Shift_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterShiftExpression(ctx);
        }
    
        //@override
        public override void ExitShift_expression(FunctionParser.Shift_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitShiftExpression(ctx);
        }
    
        //@override
        public override void EnterAdditive_expression(FunctionParser.Additive_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterAdditiveExpression(ctx);
        }
    
        //@override
        public override void ExitAdditive_expression(FunctionParser.Additive_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitAdditiveExpression(ctx);
        }
    
        //@override
        public override void EnterMultiplicative_expression(FunctionParser.Multiplicative_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterMultiplicativeExpression(ctx);
        }
    
        //@override
        public override void ExitMultiplicative_expression(FunctionParser.Multiplicative_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitMultiplicativeExpression(ctx);
        }
    
        //@override
        public override void EnterCast_expression(FunctionParser.Cast_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterCastExpression(ctx);
        }
    
        //@override
        public override void ExitCast_expression(FunctionParser.Cast_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitCastExpression(ctx);
        }
    
        //@override
        public override void EnterCast_target(FunctionParser.Cast_targetContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterCast_target(ctx);
        }
    
        //@override
        public override void ExitCast_target(FunctionParser.Cast_targetContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitCast_target(ctx);
        }
    
        //@override
        public override void EnterFuncCall(FunctionParser.FuncCallContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterFuncCall(ctx);
        }
    
        //@override
        public override void ExitFuncCall(FunctionParser.FuncCallContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitFuncCall(ctx);
        }
    
        //@override
        public override void EnterSizeof_expression([NotNull] FunctionParser.Sizeof_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterSizeofExpr(ctx);
        }
    
        //@override
        public override void ExitSizeof_expression([NotNull] FunctionParser.Sizeof_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitSizeofExpr(ctx);
        }
    
        //@override
        public override void EnterSizeof([NotNull] FunctionParser.SizeofContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterSizeof(ctx);
        }
    
    
        //@override
        public override void ExitSizeof([NotNull] FunctionParser.SizeofContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitSizeof(ctx);
        }
    
        //@override
        public override void EnterUnary_op_and_cast_expr([NotNull] FunctionParser.Unary_op_and_cast_exprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterUnaryOpAndCastExpr(ctx);
        }

        //@override
        public override void ExitUnary_op_and_cast_expr([NotNull] FunctionParser.Unary_op_and_cast_exprContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitUnaryOpAndCastExpr(ctx);
        }
    
        //@override
        public override void EnterUnary_operator([NotNull] FunctionParser.Unary_operatorContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterUnaryOperator(ctx);
        }

        //@override
        public override void ExitUnary_operator([NotNull] FunctionParser.Unary_operatorContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitUnaryOperator(ctx);
        }

    
    
        //@override
        public override void EnterFunction_argument_list(FunctionParser.Function_argument_listContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterArgumentList(ctx);
        }
    
        //@override
        public override void ExitFunction_argument_list(FunctionParser.Function_argument_listContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitArgumentList(ctx);    
        }
    
        //@override
        public override void EnterInc_dec(FunctionParser.Inc_decContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterIncDec(ctx);
        }
    
        //@override
        public override void ExitInc_dec(FunctionParser.Inc_decContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitIncDec(ctx);
        }
    
        //@override
        public override void EnterArrayIndexing(FunctionParser.ArrayIndexingContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterArrayIndexing(ctx);
        }
    
        //@override
        public override void ExitArrayIndexing(FunctionParser.ArrayIndexingContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitArrayIndexing(ctx);
        }
    
        //@override
        public override void EnterMemberAccess(FunctionParser.MemberAccessContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterMemberAccess(ctx);
        }
    
        //@override
        public override void ExitMemberAccess(FunctionParser.MemberAccessContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitMemberAccess(ctx);
        }
    
        //@override
        public override void EnterPtrMemberAccess(FunctionParser.PtrMemberAccessContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterPtrMemberAccess(ctx);
        }
    
        //@override
        public override void ExitPtrMemberAccess(FunctionParser.PtrMemberAccessContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitPtrMemberAccess(ctx);
        }
    
    
        //@override
        public override void EnterIncDecOp(FunctionParser.IncDecOpContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterIncDecOp(ctx);
        }
    
        //@override
        public override void ExitIncDecOp(FunctionParser.IncDecOpContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitIncDecOp(ctx);
        }
    
        //@override
        public override void EnterPrimary_expression(FunctionParser.Primary_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterPrimary(ctx);
        }
    
        //@override
        public override void ExitPrimary_expression(FunctionParser.Primary_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitPrimary(ctx);
        }
    
        //@override
        public override void EnterUnary_expression(FunctionParser.Unary_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterUnaryExpression(ctx);
        }
    
        //@override
        public override void ExitUnary_expression(FunctionParser.Unary_expressionContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitUnaryExpression(ctx);
        }
    
        //@override
        public override void EnterIdentifier(FunctionParser.IdentifierContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterIdentifier(ctx);
        }
    
        //@override
        public override void ExitIdentifier(FunctionParser.IdentifierContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitIdentifier(ctx);
        }

        //@override
        public override void EnterFunction_argument(FunctionParser.Function_argumentContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterArgument(ctx);
        }
    
        //@override
        public override void ExitFunction_argument(FunctionParser.Function_argumentContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitArgument(ctx);
        }
    
        //@override
        public override void EnterInitializer_list(FunctionParser.Initializer_listContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterInitializerList(ctx);
        }
    
        //@override
        public override void ExitInitializer_list(FunctionParser.Initializer_listContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitInitializerList(ctx);
        }
    
        //@override
        public override void EnterSizeof_operand2([NotNull] FunctionParser.Sizeof_operand2Context ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterSizeofOperand2(ctx);
        }
    
        //@override
        public override void ExitSizeof_operand2([NotNull] FunctionParser.Sizeof_operand2Context ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitSizeofOperand2(ctx);
        }
    
        //@override
        public override void EnterSizeof_operand([NotNull] FunctionParser.Sizeof_operandContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.enterSizeofOperand(ctx);
        }
    
        //@override
        public override void ExitSizeof_operand([NotNull] FunctionParser.Sizeof_operandContext ctx)
        {
            FunctionContentBuilder builder = (FunctionContentBuilder) p.builderStack.Peek();
            builder.exitSizeofOperand(ctx);
        }
    
    }

}
