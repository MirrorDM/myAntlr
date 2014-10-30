using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAntlr.misc
{
    public class MultiHashMap<K, V> 
    {

        // private HashMap<K, List<V>> hashMap = new HashMap<K, List<V>>();
        private Dictionary<K, List<V>> hashMap = new Dictionary<K, List<V>>();

        //public Set<Entry<K, List<V>>> entrySet() 
        public HashSet<KeyValuePair<K, List<V>>> entrySet() 
        {
	        // return hashMap.entrySet();
            return new HashSet<KeyValuePair<K, List<V>>>(hashMap);
        }


        public HashSet<K> keySet() {
	        // return hashMap.keySet();
            return new HashSet<K>(hashMap.Keys);
        }


        public bool containsKey(K key) {
            // add by zdm. C# doesn't allow "null"
            if (key == null)
                return false;
            // return hashMap.containsKey(key);
            return hashMap.ContainsKey(key);
        }

        // public Iterator<Entry<K, List<V>>> getEntrySetIterator() 
        public IEnumerator<KeyValuePair<K, List<V>>> getEntrySetIterator() 
        {
	        // return entrySet().iterator();
            return entrySet().GetEnumerator();
        }

        // public Iterator<K> getKeySetIterator()
        public IEnumerator<K> getKeySetIterator() 
        {
	        // return keySet().iterator();
            return keySet().GetEnumerator();
        }


        /**
         * Associates the specified value with the specified key. If the map
         * previously contained a mapping for the key, the new value is appended.
         * 
         * @param key
         *            key with which the specified value is to be associated
         * 
         * @param values
         *            value to be associated with the specified key
         */
        public void add(K key, V value) {
	        if (containsKey(key)) {
	            // hashMap.get(key).add(value);
                hashMap[key].Add(value);
	        } 
            else {
	            // List<V> list = new LinkedList<V>();
                List<V> list = new LinkedList<V>().ToList<V>();
	            // list.add(value);
                list.Add(value);
                // hashMap.put(key, list);
                try
                {
                    hashMap.Add(key, list);
                }
                catch (ArgumentNullException)
                {
                    return;
                }
	        }
        }


        /**
         * Associates the specified values with the specified key. If the map
         * previously contained a mapping for the key, the new values are appended.
         * 
         * @param key
         *            key with which the specified values are to be associated
         * 
         * @param values
         *            values to be associated with the specified key
         */
        public void addAll(K key, List<V> values) {
	        if (containsKey(key)) {
                // hashMap.get(key).addAll(values);
                hashMap[key].AddRange(values);
	        } else {
                // hashMap.put(key, values);
                hashMap.Add(key, values);
	        }
        }


        public void addAll(MultiHashMap<K, V> otherMap) {
	        // for (Entry<K, List<V>> entry : otherMap.entrySet()) 
            foreach (KeyValuePair<K, List<V>> entry in otherMap.entrySet())
            {
	            // addAll(entry.getKey(), entry.getValue());
                addAll(entry.Key, entry.Value);
	        }
        }


        // @Deprecated
        /**
         * @deprecated
         * Use addAll instead.
         */
        [Obsolete("Use addAll instead.", false)]
        public void addMultiHashMap(MultiHashMap<K, V> otherHashMap) {
	        // Set<Entry<K, List<V>>> entrySet = otherHashMap.entrySet();
            HashSet<KeyValuePair<K, List<V>>> entrySet = otherHashMap.entrySet();
	        // Iterator<Entry<K, List<V>>> it = entrySet.iterator();
            IEnumerator<KeyValuePair<K, List<V>>> it = entrySet.GetEnumerator();
	        // while (it.hasNext())
            while (it.MoveNext())
            {
	            // Entry<K, List<V>> pair = it.next();
                KeyValuePair<K, List<V>> pair = it.Current;
	            // Iterator<V> it2 = pair.getValue().iterator();
                IEnumerator<V> it2 = pair.Value.GetEnumerator();
	            // while (it2.hasNext())
                while (it2.MoveNext())
                {
		            // add(pair.getKey(), it2.next());
                    add(pair.Key, it2.Current);
	            }
	        }
        }


        /**
         * Removes the value from list associated with the specified key if it
         * exists.
         * 
         * @param key
         * @param value
         * @return true if the map was modified otherwise false.
         */
        public bool remove(K key, V value) {
	        if (containsKey(key)) {
	            // return get(key).remove(value);
                return get(key).Remove(value);
	        }
	        return false;
	    // List<V> dstList = hashMap.get(key);
	    // if (dstList == null)
	    // return false;
	    // dstList.remove(val);
	    // return true;
        }


        // @Deprecated
        /**
         * @deprecated
         * Use removeAll instead.
         */
        [Obsolete("Use removeAll instead.", false)]
        public void removeAllForKey(K key) {
	        // hashMap.put(key, new LinkedList<V>());
            hashMap.Remove(key);
            hashMap.Add(key, new LinkedList<V>().ToList<V>());
        }


        /**
         * Removes the mapping for the specified key from this map if present.
         * 
         * @param key
         *            key whose mapping is to be removed from the map
         * @return the previous value associated with key, or null if there was no
         *         mapping for key.
         */
        public List<V> removeAll(K key) {
	        // return hashMap.remove(key);
            if (hashMap.ContainsKey(key))
            {
                List<V> rem = hashMap[key];
                hashMap.Remove(key);
                return rem;
            }
            else
            {
                return null;
            }
        }


        // @Deprecated
        /**
         * @deprecated
         * Use totalSize instead.
         */
        // this could return hashMap.size()
        [Obsolete("Use totalSize instead.", false)]
        public int size() {
	        int s = 0;
            // Set<Entry<K, List<V>>> entrySet = hashMap.entrySet();
	        HashSet<KeyValuePair<K, List<V>>> entrySet = new HashSet<KeyValuePair<K,List<V>>>(hashMap);
	        // Iterator<Entry<K, List<V>>> it = entrySet.iterator();
            IEnumerator<KeyValuePair<K, List<V>>> it = entrySet.GetEnumerator();
	        // while (it.hasNext()) 
            while (it.MoveNext()) 
            {
	            // Entry<K, List<V>> pair = it.next();
                KeyValuePair<K, List<V>> pair = it.Current;
	            // s += pair.getValue().size();
                s += pair.Value.Count();
	    }
	    return s;
        }


        /**
         * Calculates the total count of all values in this map.
         * 
         * @return the total count of all values.
         */
        public int totalSize() {
	        int size = 0;
            // for (List<V> values : hashMap.values()) 
	        foreach (List<V> values in hashMap.Values) 
            {
                // size += values.size();
	            size += values.Count();
	        }
	        return size;
        }


        //@Deprecated
        /**
         * @deprecated
         * Use get instead.
         */
        [Obsolete("Use get instead.", false)]
        public List<V> getListForKey(K k) {
            // return hashMap.get(key);
            if (hashMap.ContainsKey(k))
            {
                return hashMap[k];
            }
            else
            {
                return null;
            }
        }


        /**
         * Returns the value to which the specified key is mapped, or null if this
         * map contains no mapping for the key.
         * 
         * @param key
         *            the key whose associated value is to be returned
         * @return the value to which the specified key is mapped, or null if this
         *         map contains no mapping for the key
         */
        public List<V> get(K key) {
            // return hashMap.get(key);
            if (hashMap.ContainsKey(key))
	        {
                return hashMap[key];
            }
            else
            {
                return null;
            }
        }

    }

}
