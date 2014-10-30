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

namespace myAntlr.parsing
{
    public class ANTLRModuleParserDriver : ANTLRParserDriver
    {   
    
        public ANTLRModuleParserDriver() : base()
        {
            // super();
            setListener(new ModuleParserTreeListener(this));
        }

        // @Override
        public override IParseTree parseTokenStreamImpl(TokenSubStream tokens)
        {
            ModuleParser parser = new ModuleParser(tokens);
            IParseTree tree = null;
        
            try {
                setSLLMode(parser);
                tree = parser.code();
            } 
            // catch (RuntimeException ex) {
            catch (SystemException ex) {
                if (isRecognitionException(ex))
                {
                    // tokens.reset();
                    tokens.Reset();
                    setLLStarMode(parser);
                    tree = parser.code();
                }
            }
            return tree;
        }

        // @Override
        // public override Lexer createLexer(ANTLRInputStream input)
        public override Lexer createLexer(AntlrInputStream input)
        {
            return new ModuleLexer(input);
        }
    
    }
}
