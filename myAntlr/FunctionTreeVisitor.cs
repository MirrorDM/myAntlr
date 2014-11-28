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

using myAntlr.misc;

namespace myAntlr
{
    public class FunctionTreeVisitor
    {
        // Dictionary<List<string>, int> pContextFreeGrammar = new Dictionary<List<string>,int>(new ListComparer<string>());
        PCFG pContextFreeGrammar = new PCFG();
        List<FunctionNode> functiontreelist;
        Random randObj = new Random();

        public FunctionTreeVisitor(List<FunctionNode> nodelist)
        {
            functiontreelist = nodelist;
        }

        public void countContextFreeGrammar()
        {
            foreach (FunctionNode node in functiontreelist)
            {
                FunctionParser.StatementsContext root = node.functionstatements;
                if (root.ChildCount > 0)
                {
                    visitNode(root);
                }
            }
        }
        public PCFG getPCFG()
        {
            return pContextFreeGrammar;
        }
        public List<TSG> getAllTSG()
        {
            int listlenth = functiontreelist.Count();
            int pos = 0;
            List<TSG> TSGs = new List<TSG>();
            for (pos = 0; pos < listlenth; pos++)
            {
                FunctionNode curfunc = functiontreelist.ElementAt(pos);
                FunctionParser.StatementsContext root = curfunc.functionstatements;
                TSGs.Add(getTSGfromRoot(root));
                Console.WriteLine(pos + " / " + listlenth);
            }
            return TSGs;
        }
        public TSG getOneTSGRandomly()
        {
            int listlenth = functiontreelist.Count();
            //Console.WriteLine("list lenth:" + listlenth);
            int pos = randObj.Next(listlenth);
            //Console.WriteLine("random position:" + pos);
            FunctionNode randomFunc = functiontreelist.ElementAt(pos);
            FunctionParser.StatementsContext root = randomFunc.functionstatements;
            return getTSGfromRoot(root);
        }
        TSG getTSGfromRoot(IParseTree node)
        {
            TSG currentNode = new TSG();
            string id = node.GetType().ToString().Split('.').Last();
            id = id.Split('+').Last();
            currentNode.setName(id);

            //expand next layer.
            int count = node.ChildCount;
            List<TSG> tmpList = new List<TSG>();
            for (int i = 0; i < count; i++)
            {
                IParseTree child = node.GetChild(i);
                TSG childTSG;
                string childName = child.GetType().ToString().Split('.').Last();
                childName = childName.Split('+').Last();
                childTSG = getTSGfromRoot(child);
                tmpList.Add(childTSG);
            }
            currentNode.setChildrenAndSetFather(tmpList);
            return currentNode;
        }
        void visitNode(IParseTree node)
        {
            List<string> grammar = new List<string>();
            string id = node.GetType().ToString().Split('.').Last();
            id = id.Split('+').Last();

            //add for temp
            //id = node.GetType().ToString();

            grammar.Add(id);
            int count = node.ChildCount;
            for (int i = 0; i < count; i++)
            {
                IParseTree child = node.GetChild(i);
                id = child.GetType().ToString().Split('.').Last();
                id = id.Split('+').Last();

                //add for temp
                //id = child.GetType().ToString();

                grammar.Add(id);
                if (child.ChildCount > 0)
                {
                    visitNode(child);
                }
            }

            // TODO: Should filter this?
            if (grammar.Count < 3)
            {
                return;
            }
            //
            pContextFreeGrammar.addGrammar(grammar);
            
        }
        
    }
}
