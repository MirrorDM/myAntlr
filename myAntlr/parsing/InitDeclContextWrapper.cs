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

// add by zdm. virtual/override handled

namespace myAntlr.parsing
{
    public class InitDeclContextWrapper
    {
        ModuleParser.DeclaratorContext ctxCodeSensor = null;
        FunctionParser.DeclaratorContext ctxFine = null;
        int contextInUse;

        public InitDeclContextWrapper(ModuleParser.DeclaratorContext ctx)
        {
            ctxCodeSensor = ctx;
            contextInUse = 0;
        }

        public InitDeclContextWrapper(FunctionParser.DeclaratorContext ctx)
        {
            ctxFine = ctx;
            contextInUse = 2;
        }

        // public InitDeclContextWrapper(ParseTree objToWrap)
        public InitDeclContextWrapper(IParseTree objToWrap)
	    {
            // if(objToWrap instanceof ModuleParser.Init_declaratorContext){
		    if(objToWrap is ModuleParser.Init_declaratorContext){
			    // ctxCodeSensor = (ModuleParser.DeclaratorContext) objToWrap.getChild(0);
                ctxCodeSensor = (ModuleParser.DeclaratorContext) objToWrap.GetChild(0);
			    contextInUse = 0;
		    }
            // else if(objToWrap instanceof FunctionParser.Init_declaratorContext){
            else if(objToWrap is FunctionParser.Init_declaratorContext){
			    // ctxFine = (FunctionParser.DeclaratorContext) objToWrap.getChild(0);
                ctxFine = (FunctionParser.DeclaratorContext)objToWrap.GetChild(0);
			    contextInUse = 2;
		    }
	    }

        public virtual ParserRuleContext getWrappedObject()
        {
            switch (contextInUse)
            {
                case 0: return ctxCodeSensor;
                case 2: return ctxFine;
            }
            return null;
        }

        public virtual ParserRuleContext ptrs()
        {
            switch (contextInUse)
            {
                case 0: return ctxCodeSensor.ptrs();
                case 2: return ctxFine.ptrs();
            }
            return null;
        }

        public virtual ParserRuleContext type_suffix()
        {
            switch (contextInUse)
            {
                case 0: return ctxCodeSensor.type_suffix();
                case 2: return ctxFine.type_suffix();
            }
            return null;
        }

        public virtual ParserRuleContext identifier()
        {
            switch (contextInUse)
            {
                case 0: return ctxCodeSensor.identifier();
                case 2: return ctxFine.identifier();
            }
            return null;
        }

    }

}
