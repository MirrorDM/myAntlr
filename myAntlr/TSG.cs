using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace myAntlr
{
    [Serializable]
    public class TSG
    {
        string name;
        int isNewFragment = 0;
        TSG father = null;
        int id = 0;
        string code = "";
        bool isCFGNode = false;

        Dictionary<string, int> codeTimes = new Dictionary<string, int>();
        int totalTimes = 0;

        List<TSG> children = new List<TSG>();

        public void updateCodeTimes(string simplename)
        {
            if (codeTimes.ContainsKey(simplename))
            {
                codeTimes[simplename]++;
            }
            else
            {
                codeTimes.Add(simplename, 1);
            }
            totalTimes++;
        }
        public void editTSG()
        {
            compresschain();
            binarization();
        }
        void compresschain()
        {
            checkIsChainFirstNode(this);
        }
        void checkIsChainFirstNode(TSG node)
        {
            if (node.getChildCount() == 1)
            {
                TSG lastnode = getChainLastNode(node);
                lastnode.setFather(node);
                List<TSG> newchildlist = new List<TSG>();
                newchildlist.Add(lastnode);
                node.setChildrenAndSetFather(newchildlist);
            }
            for (int i = 0; i < node.getChildCount(); i++)
            {
                checkIsChainFirstNode(node.getChild(i));
            }
        }
        TSG getChainLastNode(TSG node)
        {
            if (node.getChildCount() == 1)
            {
                return getChainLastNode(node.getChild(0));
            }
            return node;
        }

        void binarization()
        {
            binarizeNode(this);
        }
        void binarizeNode(TSG root)
        {
            if (root.getChildCount() > 2)
            {
                List<TSG> childlist = root.getChildren();
                List<TSG> newlist = new List<TSG>();
                newlist.Add(childlist[0]);
                TSG dummytreenode = new TSG();
                dummytreenode.setName("DummyTreeNode");
                childlist.RemoveAt(0);
                foreach (TSG t in childlist)
                {
                    dummytreenode.addChild(t);
                    t.setFather(dummytreenode);
                }
                newlist.Add(dummytreenode);
                root.setChildrenAndSetFather(newlist);
            }
            for (int i = 0; i < root.getChildCount(); i++)
            {
                binarizeNode(root.getChild(i));
            }
        }
        public TSG getChild(int i)
        {
            if (i >= children.Count)
                return null;
            return children[i];
        }
        public int getChildCount()
        {
            return children.Count;
        }
        public void addChild(TSG c)
        {
            children.Add(c);
        }

        public void setFather(TSG f)
        {
            father = f;
        }
        public TSG getFather()
        {
            return father;
        }
        public TSG getFragmentRoot()
        {
            TSG cur = this;
            while (true)
            {
                if (cur.getIsNewFragment() == 1 || cur.getFather() == null)
                    return this;
                cur = cur.getFather();
            }
        }
        public void setisCFGNode(bool bl)
        {
            isCFGNode = bl;
        }
        public bool getisCFGNode()
        {
            return isCFGNode;
        }

        public void setCode(string s)
        {
            code = s;
        }
        public string getCode()
        {
            return code;
        }
        public void setID(int i)
        {
            id = i;
        }
        public int getID()
        {
            return id;
        }
        public int getIsNewFragment()
        {
            return isNewFragment;
        }
        public void setIsNewFragment(int z)
        {
            isNewFragment = z;
        }
        public string getName()
        {
            return name;
        }
        public void setName(string s)
        {
            name = s;
        }
        public void setChildrenAndSetFather(List<TSG> cs)
        {
            children = cs;
            foreach (TSG child in children)
            {
                child.setFather(this);
            }
        }
        public List<TSG> getChildren()
        {
            return children;
        }
        //public void addChild(TSG c)
        //{
        //    children.Add(c);
        //}

        // Get whole TSG. Leaf nodes are 
        // terminal nodes
        public string getWholeSequence()
        {
            string s = "(" + name;
            foreach (TSG child in children)
            {
                s = s + child.getSequence();
            }
            s = s + ")";
            return s;
        }

        // Get Fragment of TSG. Leaf nodes 
        // are terminal nodes and those whose 
        // 'isNewFragment' equals 1.
        public string getSequence()
        {
            string s = "(" + name;
            if (isNewFragment == 0)
            {
                foreach (TSG child in children)
                {
                    s = s + child.getSequence();
                }
            }
            s = s + ")";
            return s;
        }
        public int getSize()
        {
            int size = 1;
            if (isNewFragment == 0)
            {
                foreach (TSG child in children)
                {
                    size = size + child.getSize();
                }
            }
            return size;
        }
        public override bool Equals(object obj)
        {
            if (obj is TSG)
            {
                return getSequence() == ((TSG)obj).getSequence();
            }
            return false;
        }
        public override int GetHashCode()
        {
            string s = getSequence();
            return s.GetHashCode();
        }
        public string outputXML()
        {
            string s = "";
            s = s + "<" + name + ">\n";
            //foreach (string simplename in codeTimes.Keys)
            foreach (var item in codeTimes.OrderBy(i => i.Value))
            {
                s = s + item.Key + ": " + ((double)item.Value / totalTimes) + '\n';
            }
            foreach (TSG child in children)
            {
                s = s + child.outputXML();
            }
            s = s + "</" + name + ">\n";
            return s;
        }
    }
}
