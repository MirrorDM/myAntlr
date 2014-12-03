using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using myAntlr.misc;

namespace myAntlr
{
    [Serializable]
    public class PriorPTSG
    {
        Dictionary<string, double> priorPTSG = new Dictionary<string, double>();
        PCFG pCFG;
        HashSet<string> nonTerminal;
        double expandrate = 0.7; // p$, for each nonternimal node, probability to expand.
        int totalsample = 1000; // how many times for Dirichlet process.
        int useDP = 0;       // Dirichlet Process or Maximum Likelihood
        double alpha = 0.01; // Beta(1, alpha) distribution.
        
        double[] u;  // array of random number, u_i ~ Beta(1, alpha).
        double[] pi; // pi_k = (1 - u_k) * u_(k-1) * u_(k-2) * ... * u_1
        Random rand = new Random();

        public PriorPTSG(PCFG cfg)
        {
            pCFG = cfg;
            nonTerminal = cfg.nonTerminals();
            u = new double[totalsample];
            pi = new double[totalsample];
        }

        public void generatePTSG()
        {
            // calculateDPparameters();

            // For test, test one root.
            //string s = nonTerminal.First();
            //generatePTSG(s);

            // For use, every root.
            Console.WriteLine("alpha = " + alpha + ", " + totalsample + " for each root.");
            int i = 0, nonterminalCount = nonTerminal.Count();
            foreach (string s in nonTerminal)
            {
                Console.WriteLine("Dirichlet Process for node: " + s + ", " + (i++) + " / " + nonterminalCount);
                calculateDPparameters();
                generatePTSG(s);
            }
        }
        public void generatePTSG(string root)
        {
            for (int i = 0; i < totalsample; i++)
            {
                TSG t = buildTSG(root, 1);
                string seq = t.getSequence();
                if (priorPTSG.ContainsKey(seq))
                {
                    priorPTSG[seq] += pi[i];
                }
                else
                {
                    priorPTSG.Add(seq, pi[i]);
                }
            }
        }
        public void calculateDPparameters()
        {
            // Dirichlet Process
            if (useDP == 1)
            {
                for (int i = 0; i < totalsample; i++)
                {
                    u[i] = SimpleRNG.GetBeta(1, alpha);
                    // First pi_i = u_(i-1) * ... * u_1
                    if (i == 0)
                        pi[i] = 1;
                    else
                        pi[i] = pi[i - 1] * u[i - 1];
                }
                for (int i = 0; i < totalsample; i++)
                {
                    // Second pi_i = (1-u_i) * u_(i-1) * ... * u_1
                    pi[i] = pi[i] * (1 - u[i]);
                }
            }
            // Maximum Likelihood
            else
            {
                for (int i = 0; i < totalsample; i++)
                {
                    pi[i] = (double)1 / totalsample;
                }
            }
        }
        public void outputPTSG(string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("expandrate: " + expandrate);
            sw.WriteLine("totalsample: " + totalsample);
            sw.WriteLine("alpha: " + alpha);
            if (useDP == 1)
            {
                sw.WriteLine("Dirichlet Process");
            }
            else
            {
                sw.WriteLine("Maximum Likelihood");
            }
            // foreach (var item in finalpTSG.OrderBy(i => i.Value))
            foreach (var item in priorPTSG.OrderBy(i => i.Value))
            {
                sw.WriteLine(item.Key + ": " + item.Value);
                Console.WriteLine(item.Key + ": " + item.Value);
            }
            sw.WriteLine("PriorTSG count" + priorPTSG.Count());
            Console.WriteLine("PriorTSG count" + priorPTSG.Count());
            sw.Close();
        }

        // mustexpand = 1 for outer call, mustexpand = 0 for recursive.
        // avoid single node.
        TSG buildTSG(string root, int mustexpand)
        {
            TSG currentNode = new TSG();
            currentNode.setName(root);
            List<TSG> tmpList = new List<TSG>();

            // expand rate.
            if (rand.NextDouble() < expandrate || mustexpand == 1)
            {
                List<string> expand = pCFG.getOneCFGfromRootRandomly(root);
                if (expand != null && expand.Count() > 1)
                {
                    TSG child;
                    string childName;

                    //NOTICE: expand[0] is root itself.
                    for (int i = 1; i < expand.Count(); i++)
                    {
                        childName = expand[i];
                        child = buildTSG(childName, 0);
                        //currentNode.addChild(child);
                        tmpList.Add(child);
                    }
                }
            }
            
            currentNode.setChildrenAndSetFather(tmpList);
            return currentNode;
        }

        // get prior distribution and alpha for PostPTSG.
        public Dictionary<string, double> getPrior()
        {
            return priorPTSG;
        }
        public double getalpha()
        {
            return alpha;
        }

    }
}
