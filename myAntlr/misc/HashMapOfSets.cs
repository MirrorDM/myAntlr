using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAntlr.misc
{
    public class HashMapOfSets
    {

        // public HashMap<Object, HashSet<Object>> hashMap = new HashMap<Object, HashSet<Object>>();
        public Dictionary<Object, HashSet<Object>> hashMap = new Dictionary<Object, HashSet<Object>>();

        // public Iterator<Entry<Object, HashSet<Object>>> getEntrySetIterator()
        public IEnumerator<KeyValuePair<Object, HashSet<Object>>> getEntrySetIterator()
        {
            // return hashMap.entrySet().iterator();
            return new HashSet<KeyValuePair<Object, HashSet<Object>>>(hashMap).GetEnumerator();
        }

        // public Iterator<Object> getKeySetIterator()
        public IEnumerator<Object> getKeySetIterator()
        {
            // return hashMap.keySet().iterator();
            return new HashSet<Object>(hashMap.Keys).GetEnumerator();
        }

        public void add(Object key, Object val)
        {
            //HashSet<Object> valList = hashMap.get(key);
            //if (valList == null)
            //{
            //    valList = new HashSet<Object>();
            //    hashMap.put(key, valList);
            //}
            //valList.add(val);
            HashSet<Object> valList;
            if (hashMap.ContainsKey(key))
            {
                valList = hashMap[key];
            }
            else
            {
                valList = new HashSet<Object>();
                hashMap.Add(key, valList);
            }
            valList.Add(val);
        }

        public void addHashMapOfSets(HashMapOfSets otherHashMap)
        {
            // Set<Entry<Object, HashSet<Object>>> entrySet = otherHashMap.hashMap.entrySet();
            HashSet<KeyValuePair<Object, HashSet<Object>>> entrySet = new HashSet<KeyValuePair<Object,HashSet<Object>>>(otherHashMap.hashMap);
            // Iterator<Entry<Object, HashSet<Object>>> it = entrySet.iterator();
            IEnumerator<KeyValuePair<Object, HashSet<Object>>> it = entrySet.GetEnumerator();
            //while (it.hasNext())
            while (it.MoveNext())
            {
                //Entry<Object, HashSet<Object>> pair = it.next();
                KeyValuePair<Object, HashSet<Object>> pair = it.Current;
                //Iterator<Object> it2 = pair.getValue().iterator();
                IEnumerator<Object> it2 = pair.Value.GetEnumerator();
                // while (it2.hasNext())
                while (it2.MoveNext())
                {
                    // add(pair.getKey(), it2.next());
                    add(pair.Key, it2.Current);
                }
            }
        }

        public void remove(Object key, Object val)
        {
            //HashSet<Object> dstList = hashMap.get(key);
            //if (dstList == null)
            //    return;
            //dstList.remove(val);
            if (hashMap.ContainsKey(key))
            {
                hashMap[key].Remove(val);
            }
            else
            {
                return;
            }
        }

        public void removeAllForKey(Object key)
        {
            // hashMap.put(key, new HashSet<Object>());
            hashMap.Remove(key);
            hashMap.Add(key, new HashSet<Object>());
        }

        public int size()
        {
            int s = 0;
            // Set<Entry<Object, HashSet<Object>>> entrySet = hashMap.entrySet();
            HashSet<KeyValuePair<Object, HashSet<Object>>> entrySet = new HashSet<KeyValuePair<Object, HashSet<Object>>>(hashMap);
            // Iterator<Entry<Object, HashSet<Object>>> it = entrySet.iterator();
            IEnumerator<KeyValuePair<Object, HashSet<Object>>> it = entrySet.GetEnumerator();
            // while (it.hasNext())
            while (it.MoveNext())
            {
                // Entry<Object, HashSet<Object>> pair = it.next();
                KeyValuePair<Object, HashSet<Object>> pair = it.Current;
                // s += pair.getValue().size();
                s += pair.Value.Count();
            }
            return s;
        }

        public HashSet<Object> getListForKey(Object k)
        {
            if (hashMap.ContainsKey(k))
            {
                return hashMap[k];
            }
            else
            {
                return null;
            }
        }

    }

}
