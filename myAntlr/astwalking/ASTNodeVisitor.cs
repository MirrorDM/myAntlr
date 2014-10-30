using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes;
using myAntlr.astnodes.expressions;
using myAntlr.astnodes.statements;
using myAntlr.astnodes.functionDef;
using myAntlr.astnodes.declarations;

// add by zdm. virtual/override handled

namespace myAntlr.astwalking
{
    public class ASTNodeVisitor
    {
        public virtual void visit(ASTNode item) { visitChildren(item); }

        public virtual void visit(ParameterList item) { defaultHandler(item); }
        public virtual void visit(FunctionDef item) { defaultHandler(item); }
        public virtual void visit(ClassDefStatement item) { defaultHandler(item); }
        public virtual void visit(IdentifierDeclStatement statementItem) { defaultHandler(statementItem); }
        public virtual void visit(ExpressionStatement statementItem) { defaultHandler(statementItem); }
        public virtual void visit(CallExpression expression) { defaultHandler(expression); }
        public virtual void visit(Condition expression) { defaultHandler(expression); }
        public virtual void visit(AssignmentExpr expression) { defaultHandler(expression); }

        public virtual void visit(PrimaryExpression expression) { defaultHandler(expression); }
        public virtual void visit(Identifier expression) { defaultHandler(expression); }
        public virtual void visit(MemberAccess expression) { defaultHandler(expression); }
        public virtual void visit(UnaryExpression expression) { defaultHandler(expression); }
        public virtual void visit(Argument expression) { defaultHandler(expression); }

        public virtual void visit(ReturnStatement expression) { defaultHandler(expression); }
        public virtual void visit(GotoStatement expression) { defaultHandler(expression); }
        public virtual void visit(ContinueStatement expression) { defaultHandler(expression); }
        public virtual void visit(BreakStatement expression) { defaultHandler(expression); }

        public virtual void visit(CompoundStatement expression) { defaultHandler(expression); }
        public virtual void visit(IfStatement expression) { defaultHandler(expression); }
        public virtual void visit(ForStatement expression) { defaultHandler(expression); }
        public virtual void visit(WhileStatement expression) { defaultHandler(expression); }
        public virtual void visit(DoStatement expression) { defaultHandler(expression); }
        public virtual void visit(Label expression) { defaultHandler(expression); }
        public virtual void visit(SwitchStatement expression) { defaultHandler(expression); }

        public virtual void defaultHandler(ASTNode item)
        {
            // by default, redirect to visit(ASTNode item)
            visit((ASTNode)item);
        }

        public virtual void visitChildren(ASTNode item)
        {
            int nChildren = item.getChildCount();

            for (int i = 0; i < nChildren; i++)
            {
                ASTNode child = item.getChild(i);
                child.accept(this);
            }

        }

    }

}
