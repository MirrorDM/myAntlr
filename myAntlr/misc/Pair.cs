using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAntlr.misc
{
    public class Pair<L, R>
    {
        private L l;
        private R r;
        public Pair(L l, R r)
        {
            this.l = l;
            this.r = r;
        }
        public L getL() { return l; }
        public R getR() { return r; }
        public void setL(L l) { this.l = l; }
        public void setR(R r) { this.r = r; }
    }

}
