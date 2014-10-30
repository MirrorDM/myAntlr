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

using myAntlr.astnodes.declarations;
using myAntlr.parsing;
using myAntlr.astnodes.expressions;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.builders
{
    public class IdentifierDeclBuilder : ASTNodeBuilder
    {
	    IdentifierDecl thisItem;
	
	    // @Override
	    public override void createNew(ParserRuleContext ctx)
	    {
		    item = new IdentifierDecl();
		    thisItem = (IdentifierDecl) item;
		    item.initializeFromContext(ctx);
	    }

	    public virtual void setType(InitDeclContextWrapper decl_ctx,
						    ParserRuleContext typeName)
	    {
		    String baseType = "";
		    if(typeName != null)
			    baseType = ParseTreeUtils.childTokenString(typeName);
		    String completeType = baseType;
		    if(decl_ctx.ptrs() != null)
			    completeType += " " + ParseTreeUtils.childTokenString(decl_ctx.ptrs());
		    if(decl_ctx.type_suffix() != null)
			    completeType += " " + ParseTreeUtils.childTokenString(decl_ctx.type_suffix());
		
		    IdentifierDeclType newType = new IdentifierDeclType();
		    newType.initializeFromContext(decl_ctx.getWrappedObject());
		    newType.baseType = baseType;
		    newType.completeType = completeType;
		    thisItem.setType(newType);
	    }

	    public virtual void setName(InitDeclContextWrapper decl_ctx)
	    {
		    ParserRuleContext identifier = decl_ctx.identifier();
		    Identifier newName = new Identifier();
		    newName.initializeFromContext(identifier);
		
		    thisItem.setName(newName);
	    }
	
	    public virtual List<IdentifierDecl> getDeclarations(ParserRuleContext decl_list,
												    ParserRuleContext typeName)
	    {
            // List<IdentifierDecl> declarations = new LinkedList<IdentifierDecl>();
		    List<IdentifierDecl> declarations = new LinkedList<IdentifierDecl>().ToList<IdentifierDecl>();
            InitDeclContextWrapper decl_ctx;

            // for(Iterator<ParseTree> i = decl_list.children.iterator(); i.hasNext();)
		    for(IEnumerator<IParseTree> i = decl_list.children.GetEnumerator(); i.MoveNext();)
		    {
			    decl_ctx = new InitDeclContextWrapper(i.Current);
			    // for ','s
			    if(decl_ctx.getWrappedObject() == null) continue;
			
			    createNew(decl_ctx.getWrappedObject());
			    setType(decl_ctx, typeName);
			    setName(decl_ctx);

                // declarations.add((IdentifierDecl) getItem());
			    declarations.Add((IdentifierDecl) getItem());
		    }
		    return declarations;
	    }
	
	
    }

}
