using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add by zdm. virtual/override handled

namespace myAntlr.misc
{
    // Write by zdm.
    // Don't know.
    // imitation of Java's Observer
    public class MyObserver
    {
        public virtual void update(MyObservable obj, Object arg) { }
    }
}
