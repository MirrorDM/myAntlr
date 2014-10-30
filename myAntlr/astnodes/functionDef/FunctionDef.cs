using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes.expressions;
using myAntlr.astnodes.statements;
using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.functionDef
{
    public class FunctionDef : ASTNode
    {

	    public Identifier name = new DummyNameNode();
	    private ParameterList parameterList = new ParameterList();
	    public ReturnType returnType = new DummyReturnType();
	
	    CompoundStatement content = new CompoundStatement();
	
	    public virtual CompoundStatement getContent()
	    {
		    return content;
	    }
	
	    public virtual void addStatement(ASTNode statement)
	    {
		    content.addStatement(statement);
	    }
	
	    public virtual void addParameter(Parameter aParameter)
	    {
		    getParameterList().addParameter(aParameter);
	    }
	
	    // @Override
	    public override String getEscapedCodeStr()
	    {
		    // check if codeStr has already been generated
		    if(codeStr != null)
			    return codeStr;
		    codeStr = getFunctionSignature();
		    return codeStr;
	    }
	
	    public virtual String getFunctionSignature()
	    {
		    String retval = name.getEscapedCodeStr();
		    if(getParameterList() != null)
			    retval += " (" + getParameterList().getEscapedCodeStr() + ")";
		    else
			    retval += " ()";
		    return retval;
	    }

	    public virtual void setContent(CompoundStatement functionContent)
	    {
		    content = functionContent;
		    addChild(content);
	    }
	
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }

	    public ParameterList getParameterList() {
		    return parameterList;
	    }

        public virtual void setParameterList(ParameterList parameterList)
        {
		    this.parameterList = parameterList;
		    this.addChild(this.parameterList);
	    }
	
    }

}
