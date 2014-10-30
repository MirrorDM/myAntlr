using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Atn;

namespace myAntlr
{
    public class FunctionNodeList
    {
        string filepath;
        List<FunctionNode> functionlist = new List<FunctionNode>();

        public FunctionNodeList(string path)
        {
            filepath = path;
        }

        public List<FunctionNode> getFunctionNodeListFromFile()
        {
            StreamReader inputStream = new StreamReader(filepath);
            string inputstr = inputStream.ReadToEnd();
            AntlrInputStream input = new AntlrInputStream(inputstr);
            ModuleLexer lexer = new ModuleLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            ModuleParser parser = new ModuleParser(tokens);
            IParseTree tree = parser.code(); // in Module.g4, code is the root node of AST tree.

            int count = tree.ChildCount;
            for (int i = 0; i < count; i++)
            {
                IParseTree func = tree.GetChild(i);
                // Console.WriteLine("****type****");
                // Console.WriteLine(func.GetType());
                // Console.WriteLine(func.ChildCount);
                // Console.WriteLine("****type****");
                if (func is ModuleParser.Function_defContext)
                {
                    //Console.WriteLine("****here****");
                    int countinfunc = func.ChildCount;
                    FunctionNode fnode = new FunctionNode();

                    for (int j = 0; j < countinfunc; j++)
                    {
                        IParseTree subnode = func.GetChild(j);
                        if (subnode is ModuleParser.Return_typeContext)
                        {
                            fnode.returntype = (ModuleParser.Return_typeContext)subnode;
                        }
                        if (subnode is ModuleParser.Function_nameContext)
                        {
                            fnode.functionname = (ModuleParser.Function_nameContext)subnode;
                            //Console.WriteLine(fnode.functionname.GetText());
                        }
                        if (subnode is ModuleParser.Function_param_listContext)
                        {
                            fnode.paramlist = (ModuleParser.Function_param_listContext)subnode;
                        }
                        if (subnode is ModuleParser.Compound_statementContext)
                        {
                            // for test
                            // Console.WriteLine("****compound****");

                            // String compoundtext = compoundstatement.GetText();
                            // Console.WriteLine(compoundtext);
                            Interval it = subnode.SourceInterval;
                            IList<IToken> tks = tokens.GetTokens(it.a, it.b);

                            // raw source code of compound statement
                            //Console.WriteLine(tks.First().StartIndex + "  " + tks.Last().StartIndex + "  " + tks.Last().StopIndex);
                            //Console.WriteLine(inputstr.Substring(tks.First().StartIndex, tks.Last().StopIndex - tks.First().StartIndex + 1));

                            String compoundtext = inputstr.Substring(tks.First().StartIndex, tks.Last().StopIndex - tks.First().StartIndex + 1);

                            //foreach (IToken tk in tks)
                            //{
                            //    Console.WriteLine(tk.Text);
                            //}
                            //CommonTokenStream funcTokens = new CommonTokenStream(tks);
                            //Console.WriteLine(it.a + " " + it.b);
                            //Console.WriteLine(inputstr.Substring(it.a, it.Length));
                            //Console.WriteLine(compoundtext);


                            AntlrInputStream funcInput = new AntlrInputStream(compoundtext);
                            FunctionLexer funcLex = new FunctionLexer(funcInput);
                            CommonTokenStream funcTokens = new CommonTokenStream(funcLex);
                            FunctionParser funcParser = new FunctionParser(funcTokens);
                            FunctionParser.StatementsContext funcTree = funcParser.statements();

                            fnode.functionstatements = funcTree;


                            //String funcS = funcTree.ToStringTree(funcParser);
                            //Console.WriteLine(funcS);
                            //Console.WriteLine("****");



                        }
                    }
                    functionlist.Add(fnode);
                }
                // Console.WriteLine("---------");
            }

            return functionlist;
            //StreamWriter outputStream = new StreamWriter(@"C:\Users\v-dazou\Documents\abc.tree");

            // String s = tree.ToStringTree(parser);

            // Console.WriteLine(s + "lenth: " + s.Length);

            //outputStream.WriteLine(s);
            // System.IO.File.WriteAllText(@"C:\Users\v-dazou\Documents\testfunc.tree", s);


            /*
            CFGCreatorTest cfgtest = new CFGCreatorTest();
            cfgtest.init();
            CFG cfg = cfgtest.getCFGForCode(inputstr);

            //// Add by zdm. Fix break and continue.
            //foreach (CFGNode st in cfg.loopStart.Keys)
            //{
            //    cfg.removeAllEdgesFrom(st);
            //    cfg.addEdge(st, cfg.loopStart[st]);
            //}
            //// Add by zdm. Fix break and continue.


            IEnumerator<CFGEdge> it = cfg.edgeIterator();
            int count = 0;
            while (it.MoveNext())
            {
                CFGEdge edge = it.Current;
                Console.WriteLine(edge.getSource().GetHashCode() + " to " +edge.getDestination().GetHashCode());
                Console.WriteLine(edge.getSource().getEscapedCodeStr() + " to " + edge.getDestination().getEscapedCodeStr());
                // Console.WriteLine(edge.getProperties().ToString());
                foreach (string property in edge.getProperties().Keys)
                {
                    Console.WriteLine(property + ": " + edge.getProperties()[property].ToString());
                }
                Console.WriteLine("------------------------------");
                count++;
            }
            Console.WriteLine("Count: " + count);
            Console.WriteLine("-----------break and continue--------------");

            foreach (CFGNode st in cfg.loopStart.Keys)
            {
                Console.WriteLine(st.getEscapedCodeStr() + " to " + cfg.loopStart[st].getEscapedCodeStr());
                Console.WriteLine("------------------------------");
            }
            */
        }
    }
}
