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

namespace myAntlr.parsing
{
    public class ModuleFunctionParserInterface
    {
        // Extracts compound statement from input stream
        // as a string and passes that string to the
        // function parser. The resulting 'CompoundStatement'
        // (an AST node) is returned.

        // public static CompoundStatement parseFunctionContents(Function_defContext ctx)
        public static CompoundStatement parseFunctionContents(ModuleParser.Function_defContext ctx)
        {
            String text = getCompoundStmtAsString(ctx);

            FunctionParserInParsing parser = new FunctionParserInParsing();

            try
            {
                parser.parseAndWalkString(text);
            }
            // catch (RuntimeException ex)
            catch (SystemException ex)
            {
                System.Console.Error.WriteLine("Error parsing function " +
                                  ctx.function_name().GetText()
                                  + ". skipping.");

                //ex.printStackTrace();
                System.Console.Error.WriteLine(ex.StackTrace);
            }
            return parser.getResult();
        }

        private static String getCompoundStmtAsString(ModuleParser.Function_defContext ctx)
        {
            ModuleParser.Compound_statementContext compound_statement = ctx.compound_statement();

            // CharStream inputStream = compound_statement.start.getInputStream();
            ICharStream inputStream = compound_statement.start.InputStream;
            // int startIndex = compound_statement.start.getStopIndex();
            int startIndex = compound_statement.start.StopIndex;
            // int stopIndex = compound_statement.stop.getStopIndex();
            int stopIndex = compound_statement.stop.StopIndex;

            //return inputStream.getText(new Interval(startIndex + 1, stopIndex - 1));
            return inputStream.GetText(new Interval(startIndex + 1, stopIndex - 1));
        }

    }

}
