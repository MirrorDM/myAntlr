using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.cfg
{
    public abstract class Edge<V> 
    {

        private V destination;
        private V source;

        
        public Edge(V source, V destination) {
            this.source = source;
            this.destination = destination;
        }


        public virtual V getDestination() {
            return this.destination;
        }


        public virtual V getSource()
        {
            return this.source;
        }


        // public abstract Map<String, Object> getProperties();
        public abstract Dictionary<String, Object> getProperties();

        // @Override
        // public override int hashCode()
        public override int GetHashCode() 
        {
                const int prime = 31;
                int result = 1;
                // result = prime * result + ((destination == null) ? 0 : destination.hashCode());
                result = prime * result + ((destination == null) ? 0 : destination.GetHashCode());
                // result = prime * result + ((source == null) ? 0 : source.hashCode());
                result = prime * result + ((source == null) ? 0 : source.GetHashCode());
                return result;
        }


        // @Override
        // public override bool equals(Object obj) 
        public override bool Equals(Object obj) 
        {
            if (this == obj) {
                return true;
            }
            if (obj == null) {
                return false;
            }
        // if (!(obj instanceof Edge)) 
            if (!(obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().Equals(typeof(Edge<>))))
            {
                return false;
            }

            // -------- zdm added. Don't know.
            if (!(obj is Edge<CFGNode>))
                return false;
            // -------- zdm added. Don't know.
            // So obj is Edge<CFGNode>, Change the logic below.


                // Edge<?> other = (Edge<?>) obj;
            Edge<CFGNode> other = (Edge<CFGNode>) obj;
            if (destination == null) 
            {
                if (other.destination != null) 
                {
                    return false;
                }
            } 
            // else if (!destination.equals(other.destination)) 
            else if (!destination.Equals(other.destination)) 
            {
                    return false;
            }
            if (source == null) 
            {
                if (other.source != null) 
                {
                    return false;
                }
            }
            // else if (!source.equals(other.source)) 
            else if (!source.Equals(other.source))
            {
                return false;
            }
            return true;
        }

    }
}
