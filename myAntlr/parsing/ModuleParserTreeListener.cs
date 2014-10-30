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


using myAntlr.astnodes.builders;
using myAntlr.astnodes.builders.function;
using myAntlr.astnodes.declarations;
using myAntlr.astnodes.statements;

namespace myAntlr.parsing
{
    public class ModuleParserTreeListener : ModuleBaseListener
    {
    
        ANTLRParserDriver p;
    
        public ModuleParserTreeListener(ANTLRParserDriver aP)
        {
            p = aP;
        }
    
        //@Override
        public override void EnterCode(ModuleParser.CodeContext ctx)
        {
            p.notifyObserversOfUnitStart(ctx);
        }
    
        //@Override
        public override void ExitCode(ModuleParser.CodeContext ctx)
        {
            p.notifyObserversOfUnitEnd(ctx);
        }
    
        ///////////////////////////////////////////////////////////////
        // This is where the ModuleParser invokes the FunctionParser
        ///////////////////////////////////////////////////////////////
        // This function is invoked when a Function_Def parse tree node
        // is entered. This is where we hand over the function contents to
        // the function parser and connect the AST node created for the
        // function definition to the AST created by the function parser.
        //////////////////////////////////////////////////////////////////
    
        //@Override
        public override void EnterFunction_def(ModuleParser.Function_defContext ctx)
        {
        
            FunctionDefBuilder builder = new FunctionDefBuilder();
            builder.createNew(ctx);
            p.builderStack.Push(builder);
    
            CompoundStatement functionContent =
                    ModuleFunctionParserInterface.parseFunctionContents(ctx);
            builder.setContent(functionContent);
        }

        //@Override
        public override void ExitFunction_def(ModuleParser.Function_defContext ctx)
        {
            FunctionDefBuilder builder = (FunctionDefBuilder) p.builderStack.Pop();     
            p.notifyObserversOfItem(builder.getItem());
        }
    
        //@Override
        public override void EnterReturn_type(ModuleParser.Return_typeContext ctx)
        {
            FunctionDefBuilder builder = (FunctionDefBuilder) p.builderStack.Peek();
            builder.setReturnType(ctx, p.builderStack);
        }
    
        //@Override
        public override void EnterFunction_name(ModuleParser.Function_nameContext ctx)
        {
            FunctionDefBuilder builder = (FunctionDefBuilder) p.builderStack.Peek();
            builder.setName(ctx, p.builderStack);
        }
    
        //@Override
        public override void EnterFunction_param_list(ModuleParser.Function_param_listContext ctx)
        {
            FunctionDefBuilder builder = (FunctionDefBuilder) p.builderStack.Peek();
            builder.setParameterList(ctx, p.builderStack);
        }
    
        //@Override
        public override void EnterParameter_decl(ModuleParser.Parameter_declContext ctx)
        {
            FunctionDefBuilder builder = (FunctionDefBuilder) p.builderStack.Peek();
            builder.addParameter(ctx, p.builderStack);
        }
    
        // DeclByType
    
        //@Override
        public override void EnterDeclByType(ModuleParser.DeclByTypeContext ctx)
        {
            ModuleParser.Init_declarator_listContext decl_list = ctx.init_declarator_list();
            ModuleParser.Type_nameContext typeName = ctx.type_name();
            emitDeclarations(decl_list, typeName, ctx);
        }
        
        private void emitDeclarations(ParserRuleContext decl_list,
                  ParserRuleContext typeName, ParserRuleContext ctx)
        {
            IdentifierDeclBuilder builder = new IdentifierDeclBuilder();
            List<IdentifierDecl> declarations = builder.getDeclarations(decl_list, typeName);

            IdentifierDeclStatement stmt = new IdentifierDeclStatement();
            // stmt.initializeFromContext(ctx);
        
            // Iterator<IdentifierDecl> it = declarations.iterator();
            IEnumerator<IdentifierDecl> it = declarations.GetEnumerator();
            // while(it.hasNext()){
            while(it.MoveNext()){
                // IdentifierDecl decl = it.next();
                IdentifierDecl decl = it.Current;
                stmt.addChild(decl);
            }       
    
            p.notifyObserversOfItem(stmt);
        }
    
        // DeclByClass
    
        //@Override
        public override void EnterDeclByClass(ModuleParser.DeclByClassContext ctx)
        {
            ClassDefBuilder builder = new ClassDefBuilder();
            builder.createNew(ctx);
            p.builderStack.Push(builder);       
        }

        //@Override
        public override void ExitDeclByClass(ModuleParser.DeclByClassContext ctx)
        {
            ClassDefBuilder builder = (ClassDefBuilder) p.builderStack.Pop();
        
            CompoundStatement content = parseClassContent(ctx);
            builder.setContent(content);
        
            p.notifyObserversOfItem(builder.getItem());     
            emitDeclarationsForClass(ctx);
        }
    
        //@Override
        public override void EnterClass_name(ModuleParser.Class_nameContext ctx)
        {
            ClassDefBuilder builder = (ClassDefBuilder) p.builderStack.Peek();
            builder.setName(ctx);
        }

        private void emitDeclarationsForClass(ModuleParser.DeclByClassContext ctx)
        {

            ModuleParser.Init_declarator_listContext decl_list = ctx.init_declarator_list();     
            if(decl_list == null)
                return;
        
            ParserRuleContext typeName = ctx.class_def().class_name();
            emitDeclarations(decl_list, typeName, ctx);
        }
    
        private CompoundStatement parseClassContent(ModuleParser.DeclByClassContext ctx)
        {
            ANTLRModuleParserDriver shallowParser = createNewShallowParser();
            CompoundItemAssembler generator = new CompoundItemAssembler();
            shallowParser.addObserver(generator);

            restrictStreamToClassContent(ctx);
            shallowParser.parseAndWalkTokenStream(p.stream);
            p.stream.resetRestriction();
        
            return generator.getCompoundItem();
        }

        private void restrictStreamToClassContent(ModuleParser.DeclByClassContext ctx)
        {
            ModuleParser.Class_defContext class_def = ctx.class_def();
            // int startIndex = class_def.OPENING_CURLY().getSymbol().getTokenIndex();
            int startIndex = class_def.OPENING_CURLY().Symbol.TokenIndex;
            // int stopIndex = class_def.stop.getTokenIndex();
            int stopIndex = class_def.stop.TokenIndex;
        
            p.stream.restrict(startIndex+1, stopIndex);
        }

        private ANTLRModuleParserDriver createNewShallowParser()
        {
            ANTLRModuleParserDriver shallowParser = new ANTLRModuleParserDriver();
            shallowParser.setStack(p.builderStack);
            return shallowParser;
        }
    
    }

}
