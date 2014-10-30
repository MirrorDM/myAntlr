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
    public class ANTLRFunctionParserDriver : ANTLRParserDriver
    {
    
        public ANTLRFunctionParserDriver() : base()
        {
            // super();
            setListener(new FunctionParseTreeListener(this));
        }
    
        // @Override
        // public Lexer createLexer(ANTLRInputStream input)
        public override Lexer createLexer(AntlrInputStream input)
        {
            return new FunctionLexer(input);
        }
    
        // @Override
        public override IParseTree parseTokenStreamImpl(TokenSubStream tokens)
        {
            setAntlrParser(new FunctionParser(tokens));
            FunctionParser thisParser = (FunctionParser) getAntlrParser();
            IParseTree tree = null;
        
            try {
                setSLLMode(getAntlrParser());
                tree = thisParser.statements();
            }
            // catch (RuntimeException ex) {
            catch (SystemException ex) {
                if (isRecognitionException(ex))
                {
                    // tokens.reset();
                    tokens.Reset();
                    setLLStarMode(getAntlrParser());
                    tree = thisParser.statements();
                }
        
            }
            return tree;
        }
    }
}
