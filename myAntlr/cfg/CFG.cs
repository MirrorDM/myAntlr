using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.cfg
{
    public class CFG 
    {
        // Vector<CFGNode> statements = new Vector<CFGNode>();
        List<CFGNode> statements = new List<CFGNode>();

        Edges<CFGEdge, CFGNode> edges = new Edges<CFGEdge, CFGNode>();
        SwitchLabels switchLabels = new SwitchLabels();

        // Vector<CFGNode> jumpStatements = new Vector<CFGNode>();
        List<CFGNode> jumpStatements = new List<CFGNode>();
        // HashMap<String, CFGNode> labels = new HashMap<String, CFGNode>();
        Dictionary<String, CFGNode> labels = new Dictionary<String, CFGNode>();

        // HashMap<CFGNode, CFGNode> loopStart = new HashMap<CFGNode, CFGNode>();
        public Dictionary<CFGNode, CFGNode> loopStart = new Dictionary<CFGNode, CFGNode>();


        public virtual void addCFG(CFG otherCFG) {
            //if (statements.size() == 0) {
            if (statements.Count() == 0) {
                replaceCFGBy(otherCFG);
                return;
            }

            //Vector<CFGNode> otherBlocks = otherCFG.getStatements();
            List<CFGNode> otherBlocks = otherCFG.getStatements();
            Edges<CFGEdge, CFGNode> otherEdges = otherCFG.getEdges();
            switchLabels.addAll(otherCFG.getSwitchLabels());
            // statements.addAll(otherBlocks);
            statements.AddRange(otherBlocks);
            edges.addEdges(otherEdges);

            // jumpStatements.addAll(otherCFG.getJumpStatements());
            jumpStatements.AddRange(otherCFG.getJumpStatements());
            
            // labels.putAll(otherCFG.getLabels());
            foreach (KeyValuePair<String, CFGNode> lable in otherCFG.getLabels())
            {
                labels.Add(lable.Key, lable.Value);
            }

            //loopStart.putAll(otherCFG.loopStart);
            foreach (KeyValuePair<CFGNode, CFGNode> lp in otherCFG.loopStart)
            {
                loopStart.Add(lp.Key, lp.Value);
            }

        }


        public virtual void replaceCFGBy(CFG otherCFG)
        {
            this.statements = otherCFG.statements;
            this.edges = otherCFG.edges;
            this.switchLabels = otherCFG.switchLabels;
            this.labels = otherCFG.labels;
            this.jumpStatements = otherCFG.jumpStatements;
            this.loopStart = otherCFG.loopStart;
        }


        public virtual CFGNode getBlockByLabel(String label)
        {
            // return labels.get(label);
            if (labels.ContainsKey(label))
                return labels[label];
            else
                return null;
        }


        public virtual void addSwitchLabel(CFGNode surroundingSwitch, CFGNode labeledBlock)
        {
            switchLabels.add(surroundingSwitch, labeledBlock);
        }


        public virtual CFGNode getOuterLoop(CFGNode thisStatement)
        {
            // return loopStart.get(thisStatement);
            if (loopStart.ContainsKey(thisStatement))
                return loopStart[thisStatement];
            else
                return null;

        }


        public virtual SwitchLabels getSwitchLabels()
        {
            return switchLabels;
        }


        public virtual CFGNode getLastStatement()
        {
            try 
            {
                // return statements.lastElement();
                return statements.Last();
            } 
            //catch (RuntimeException ex)
            catch (SystemException ex) 
            {
                return null;
            }
        }


        public virtual CFGNode getFirstStatement()
        {
            try 
            {
                // return statements.firstElement();
                return statements.First();
            } 
            // catch (RuntimeException ex)
            catch (SystemException ex) 
            {
                return null;
            }
        }


        public virtual void labelBlock(String label, CFGNode block)
        {
            // labels.put(label, block);
            labels.Add(label, block);
        }


        public virtual void addStatement(CFGNode newBlock)
        {
            // statements.add(newBlock);
            statements.Add(newBlock);
        }


        public virtual void addEdge(CFGNode srcBlock, CFGNode dstBlock)
        {
            addEdge(srcBlock, dstBlock, CFGEdge.EMPTY_LABEL);
        }


        public virtual void addEdge(CFGNode srcBlock, CFGNode dstBlock, String label)
        {
            CFGEdge edge = new CFGEdge(srcBlock, dstBlock, label);
            edges.addEdge(edge);
        }


        public virtual void removeAllEdgesFrom(CFGNode srcBlock)
        {
            edges.removeAllEdgesFrom(srcBlock);
        }


        public virtual List<CFGEdge> getAllEdgesFrom(CFGNode srcBlock)
        {
            return edges.getEdgesFrom(srcBlock);
        }
    
        // public boolean isConnected(CFGNode src, CFGNode dst)
        public virtual bool isConnected(CFGNode src, CFGNode dst)
        {
            return edges.isConnected(src, dst);
        }

        // public Iterator<CFGEdge> edgeIterator()
        public virtual IEnumerator<CFGEdge> edgeIterator()
        {
            return edges.GetEnumerator();
        }


        public virtual int getNumberOfStatements()
        {
            // return statements.size();
            return statements.Count();
        }


        public virtual int getNumberOfEdges()
        {
            return edges.totalSize();
        }


        // zdm change this. Don't know.
        // There are two Class extend from CFGNode, SwitchBlock and LoopBlock, and no Class extends from these two Class.
        // And there is no override function for SwitchBlock and LoopBlock, So it seems do the same thing like CFGNode.
        // public Collection<? extends CFGNode> getJumpStatements() {
        public virtual List<CFGNode> getJumpStatements()
        {
            return jumpStatements;
        }

        // public HashMap<String, CFGNode> getLabels()
        public virtual Dictionary<String, CFGNode> getLabels()
        {
            return labels;
        }


        private Edges<CFGEdge, CFGNode> getEdges() {
            return edges;
        }


        // public Vector<CFGNode> getStatements() {
        public virtual List<CFGNode> getStatements()
        {
            return statements;
        }


        public virtual void addJumpStatement(CFGNode block) 
        {
            // this.jumpStatements.add(block);
            this.jumpStatements.Add(block);
        }

    }

}
