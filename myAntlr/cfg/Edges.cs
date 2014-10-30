using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.misc;

// add by zdm. virtual/override handled
// No Class extends from this class.

namespace myAntlr.cfg
{
    // public class Edges<E extends Edge<V>, V> extends MultiHashMap<V, E> implements Iterable<E>
    public class Edges<E, V> : MultiHashMap<V, E>, IEnumerable<E> where E : Edge<V>
    {

        public void addEdge(E edge) {
            add(edge.getSource(), edge);
        }


        public List<E> getEdgesFrom(V src) {
            return get(src);
        }
    
        public List<V> outNeighborhood(V src) {
            // List<V> destinationList = new LinkedList<V>();
            List<V> destinationList = new LinkedList<V>().ToList<V>();
            // for (Edge<V> e : getEdgesFrom(src)) {
            foreach (Edge<V> e in getEdgesFrom(src)) {
                // destinationList.add(e.getDestination());
                destinationList.Add(e.getDestination());
        }
        return destinationList;
        }
    
        public bool isConnected(V src, V dst) {
            // for (Edge<V> e : getEdgesFrom(src))
            foreach (Edge<V> e in getEdgesFrom(src)) {
                // if (e.getDestination().equals(dst))
                if (e.getDestination().Equals(dst)) {
                    return true;
                }
            }
            return false;
        } 


        public void addEdges(Edges<E, V> otherEdges) {
            addAll(otherEdges);
        }


        public void removeEdge(V src, V dst) {
            List<E> edges = get(src);
            // Iterator<E> it = edges.iterator();
            //IEnumerator<E> it = edges.GetEnumerator();
            //E edge;
            //// while (it.hasNext())
            //while (it.MoveNext()) {
            //    // edge = it.next();
            //    edge = it.Current;
            //    // if (edge.getDestination() == dst) Don't know
            //    if (edge.getDestination().Equals(dst)) {
            //        it.remove();
            //    }
            //}

            foreach (E edge in edges)
            {
                if (edge.getDestination().Equals(dst))
                {
                    edges.Remove(edge);
                }
            }
        }


        public void removeAllEdgesFrom(V src) {
            removeAll(src);
        }


        // @Override
        // public Iterator<E> iterator() {
        public IEnumerator<E> GetEnumerator()
        {
        // TODO
            // List<E> list = new LinkedList<E>();
            List<E> list = new LinkedList<E>().ToList<E>();
            // for (V key : keySet())
            foreach (V key in keySet()) {
                //list.addAll(get(key));
                list.AddRange(get(key));
            }
            return list.GetEnumerator();
        }

        // add by zdm
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }

}
