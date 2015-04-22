using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myAntlr
{
    class XML2TSG
    {
        string xmlpath;
        XmlDocument doc;
        Dictionary<int, TSG> num_node = new Dictionary<int, TSG>();
        Dictionary<string, int> id_num = new Dictionary<string, int>();
        public XML2TSG(string s)
        {
            xmlpath = s;

        }
        void initXML()
        {
            doc = new XmlDocument();
            doc.Load(xmlpath);
        }
        void getNodes()
        {
            XmlNodeList nodes = doc.SelectNodes("//node");
            //Console.WriteLine("nodes: " + nodes.Count);

            //<node id="N_0">
            //<attr name="name">
            //    <string>84562</string>
            //</attr>
            //<attr name="label">
            //    <string>childNum:0,code:,type:CompoundStatement,functionId:84556</string>
            //</attr>
            //</node>
            foreach (XmlNode node in nodes)
            {
                XmlElement xe = (XmlElement)node;
                //id = "N_0"
                string id = xe.GetAttribute("id");
                //Console.WriteLine(id);

                //num = 84562
                XmlNode numNode = node.SelectSingleNode("attr[@name='name']/string");
                int num;
                bool res = Int32.TryParse(numNode.InnerXml, out num);
                if (res == false)
                    Console.WriteLine("XML2TSG.cs, getNodes(), str2int error.");

                //childNum:0,code:,type:CompoundStatement,functionId:84556
                XmlNode labelNode = node.SelectSingleNode("attr[@name='label']/string");
                string labelLine = labelNode.InnerXml;
                //Console.WriteLine(labelLine);
                string[] labels = labelLine.Split(',');
                string type = "";
                string code = "";
                bool isCFGNode = false;
                foreach (string label in labels)
                {
                    //Console.WriteLine(label);
                    if (label.IndexOf("type:") >= 0)
                    {
                        type = label.Split(':')[1];
                        continue;
                    }
                    if (label.IndexOf("code:") >= 0)
                    {
                        code = label.Split(':')[1];
                        continue;
                    }
                    if (label.IndexOf("isCFGNode:True") >= 0)
                    {
                        isCFGNode = true;
                        continue;
                    }
                }
                //Console.WriteLine("type: " + type);
                //Console.WriteLine("code: " + code);
                //Console.WriteLine("isCFGNode: " + isCFGNode);

                TSG TSGNode = new TSG();
                TSGNode.setName(type);
                TSGNode.setCode(code);
                TSGNode.setID(num);
                TSGNode.setisCFGNode(isCFGNode);
                num_node.Add(num, TSGNode);
                id_num.Add(id, num);

                //Console.WriteLine(num);

            }
        }

        void getEdgesAndBuildTSG()
        {
            XmlNodeList edges = doc.SelectNodes("//edge");
            //Console.WriteLine("edges: " + edges.Count);
            //<edge from="N_0" to="N_1" isdirected="false" id="N_0--N_1">
            //</edge>
            foreach (XmlNode edge in edges)
            {
                XmlElement xe = (XmlElement)edge;
                string fr = xe.GetAttribute("from");
                string to = xe.GetAttribute("to");
                //Console.WriteLine(fr + " -> " + to);
                int fathernum = id_num[fr];
                TSG father = num_node[fathernum];
                int sonnum = id_num[to];
                TSG son = num_node[sonnum];

                father.addChild(son);
                son.setFather(father);
            }

        }

        public TSG getTSG()
        {
            initXML();
            getNodes();
            getEdgesAndBuildTSG();
            int rootnum = id_num["N_0"];
            TSG root = num_node[rootnum];
            return root;
        }

    }
}
