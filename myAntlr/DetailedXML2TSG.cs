using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myAntlr
{
    class DetailedXML2TSG
    {
        string xmlpath;
        XmlDocument doc;
        public DetailedXML2TSG(string s)
        {
            xmlpath = s;
        }
        void initXML()
        {
            doc = new XmlDocument();
            try
            {
                doc.Load(xmlpath);
            }
            catch (XmlException xe)
            {
                Console.Error.WriteLine(xmlpath);
                Console.Error.WriteLine(xe);
            }
        }
        List<TSG> getMethods()
        {
            XmlNodeList nodes = doc.SelectNodes("//MethodDeclaration");
            List<TSG> methods = new List<TSG>();
            //Console.WriteLine(nodes.Count);
            foreach (XmlNode node in nodes)
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

            if (root.Name == "SimpleName")
            {
                string s = root.Attributes["SourceCode"].Value;
                //Console.WriteLine("source code: " + s);
                //Console.WriteLine(s);
                //Console.ReadLine();
                //t.setCode(s);
            }
            if (root.Name == "MethodInvocation")
            {
                if (root.Attributes["MethodBinding"] != null)
                {
                    string binding = root.Attributes["MethodBinding"].Value;
                    Console.WriteLine("methodbinding: " + binding);
                    if (binding.Contains("java") == true)
                    {
                        t.setName(root.Name + binding);
                    }
                }
            }

            foreach (XmlNode childnode in childnodes)
            {
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
