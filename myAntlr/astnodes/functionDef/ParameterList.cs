using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using myAntlr.astnodes.expressions;
using myAntlr.astwalking;

// add by zdm. virtual/override handled

namespace myAntlr.astnodes.functionDef
{
    public class ParameterList : ASTNode
    {
	
	    // TODO: we don't want to give back a reference to the list,
	    // we need to provide iterators for type and name
	
	    public virtual LinkedList<Parameter> getParameters()
	    {
		    return parameters;
	    }

        public virtual void addParameter(Parameter aParam)
	    {
		    // parameters.add(aParam);
            parameters.AddLast(aParam);
		    this.addChild(aParam);
	    }

        public virtual Identifier[] getNames()
	    {
		    // Identifier retNames [] = new Identifier[parameters.size()];
            Identifier [] retNames = new Identifier[parameters.Count()];
		    // for(int i = 0; i < parameters.size(); i++){
            for(int i = 0; i < parameters.Count(); i++){
			    // retNames[i] = parameters.get(i).name;
                retNames[i] = parameters.ElementAt(i).name;
		    }
		    return retNames;
	    }

        public virtual String[] getNameStrings()
	    {
		    // String retStrings [] = new String[parameters.size()];
            String [] retStrings = new String[parameters.Count()];
            // for(int i = 0; i < parameters.size(); i++){
		    for(int i = 0; i < parameters.Count(); i++){
			    // retStrings[i] = parameters.get(i).name.getEscapedCodeStr();
                retStrings[i] = parameters.ElementAt(i).name.getEscapedCodeStr();
		    }
		    return retStrings;
	    }

        public virtual ParameterType[] getTypes()
	    {
		    // ParameterType retTypes [] = new ParameterType[parameters.size()];
            ParameterType [] retTypes = new ParameterType[parameters.Count()];
		    // for(int i = 0; i < parameters.size(); i++){
            for(int i = 0; i < parameters.Count(); i++){
			    // retTypes[i] = parameters.get(i).type;
                retTypes[i] = parameters.ElementAt(i).type;
		    }
		    return retTypes;
	    }

        public virtual String[] getTypeStrings()
	    {
            // String retStrings [] = new String[parameters.size()];
		    String [] retStrings = new String[parameters.Count()];
            // for(int i = 0; i < parameters.size(); i++){
		    for(int i = 0; i < parameters.Count(); i++){
                // retStrings[i] = parameters.get(i).type.getEscapedCodeStr();
			    retStrings[i] = parameters.ElementAt(i).type.getEscapedCodeStr();
		    }
		    return retStrings;
	    }
	
	    private LinkedList<Parameter> parameters = new LinkedList<Parameter>();

	
	    // @Override
	    public override String getEscapedCodeStr()
	    {
		    if(codeStr != null)
			    return codeStr;
		
		    // if(parameters.size() == 0){
            if (parameters.Count() == 0)
            {
			    codeStr = "";
			    return codeStr;
		    }
		
            //Iterator<Parameter> i = parameters.iterator();
            //StringBuilder s = new StringBuilder();
            //for(; i.hasNext();){
            //    Parameter param = i.next();
            //    s.append(param.getEscapedCodeStr() + " , ");
            //}
		    IEnumerator<Parameter> i = parameters.GetEnumerator();
		    StringBuilder s = new StringBuilder();
		    for(; i.MoveNext();){
			    Parameter param = i.Current;
			    s.Append(param.getEscapedCodeStr() + " , ");
		    }
            // codeStr = s.toString();
		    codeStr = s.ToString();
            //codeStr = codeStr.substring(0, s.length() - 3);
		    codeStr = codeStr.Substring(0, s.Length - 3);
		
		    return codeStr;
	    }

	    // @Override
	    public override void accept(ASTNodeVisitor visitor){ visitor.visit(this); }
    }

}
