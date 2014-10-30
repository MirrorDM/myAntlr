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

using myAntlr.astnodes;
using myAntlr.parsing;


namespace myAntlr.tests.parseTreeToAST
{
    public class FunctionContentTestUtil
    {
        public static ASTNode parseAndWalk(String input)
        {
            ANTLRFunctionParserDriver parser = new ANTLRFunctionParserDriver();
            TokenSubStream tokens = tokenStreamFromString(input);
            parser.parseAndWalkTokenStream(tokens);
            // return parser.builderStack.peek().getItem();
            return parser.builderStack.Peek().getItem();
        }


        static IParseTree parse(String input)
        {
            ANTLRFunctionParserDriver parser = new ANTLRFunctionParserDriver();
            return parser.parseString(input);
        }

        private static TokenSubStream tokenStreamFromString(String input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            FunctionLexer lex = new FunctionLexer(inputStream);
            TokenSubStream tokens = new TokenSubStream(lex);
            return tokens;
        }
    }

}
