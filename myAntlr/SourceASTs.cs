using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace myAntlr
{
    [Serializable]
    public class SourceASTs
    {
        List<TSG> TSGList = new List<TSG>();
        Random randObj = new Random();


        public SourceASTs(FunctionTreeVisitor fv)
        {
            TSGList = fv.getAllTSG();
        }

        public TSG getOneTSGRandomly()
        {
            int listlenth = TSGList.Count();
            int pos = randObj.Next(listlenth);
            return TSGList.ElementAt(pos);
        }

        public void outputXML()
        {
            int n = 0;
            foreach (TSG t in TSGList)
            {
                string path = "source\\" + n.ToString() + ".xml";
                StreamWriter sw = new StreamWriter(path);
                string stringxml = t.outputXML();
                sw.WriteLine(stringxml);
                sw.Close();
                n++;
            }
        }
    }
}
