using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAntlr
{
    [Serializable]
    public class TSG
    {
        string name;
        int isNewFragment = 0;
        TSG father = null;
        List<TSG> children = new List<TSG>();

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
    }
}
