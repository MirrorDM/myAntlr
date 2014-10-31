using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAntlr
{
    public class PostPTSG
    {
        Dictionary<string, int> TSGcount = new Dictionary<string, int>();
        Dictionary<string, int> rootcount = new Dictionary<string, int>();
        Dictionary<string, double> p0;
        double alpha;
        FunctionTreeVisitor fvisitor;
        Random rand = new Random();

        const int getTSGtimes = 1000;
        const int iterationOfEachTSG = 100;

        public PostPTSG(FunctionTreeVisitor f, PriorPTSG prior)
        {
            fvisitor = f;
            p0 = prior.getPrior();
            alpha = prior.getalpha();
        }

        public void calculatePostPTSG()
        {
            for (int i = 0; i < getTSGtimes; i++)
            {
                Console.WriteLine(i + " / " + getTSGtimes + "Processing.");

                calculateOneTSG();
            }
        }

        public Dictionary<string, double> getPostPTSG()
        {
            Dictionary<string, double> finalpTSG = new Dictionary<string, double>();

            foreach (string seq in TSGcount.Keys)
            {
                double postP = postProbablity(seq);

                finalpTSG.Add(seq, postP);
            }

            foreach (var item in finalpTSG.OrderBy(i => i.Value))
            {
                Console.WriteLine(item.Key + item.Value);
            }

            Console.WriteLine("pTSG count" + finalpTSG.Count());

            return finalpTSG;
        }
        static string getStringRoot(string s)
        {
            int len = s.Length;
            string root = "";
            for (int i = 1; i < len; i++)
            {
                if (s[i] == '(' || s[i] == ')')
                {
                    root = s.Substring(1, i - 1);
                    break;
                }
            }
            return root;
        }
        void calculateOneTSG()
        {
            TSG currentTSG = fvisitor.getOneTSGRandomly();
            
            setInitialZ(currentTSG);

            // How many iteration?
            for (int i = 0; i < iterationOfEachTSG; i++)
            {
                gibbsSampler(currentTSG);
            }
        }

        void gibbsSampler(TSG t)
        {
            Queue<TSG> queue = new Queue<TSG>();
            queue.Enqueue(t);
            TSG cur;
            List<TSG> cur_children;
            while (queue.Count() > 0)
            {
                cur = queue.Dequeue();
                cur_children = cur.getChildren();
                foreach (TSG c in cur_children)
                {
                    queue.Enqueue(c);
                }

                // z0rate = p_post(join) / (p_post(join) + p_post(s) * p_post(t))
                double z0rate;
                double post_join, post_s, post_t;
                TSG fragmentroot = cur.getFragmentRoot();

                // set together.
                cur.setIsNewFragment(0);
                post_join = postProbability(fragmentroot);

                // set separate.
                cur.setIsNewFragment(1);
                post_t = postProbability(fragmentroot);
                post_s = postProbability(cur);

                z0rate = post_join / (post_join + post_s * post_t);

                double random01 = rand.NextDouble();

                // isNewFragment = 0.
                if (random01 < z0rate)
                {
                    cur.setIsNewFragment(0);
                    // update join
                    updateTSGcount(fragmentroot);
                    updateRootcount(fragmentroot.getName());
                }

                // isNewFragment = 1.
                else
                {
                    cur.setIsNewFragment(1);
                    // update t
                    updateTSGcount(fragmentroot);
                    updateRootcount(fragmentroot.getName());
                    // update s
                    updateTSGcount(cur);
                    updateRootcount(cur.getName());
                }
            }
        }
        void setInitialZ(TSG t)
        {
            Queue<TSG> queue = new Queue<TSG>();
            queue.Enqueue(t);
            TSG cur;
            List<TSG> cur_children;
            while (queue.Count() > 0)
            {
                cur = queue.Dequeue();

                cur.setIsNewFragment(0); // Set initial value.

                cur_children = cur.getChildren();
                foreach (TSG c in cur_children)
                {
                    queue.Enqueue(c);
                }
            }

        }
        double postProbability(TSG t)
        {
            string seq = t.getSequence();
            double res;
            res = postProbablity(seq);
            return res;
        }
        double postProbablity(string seq)
        {
            // res = (count(t) + alpha * p0(t)) / (count(root(t)) + alpha)
            double res;
            int count_t = 0;
            double p0_t = 0;
            string root_t = getStringRoot(seq);
            double count_root_t = 0;
            string t_seq = seq;
            if (TSGcount.ContainsKey(t_seq))
            {
                count_t = TSGcount[t_seq];
            }
            if (p0.ContainsKey(t_seq))
            {
                p0_t = p0[t_seq];
            }
            if (rootcount.ContainsKey(root_t))
            {
                count_root_t = rootcount[root_t];
            }

            res = (count_t + alpha * p0_t) / (count_root_t + alpha);
            return res;
        }
        void updateTSGcount(TSG t)
        {
            string seq = t.getSequence();
            if (TSGcount.ContainsKey(seq))
            {
                TSGcount[seq]++;
            }
            else
            {
                TSGcount.Add(seq, 1);
            }
        }
        void updateRootcount(string s)
        {
            if (rootcount.ContainsKey(s))
            {
                rootcount[s]++;
            }
            else
            {
                rootcount.Add(s, 1);
            }
        }
        //double z0Probablility(TSG join, TSG s, TSG t)
        //{
        //    // res = p_post(join) / (p_post(join) + p_post(s) * p_post(t))
        //    double res;
        //    double post_join, post_s, post_t;
        //    post_join = postProbability(join);
        //    post_s = postProbability(s);
        //    post_t = postProbability(t);

        //    res = post_join / (post_join + post_s * post_t);
        //    return res;
        //}
    }
}
