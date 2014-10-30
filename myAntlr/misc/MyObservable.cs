using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.misc
{
    public class MyObservable
    {
        HashSet<MyObserver> observers = new HashSet<MyObserver>();
        bool changed = false;

        public virtual void addObserver(MyObserver o) {
            if (o == null)
                return;
            if (observers.Contains(o))
                return;
            observers.Add(o);
        }

        public virtual void notifyObservers() 
        {
            if (hasChanged())
            {
                foreach (MyObserver observer in observers)
                {
                    observer.update(this, null);
                }
                clearChanged();
            }
        }

        public virtual void notifyObservers(Object arg) 
        {
            if (hasChanged())
            {
                foreach (MyObserver observer in observers)
                {
                    observer.update(this, arg);
                }
                clearChanged();
            }
        }

        protected virtual void setChanged()
        {
            changed = true;
        }
        public virtual bool hasChanged()
        {
            return changed;
        }
        protected virtual void clearChanged()
        {
            changed = false;
        }
    }
}
