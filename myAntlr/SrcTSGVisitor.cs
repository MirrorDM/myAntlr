using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAntlr
{
    public class SrcTSGVisitor
    {
        PCFG pContextFreeGrammar = new PCFG();
        List<TSG> TSGList;
        Random randObj = new Random();

        public SrcTSGVisitor(List<TSG> ltsg)
        {
            TSGList = ltsg;
        }
        public void countContextFreeGrammar()
        {
            foreach (TSG node in TSGList)
            {
                if (node.getChildren().Count > 0)
                {
                    visitNode(node);
                }
            }
        }
        public PCFG getPCFG()
        {
            return pContextFreeGrammar;
        }
        public List<TSG> getAllTSG()
        {
            return TSGList;
        }
        public TSG getOneTSGRandomly()
        {
            int listlenth = TSGList.Count;
            //Console.WriteLine("list lenth:" + listlenth);
            int pos = randObj.Next(listlenth);
            //Console.WriteLine("random position:" + pos);
            return TSGList[pos];
        }
        void visitNode(TSG node)
        {
            List<string> grammar = new List<string>();
            string id = node.getName();
            grammar.Add(id);

            int count = node.getChildCount();
            for (int i = 0; i < count; i++)
            {
                TSG child = node.getChild(i);
                id = child.getName();

                grammar.Add(id);
                if (child.getChildCount() > 0)
                {
                    visitNode(child);
                }
            }

            // TODO: Should filter this?
            if (grammar.Count < 2)
            {
                return;
            }
            //
            pContextFreeGrammar.addGrammar(grammar);

        }
    }
}
