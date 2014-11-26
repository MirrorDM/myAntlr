using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.misc;

namespace myAntlr
{
    [Serializable]
    public class PCFG
    {
        Dictionary<List<string>, int> pCFGlist = new Dictionary<List<string>, int>(new ListComparer<string>());
        Random randObj = new Random();

        public HashSet<string> nonTerminals()
        {
            HashSet<string> nt = new HashSet<string>();
            foreach (List<string> cfg in pCFGlist.Keys)
            {
                if (cfg.Count() > 1)
                {
                    if (nt.Contains(cfg[0]) == false)
                    {
                        nt.Add(cfg[0]);
                    }
                }
            }
            return nt;
        }
        public void addGrammar(List<string> grammar)
        {
            if (pCFGlist.ContainsKey(grammar))
            {
                pCFGlist[grammar] = pCFGlist[grammar] + 1;
            }
            else
            {
                pCFGlist.Add(grammar, 1);
            }
        }
        public List<string> getOneCFGfromRootRandomly(string s)
        {
            if (!canExpandfrom(s))
            {
                return new List<string>();
            }
            Dictionary<List<string>, double> CFGwithProb = getCFGfromRoot(s);
            double p = randObj.NextDouble();
            double tmpsum = 0;
            foreach (List<string> cfg in CFGwithProb.Keys)
            {
                tmpsum += CFGwithProb[cfg];
                if (tmpsum >= p)
                {
                    return cfg;
                }
            }
            // never reach
            Console.Error.WriteLine("PCFG.getOneCFGfromRootRandomly" + " Wrong prob");
            return CFGwithProb.Last().Key;
        }
        Dictionary<List<string>, double> getCFGfromRoot(string s)
        {
            Dictionary<List<string>, double> CFGwithProb = new Dictionary<List<string>, double>(new ListComparer<string>());
            HashSet<List<string>> CFGwithRoot_s = new HashSet<List<string>>(new ListComparer<string>());
            int totalCount = 0;
            foreach (List<string> cfg in pCFGlist.Keys)
            {
                if (cfg.Count() > 1)
                {
                    if (s == cfg.ElementAt(0))
                    {
                        totalCount += pCFGlist[cfg];
                        CFGwithRoot_s.Add(cfg);
                    }
                }
            }
            if (totalCount == 0)
            {
                Console.WriteLine("Should check \"PCFG.canExpandfrom\" before use \"PCFG.getCFGfromRoot\"");
                return new Dictionary<List<string>, double>(new ListComparer<string>());
            }
            foreach (List<string> cfg in CFGwithRoot_s)
            {
                CFGwithProb.Add(cfg, (double)pCFGlist[cfg] / totalCount);
            }
            return CFGwithProb;
        }
        public bool canExpandfrom(string s)
        {
            foreach (List<string> cfg in pCFGlist.Keys)
            {
                if (cfg.Count() > 1)
                {
                    if (s == cfg.ElementAt(0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void printGrammar()
        {
            foreach (KeyValuePair<List<string>, int> grammarcount in pCFGlist.OrderBy(i => i.Value))
            {
                for (int i = 0; i < grammarcount.Key.Count; i++)
                {
                    Console.Write(grammarcount.Key[i] + " ");
                }
                Console.WriteLine("\nFrequency: " + grammarcount.Value + "\n----------");
            }
            Console.WriteLine("Total grammar: " + pCFGlist.Count());
        }

    }
}
