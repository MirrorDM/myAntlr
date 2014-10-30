using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.cfg
{
    public class CFGEdge : Edge<CFGNode> {

        public static readonly String EMPTY_LABEL = "";
        public static readonly String TRUE_LABEL = "True";
        public static readonly String FALSE_LABEL = "False";

        private String label;
        // private Map<String, Object> properties;
        private Dictionary<String, Object> properties;

        public CFGEdge(CFGNode source, CFGNode destination, String label) : base(source, destination)
        {
	        // super(source, destination);
	        this.label = label;
        }


        // @Override
        // public Map<String, Object> getProperties() 
        public override Dictionary<String, Object> getProperties() 
        {
	        if (this.properties == null) {
	            // this.properties = new HashMap<String, Object>();
                this.properties = new Dictionary<String,Object>();
	            this.properties.Add("flowLabel", label);
	        }
	        return this.properties;
        }


        // @Override
        // public override int hashCode() 
        public override int GetHashCode() 
        {
	        // final int prime = 31;
            const int prime = 31;
            // int result = super.hashCode();
            int result = base.GetHashCode();
            // result = prime * result + ((label == null) ? 0 : label.hashCode());
	        result = prime * result + ((label == null) ? 0 : label.GetHashCode());
	        return result;
        }


        // @Override
        // public override bool equals(Object obj) 
        public override bool Equals(Object obj) 
        {
	        if (this == obj) 
            {
	            return true;
	        }
            // if (!super.equals(obj)) 
            if (!base.Equals(obj)) 
            {
	            return false;
	        }
            // if (!(obj instanceof CFGEdge)) 
	        if (!(obj is CFGEdge))
            {
	            return false;
	        }
	        CFGEdge other = (CFGEdge) obj;
            if (label == null)
            {
                if (other.label != null)
                {
                    return false;
                }
            }

            // else if (!label.equals(other.label)) 
            else if (!label.Equals(other.label))
            {
                return false;
            }
	        return true;
        }

    }

}
