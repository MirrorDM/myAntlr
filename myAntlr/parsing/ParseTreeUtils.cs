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
    public class ParseTreeUtils
    {
        // public static String childTokenString(ParseTree ctx)
        public static String childTokenString(IParseTree ctx)
        {
            // TODO: Optimize this. Strings are immutable

            // The reason we don't just call ctx.getText()
            // here is because it removes whitespace, making
            // 'inti' from 'int i'.

            if (ctx == null)
                return "";

            // int nChildren = ctx.getChildCount();
            int nChildren = ctx.ChildCount;

            if (nChildren == 0)
            {
                // return ctx.getText();
                return ctx.GetText();
            }

            String retval = "";

            for (int i = 0; i < nChildren; i++)
            {
                // ParseTree child = ctx.getChild(i);
                IParseTree child = ctx.GetChild(i);
                String childText = childTokenString(child);
                // if (!childText.equals(""))
                if (!childText.Equals(""))
                {
                    retval += childText + " ";
                }
            }

            // if (retval.length() > 0)
            if (retval.Length > 0)
            {
                // retval = retval.substring(0, retval.length() - 1);
                retval = retval.Substring(0, retval.Length - 1);
            }
            return retval;
        }
    }

}
