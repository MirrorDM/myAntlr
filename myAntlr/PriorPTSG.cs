using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.misc;

namespace myAntlr
{
    public class PriorPTSG
    {
        Dictionary<string, double> priorPTSG = new Dictionary<string, double>();
        PCFG pCFG;
        HashSet<string> nonTerminal;
        double expandrate = 0.9; // p$, for each nonternimal node, probability to expand.
        int totalsample = 1000; // how many times for Dirichlet process.
        
        double alpha = 0.2; // Beta(1, alpha) distribution.
        
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
            calculateDPparameters();

            // For test, test one root.
            //string s = nonTerminal.First();
            //generatePTSG(s);

            // For use, every root.
            foreach (string s in nonTerminal)
            {
                generatePTSG(s);
            }
        }
        public void generatePTSG(string root)
        {
            for (int i = 0; i < totalsample; i++)
            {
                TSG t = buildTSG(root);
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
        public void outputPTSG()
        {
            foreach (string seq in priorPTSG.Keys)
            {
                if (priorPTSG[seq] > 0.1)
                    Console.WriteLine(seq + ": " + priorPTSG[seq]);
            }
        }
        TSG buildTSG(string root)
        {
            TSG currentNode = new TSG();
            currentNode.setName(root);
            List<TSG> tmpList = new List<TSG>();

            // expand rate.
            if (rand.NextDouble() < expandrate)
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
                        child = buildTSG(childName);
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
