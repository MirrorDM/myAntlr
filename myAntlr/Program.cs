using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

            IFormatter serializationformatter;
            Stream serializationstream;


            Stopwatch stw = new Stopwatch();
            stw.Start();

            // string pt = "C:\\Users\\v-dazou\\Documents\\linux-3.16.1\\sound";
            // string pt = "C:\\Users\\v-dazou\\Documents\\sound";
            string pt = "D:\\work\\linux-3.18.10\\security";
            DirectoryWalker walker = new DirectoryWalker(pt);
            // for test.
            walker.setMaxDepth(1);
            List<string> files = walker.getAllfiles();
            Console.WriteLine(files.Count);

            int currentfile = 0;

            int useAntlr = 1;

            if (useAntlr == 1)
            {
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
                Console.WriteLine("Start binarization && compress chain");
                int countfunction = functionlist.Count();
                int curfunction = 0;
                foreach (FunctionNode fnode in functionlist)
                {
                    fnode.editAST();
                    Console.WriteLine("Processing: " + curfunction + " / " + countfunction);
                    curfunction++;
                    //fnode.structureblock();
                    //fnode.compresschain();
                    //fnode.binarization();
                }
                Console.WriteLine("Finish binarization && compress chain");

                Console.WriteLine("total functions: " + functionlist.Count);
                //Thread.Sleep(1000);
            }

            
            // ############### Start calculate PCFG & SourceASTs ################
            PCFG pCFG;
            SourceASTs sourceASTs;
            // calculate & write
            if (useAntlr == 1)
            {
                // PCFG
                Console.WriteLine("Start calculate PCFG, Press Enter to continue.");
                Console.ReadLine(); //Pause
                FunctionTreeVisitor funcvisitor = new FunctionTreeVisitor(functionlist);
                funcvisitor.countContextFreeGrammar();
                //funcvisitor.printGrammar();
                pCFG = funcvisitor.getPCFG();
                // pCFG.printGrammar();

                // Start Serialize PCFG
                serializationformatter = new BinaryFormatter();
                serializationstream = new FileStream("PCFG.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                serializationformatter.Serialize(serializationstream, pCFG);
                serializationstream.Close();
                Console.WriteLine("Finish calculate PCFG.");

                // SourceASTs
                Console.WriteLine("Start calculate SourceASTs, Press Enter to continue.");
                Console.ReadLine(); //Pause
                sourceASTs = new SourceASTs(funcvisitor);
                // Start Serialize SourceASTs
                serializationformatter = new BinaryFormatter();
                serializationstream = new FileStream("SourceASTs.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                serializationformatter.Serialize(serializationstream, sourceASTs);
                serializationstream.Close();
                Console.WriteLine("Finish calculate SourceASTs.");
            }
            // read from disk
            else
            {
                Console.WriteLine("Start read PCFG from PCFG.bin, Press Enter to continue.");
                Console.ReadLine(); //Pause
                serializationformatter = new BinaryFormatter();
                serializationstream = new FileStream("PCFG.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                pCFG = (PCFG)serializationformatter.Deserialize(serializationstream);
                serializationstream.Close();
                Console.WriteLine("Finish read PCFG.");

                Console.WriteLine("Start read SourceASTs from SourceASTs.bin, Press Enter to continue.");
                Console.ReadLine(); //Pause
                Console.WriteLine("Reading SourceASTs.....");
                serializationformatter = new BinaryFormatter();
                serializationstream = new FileStream("SourceASTs.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                sourceASTs = (SourceASTs)serializationformatter.Deserialize(serializationstream);
                serializationstream.Close();
                Console.WriteLine("Finish read SourceASTs.");
            }
            sourceASTs.outputXML();
            pCFG.outputPCFG("PCFG.txt");
            // ############### Finish calculate & SourceASTs ###############

            // ############### Start calculate PriorPTSG ###################
            int calculatePriorPTSG = 1;
            PriorPTSG pTSGprior;
            if (calculatePriorPTSG == 1 || useAntlr == 1)
            {
                Console.WriteLine("Start calculate PriorPTSG, Press Enter to continue.");
                Console.ReadLine(); //Pause
                pTSGprior = new PriorPTSG(pCFG);
                pTSGprior.generatePTSG();
                //pTSGprior.outputPTSG();

                // Start Serialize PriorPTSG
                serializationformatter = new BinaryFormatter();
                serializationstream = new FileStream("PriorPTSG.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                serializationformatter.Serialize(serializationstream, pTSGprior);
                serializationstream.Close();
                // Finish Serialize PriorPTSG 
                Console.WriteLine("Finish calculate PriorPTSG");
            }
            else
            {
                Console.WriteLine("Start read PriorPTSG from PriorPTSG.bin, Press Enter to continue.");
                Console.ReadLine(); //Pause
                serializationformatter = new BinaryFormatter();
                serializationstream = new FileStream("PriorPTSG.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                pTSGprior = (PriorPTSG)serializationformatter.Deserialize(serializationstream);
                serializationstream.Close();
                Console.WriteLine("Finish read PriorPTSG.");
            }
            pTSGprior.outputPTSG("PriorPTSG.txt");
            // ############### Finish calculate PriorPTSG ##################



            //TSG tmp = funcvisitor.getOneTSGRandomly();
            //Console.WriteLine(tmp.getSequence());

            Console.WriteLine("Start calculate PostPTSG, Press Enter to continue.");
            Console.ReadLine(); //Pause
            PostPTSG postPTSG = new PostPTSG(sourceASTs, pTSGprior);
            postPTSG.calculatePostPTSG();
            postPTSG.outputpostPTSG("PostPTSG.txt");
            postPTSG.outputXML();
            Console.WriteLine("Finish calculate PostPTSG");

            stw.Stop();
        }

    }


}
