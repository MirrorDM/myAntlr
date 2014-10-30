using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Atn;

using myAntlr.tests.cfgCreation;
using myAntlr.cfg;


namespace myAntlr
{
    class Program
    {
        static List<FunctionNode> functionlist = new List<FunctionNode>();

        static void Main(string[] args)
        {

            Stopwatch stw = new Stopwatch();
            stw.Start();

            // string pt = "C:\\Users\\v-dazou\\Documents\\linux-3.16.1\\sound";
            string pt = "C:\\Users\\v-dazou\\Documents\\sound";
            DirectoryWalker walker = new DirectoryWalker(pt);
            // for test.
            walker.setMaxDepth(0);
            List<string> files = walker.getAllfiles();
            Console.WriteLine(files.Count);

            int currentfile = 0;

            int filecount = files.Count;
            for (int i = 0; i < filecount; i++)
            {
                string filepath = files[i];
                FunctionNodeList nodelist = new FunctionNodeList(filepath);
                List<FunctionNode> filenodes = nodelist.getFunctionNodeListFromFile();
                functionlist.AddRange(filenodes);
                

                Console.Write(currentfile + " / " + files.Count + " ");
                currentfile++;
                Console.WriteLine("time: " + stw.Elapsed.Hours.ToString("D2") + ":" + stw.Elapsed.Minutes.ToString("D2") + ":" + stw.Elapsed.Seconds.ToString("D2"));
            }

            // binarization && compress chain
            foreach (FunctionNode fnode in functionlist)
            {
                fnode.structureblock();
                fnode.compresschain();
                fnode.binarization();
                //fnode.outputdot();
            }


            Console.WriteLine("total functions: " + functionlist.Count);
            //Thread.Sleep(1000);

            FunctionTreeVisitor funcvisitor = new FunctionTreeVisitor(functionlist);
            funcvisitor.countContextFreeGrammar();
            //funcvisitor.printGrammar();
            PCFG pCFG = funcvisitor.getPCFG();
            pCFG.printGrammar();

            PTSGprior pTSGprior = new PTSGprior(pCFG);
            pTSGprior.generatePTSG();
            pTSGprior.outputPTSG();


            //TSG tmp = funcvisitor.getOneTSGRandomly();
            //Console.WriteLine(tmp.getSequence());

            PostPTSG postPTSG = new PostPTSG(funcvisitor, pTSGprior);
            postPTSG.calculatePostPTSG();
            
            stw.Stop();
        }

    }


}
