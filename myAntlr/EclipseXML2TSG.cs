using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myAntlr
{
    class EclipseXML2TSG
    {
        string xmlpath;
        XmlDocument doc;

        public EclipseXML2TSG(string s)
        {
            xmlpath = s;
        }
        void initXML()
        {
            doc = new XmlDocument();
            doc.Load(xmlpath);
        }
        List<TSG> getMethods()
        {
            XmlNodeList nodes = doc.SelectNodes("//MethodDeclaration");
            List<TSG> methods = new List<TSG>();
            //Console.WriteLine(nodes.Count);
            foreach(XmlNode node in nodes)
            {
                TSG t = getTSGfromNode(node);
                methods.Add(t);
            }
            return methods;

        }
        TSG getTSGfromNode(XmlNode root)
        {
            TSG t = new TSG();
            XmlNodeList childnodes = root.ChildNodes;

            t.setName(root.Name);

            //Console.WriteLine("--------\n" + root.Name);
            //Console.WriteLine(root.InnerText);
            //Console.WriteLine(childnodes.Count);
            if (root.Name == "SimpleName")
            {
                string s = root.InnerText;
                //for (int i = 0; i < s.Length; i++)
                //{
                //    Console.WriteLine(i + ", acsii: " + (int)s[i] + "char:" + s[i]);
                //}
                s = s.Replace("\r\n", "");
                //Console.WriteLine(s);
                //Console.ReadLine();
                t.setCode(s);
            }

            foreach (XmlNode childnode in childnodes)
            {
                if (childnode.Name == "#text")
                    continue;
                TSG child = getTSGfromNode(childnode);
                t.addChild(child);
                child.setFather(t);
            }

            return t;
        }
        public List<TSG> getTSGs()
        {
            List<TSG> TSGs;
            initXML();
            TSGs = getMethods();

            return TSGs;
        }

    }
}
