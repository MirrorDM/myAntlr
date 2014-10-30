using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Atn;

namespace myAntlr
{
    public class FunctionNode
    {
        public ModuleParser.Return_typeContext returntype;
        public ModuleParser.Function_nameContext functionname;
        public ModuleParser.Function_param_listContext paramlist;
        public FunctionParser.StatementsContext functionstatements;

        public void binarization()
        {
            binarizeParserRuleContext(functionstatements);
        }
        public void compresschain()
        {
            checkIsChainFirstNode(functionstatements);
        }
        public void structureblock()
        {
            isBlockStarter(functionstatements);
        }
        void isBlockStarter(ParserRuleContext node)
        {
            // statement -> block_starter -> for_statement
            // statement -> open_curly
            // ....
            // statement -> close_curly

            // We will add open_curly, ..., close_curly to block_starter.children

            if (node is FunctionParser.Block_starterContext)
            {
                Console.WriteLine("is block starter: " + node.GetType().ToString());
                ParserRuleContext stmt, stmts;
                if (node.parent != null)
                {
                    stmt = (ParserRuleContext)node.parent;
                    if (stmt.parent != null)
                    {
                        stmts = (ParserRuleContext)stmt.parent;
                    }
                    else
                    {
                        Console.Error.WriteLine("structure error at functionNode.cs");
                        return;
                    }
                }
                else
                {
                    Console.Error.WriteLine("structure error at functionNode.cs");
                    return;
                }
                // cat successor statement to blockstarter.
                int index = stmts.children.IndexOf(stmt);
                // check single expression
                ParserRuleContext nextstmt = (ParserRuleContext)stmts.children.ElementAt(index + 1);
                if (nextstmt.children.First() is FunctionParser.Expr_statementContext)
                {
                    stmts.children.Remove(nextstmt);
                    node.children.Add(nextstmt);
                    nextstmt.parent = node;
                }
                // check block statement. open_curly and matched close curly.
                else if (nextstmt.children.First() is FunctionParser.Opening_curlyContext)
                {
                    int curlycount = 1;
                    int closecurlypos = -1;
                    ParserRuleContext currentstmt;
                    for (int i = index + 2; i < stmts.children.Count(); i++)
                    {
                        currentstmt = (ParserRuleContext)stmts.children.ElementAt(i);
                        if (currentstmt.children.First() is FunctionParser.Opening_curlyContext)
                        {
                            curlycount++;
                        }
                        if (currentstmt.children.First() is FunctionParser.Closing_curlyContext)
                        {
                            curlycount--;
                            if (curlycount == 0)
                            {
                                closecurlypos = i;
                                break;
                            }
                        }
                    }
                    if (closecurlypos == -1)
                    {
                        Console.Error.WriteLine("structure error at functionNode.cs");
                        return;
                    }

                    // retain the outer open_curly and close_curly
                    List<IParseTree> removelist = new List<IParseTree>();
                    for (int i = index + 1; i <= closecurlypos; i++)
                    {
                        currentstmt = (ParserRuleContext)stmts.children.ElementAt(i);
                        // stmts.children.Remove(currentstmt);
                        removelist.Add(currentstmt);
                        node.children.Add(currentstmt);
                        currentstmt.parent = node;
                    }
                    foreach (IParseTree ipt in removelist)
                    {
                        stmts.children.Remove(ipt);
                    }

                }

            }
            if (node.children != null)
            {
                for (int i = 0; i < node.children.Count(); i++)
                {
                    if (node.children.ElementAt(i) is ParserRuleContext)
                    {
                        isBlockStarter((ParserRuleContext)node.children.ElementAt(i));
                    }
                }
            }
        }

        public void outputdot()
        {
            StreamWriter sw = new StreamWriter("C:\\Users\\v-dazou\\Documents\\for\\" + functionname.GetText() + ".dot");
            sw.WriteLine("graph ast {\nnode [label=\"\\N\"];");
            outputEachNode(functionstatements, sw);
            sw.WriteLine("}");
            sw.Flush();
            sw.Close();
        }
        void outputEachNode(IParseTree node, StreamWriter sw)
        {
            string label = node.GetType().ToString().Split('.').Last().Split('+').Last();
            sw.WriteLine(node.GetHashCode() + "   [label=\"" + label + "\"];");
            if (node is ParserRuleContext)
            {
                ParserRuleContext nonterminal = (ParserRuleContext)node;
                for (int i = 0; i < nonterminal.children.Count(); i++)
                {
                    outputEachNode(nonterminal.children.ElementAt(i), sw);
                    sw.WriteLine(nonterminal.GetHashCode() + " -- " + nonterminal.children.ElementAt(i).GetHashCode() + ";");
                }
            }
        }

        void checkIsChainFirstNode(ParserRuleContext node)
        {
            if (node.ChildCount == 1 && (node.children.ElementAt(0) is ParserRuleContext))
            {
                ParserRuleContext lastnode = getChainLastNode(node);
                lastnode.parent = node;
                List<IParseTree> newchildlist = new List<IParseTree>();
                newchildlist.Add(lastnode);
                node.children = newchildlist;
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                if (node.children.ElementAt(i) is ParserRuleContext)
                {
                    checkIsChainFirstNode((ParserRuleContext)node.children.ElementAt(i));
                }
            }
        }
        ParserRuleContext getChainLastNode(ParserRuleContext node)
        {
            if (node.ChildCount == 1 && (node.children.ElementAt(0) is ParserRuleContext))
            {
                return getChainLastNode((ParserRuleContext)node.children.ElementAt(0));
            }
            return node;
        }
        void binarizeParserRuleContext(ParserRuleContext root)
        {
            if (root.ChildCount > 2)
            {
                IList<IParseTree> childlist = root.children;
                IList<IParseTree> newlist = new List<IParseTree>();
                newlist.Add(childlist.First());
                DummyTreeNode dtn = new DummyTreeNode(functionstatements, 112233);
                dtn.children = new List<IParseTree>();
                childlist.RemoveAt(0);
                foreach (IParseTree ipt in childlist)
                {
                    dtn.children.Add(ipt);
                    if (ipt is ParserRuleContext)
                    {
                        ((ParserRuleContext)ipt).parent = dtn;
                    }
                    else if (ipt is TerminalNodeImpl)
                    {
                        ((TerminalNodeImpl)ipt).parent = dtn;
                    }
                    else
                    {
                        Console.Error.WriteLine("Type unhandled at FunctionNode.cs: " + ipt.GetType().ToString());
                    }
                }
                newlist.Add(dtn);
                root.children = newlist;
            }
            // Console.WriteLine("child count after binarize: " + root.ChildCount);
            for (int i = 0; i < root.ChildCount; i++)
            {
                if (root.children.ElementAt(i) is ParserRuleContext)
                {
                    binarizeParserRuleContext((ParserRuleContext)root.children.ElementAt(i));
                }
            }

        }
    }
}
