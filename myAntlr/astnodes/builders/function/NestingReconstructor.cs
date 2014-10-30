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

// add by zdm. virtual/override handled
// No Class extends from this class.

namespace myAntlr.astnodes.builders.function
{
    public class NestingReconstructor
    {

        ContentBuilderStack stack;

        public NestingReconstructor(ContentBuilderStack aStack)
        {
            stack = aStack;
        }

        //protected void addItemToParent(ASTNode expression)
        public  void addItemToParent(ASTNode expression)
        {
            ASTNode topOfStack = stack.peek();
            topOfStack.addChild(expression);
        }

        // protected void consolidateSubExpression(ParserRuleContext ctx)
        public void consolidateSubExpression(ParserRuleContext ctx)
	    {
		    Expression expression = (Expression) stack.pop();
		    expression.initializeFromContext(ctx);
            // if(!(expression instanceof ExpressionHolder))
		    if(!(expression is ExpressionHolder))
			    expression = pullUpOnlyChild(expression);
		    addItemToParent(expression);
	    }

        private Expression pullUpOnlyChild(Expression expression)
        {
            if (expression.getChildCount() == 1)
                expression = (Expression)expression.getChild(0);
            return expression;
        }

        //protected void consolidate()
        public void consolidate()
	    {

		    ASTNode stmt = stack.pop();
		    ASTNode topOfStack = null;

		    if(stack.size() > 0)
			    topOfStack = stack.peek();

            // if(topOfStack instanceof CompoundStatement){
		    if(topOfStack is CompoundStatement){
			    CompoundStatement compound = (CompoundStatement)topOfStack;
			    compound.addStatement(stmt);
		    }else{
			    consolidateBlockStarters(stmt);
		    }

	    }

            // Joins consecutive BlockStarters on the stack

        //protected void consolidateBlockStarters(ASTNode node)
        public void consolidateBlockStarters(ASTNode node)
	    {

		    while(true){
			    try{
				    BlockStarter curBlockStarter = (BlockStarter) stack.peek();
				    curBlockStarter = (BlockStarter) stack.pop();
				    curBlockStarter.addChild(node);
				    node = curBlockStarter;


				    // if(curBlockStarter instanceof IfStatement){
				    if(curBlockStarter is IfStatement){

                        // if(stack.size() > 0 && stack.peek() instanceof ElseStatement){
					    if(stack.size() > 0 && stack.peek() is ElseStatement){
						    // This is an if inside an else, e.g., 'else if' handling
						
						    BlockStarter elseItem = (BlockStarter) stack.pop();
						    elseItem.addChild(curBlockStarter);

						    IfStatement lastIf = (IfStatement) stack.getIfInElseCase();
						    if( lastIf != null){
							    lastIf.setElseNode((ElseStatement) elseItem);
						    }
						
						    return;
					    }
					
				    }
                    // else if(curBlockStarter instanceof ElseStatement){
                    else if(curBlockStarter is ElseStatement){
					    // add else statement to the previous if-statement,
					    // which has already been consolidated so we can return
					
					    IfStatement lastIf = (IfStatement) stack.getIf();
					    if(lastIf != null)
						    lastIf.setElseNode((ElseStatement) curBlockStarter);
					    else
						    // System.err.println("Warning: cannot find if for else");
                            System.Console.Error.WriteLine("Warning: cannot find if for else");
					
					    return;
				    }
                    // else if(curBlockStarter instanceof WhileStatement){
                    else if(curBlockStarter is WhileStatement){
					    // add while statement to the previous do-statement
					    // if that exists. Otherwise, do nothing special.
					
					    DoStatement lastDo = stack.getDo();
					    if(lastDo != null){
						    lastDo.addChild( ((WhileStatement) curBlockStarter).getCondition() );
						    return;
					    }
				    }

			    }
                // catch(ClassCastException ex){
                catch (InvalidCastException ex)
                {
				    break;
			    }
		    }
		    // Finally, add chain to top compound-item
		    ASTNode root = stack.peek();
		    root.addChild(node);
	    }

    }

}
