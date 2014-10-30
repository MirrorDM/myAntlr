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

using myAntlr.astwalking;
using myAntlr.astnodes.expressions;
using myAntlr.parsing;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes
{
    public class ASTNode {

	    protected String codeStr = null;		
	    protected ParserRuleContext parseTreeNodeContext;
	    private CodeLocation location = new CodeLocation();
	
	    // private boolean isInCFG = false;
	    private bool _isInCFG = false;
	    protected LinkedList<ASTNode> children;
	    protected int childNumber;
	
	    public virtual void addChild(ASTNode node)
	    { 
		    if(children == null)
			    children = new LinkedList<ASTNode>();
            //node.setChildNumber(children.size());
            node.setChildNumber(children.Count());
            //children.add(node);
            children.AddLast(node);
	    }

        public virtual int getChildCount()
        {
            if(children == null) return 0; return children.Count();
        }
	    public virtual ASTNode getChild(int i)
	    {
		    if(children == null) return null;
		
		    ASTNode retval;
		    try{
                //retval = children.get(i);
                retval = children.ElementAt(i);
		    }
            //catch(IndexOutOfBoundsException ex){
            catch(ArgumentOutOfRangeException ex){
			    return null;
		    }
		    return retval;
	    }

        public virtual ASTNode popLastChild()
        { 
            //return children.removeLast();
            ASTNode lst = children.Last();
            children.RemoveLast();
            return lst;
        }

        private void setChildNumber(int num)
	    {
		    childNumber = num;
	    }

        public virtual int getChildNumber()
	    {
		    return childNumber;	
	    }

        public virtual void initializeFromContext(ParserRuleContext ctx)
	    {
		    parseTreeNodeContext = ctx;
	    }

        public virtual void setLocation(ParserRuleContext ctx)
	    {
		    if(ctx == null) return;
		    location = new CodeLocation(ctx);
	    }

        public virtual void setCodeStr(String aCodeStr)
	    {
		    codeStr = aCodeStr;
	    }

        public virtual String getEscapedCodeStr()
	    {
		    if(codeStr != null)
			    return codeStr;
		
		    codeStr = escapeCodeStr(ParseTreeUtils.childTokenString(parseTreeNodeContext));
		    return codeStr;
	    }

        private String escapeCodeStr(String codeStr)
        {
		    String retval = codeStr;
            //retval = retval.replace("\n", "\\n");
            retval = retval.Replace("\n", "\\n");
            //retval = retval.replace("\t", "\\t");
            retval = retval.Replace("\t", "\\t");
		    return retval;
	    }

        public virtual String getLocationString()
	    {
		    setLocation(parseTreeNodeContext);
		    return location.ToString();
	    }

        public virtual void accept(ASTNodeVisitor visitor) { visitor.visit(this); }

        //public boolean isLeaf()
        public virtual bool isLeaf()
	    {
		    return (children.Count() == 0);
	    }

        public virtual String getTypeAsString()
	    {
            //return this.getClass().getSimpleName();
            // Don't Know.
            return this.GetType().Name;
	    }

        public virtual void markAsCFGNode()
	    {
            //isInCFG = true;
            _isInCFG = true;
	    }

        //public boolean isInCFG()
        public virtual bool isInCFG()
	    {
            //return isInCFG;
            return _isInCFG;
	    }

        public virtual String getOperatorCode() 
        {
            // if(BinaryExpression.class.isAssignableFrom(this.getClass())){
            if (typeof(BinaryExpression).IsAssignableFrom(this.GetType()))
            {
                return ((BinaryExpression) this).getOperator();
            }
            // TODO Auto-generated method stub
            return null;
        }
	
    }

}
