using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Atn;

using myAntlr.astwalking;
using myAntlr.astnodes;
using myAntlr.astnodes.statements;
using myAntlr.misc;

namespace myAntlr.parsing
{
    // abstract public class ANTLRParserDriver : Observable
    abstract public class ANTLRParserDriver : MyObservable
    {
        // TODO: This class does two things:
        // * It is a driver for the ANTLRParser, i.e., the parser
        //   that creates ParseTrees from Strings. It can also already
        //   'walk' the ParseTree to create ASTs.
        // * It is an AST provider in that it will notify watchers
        //   when ASTs are ready.
        // We should split this into two classes.
    
        public Stack<ASTNodeBuilder> builderStack = new Stack<ASTNodeBuilder>();
        public TokenSubStream stream;
        public String filename;
    
        private Parser antlrParser;
        // private ParseTreeListener listener;
        private IParseTreeListener listener;
        private CommonParserContext context = null;
    
        // abstract public ParseTree parseTokenStreamImpl(TokenSubStream tokens);
        abstract public IParseTree parseTokenStreamImpl(TokenSubStream tokens);
        // abstract public Lexer createLexer(ANTLRInputStream input);
        abstract public Lexer createLexer(AntlrInputStream input);
    
        public ANTLRParserDriver() : base()
        {
            // super();
        }   
    
        // public void parseAndWalkFile(String filename) throws ParserException
        public void parseAndWalkFile(String filename)
        {
            TokenSubStream stream = createTokenStreamFromFile(filename);
            initializeContextWithFile(filename, stream);
        
            // ParseTree tree = parseTokenStream(stream);  
            IParseTree tree = parseTokenStream(stream);      
            walkTree(tree);
        }
    
        // public void parseAndWalkTokenStream(TokenSubStream tokens) throws ParserException
        public void parseAndWalkTokenStream(TokenSubStream tokens)
        {
            filename = "";
            stream = tokens;
            // ParseTree tree = parseTokenStream(tokens);
            IParseTree tree = parseTokenStream(tokens);
            walkTree(tree);
        }
    
        // public ParseTree parseAndWalkString(String input) throws ParserException
        public IParseTree parseAndWalkString(String input)
        {
            // ParseTree tree = parseString(input);
            IParseTree tree = parseString(input);
            walkTree(tree);
            return tree;
        }
    
        // public ParseTree parseTokenStream(TokenSubStream tokens) throws ParserException
        public IParseTree parseTokenStream(TokenSubStream tokens)
        {
            // ParseTree returnTree = parseTokenStreamImpl(tokens);
            IParseTree returnTree = parseTokenStreamImpl(tokens);
            if(returnTree == null)
                throw new ParserException();
            return returnTree;
        }
    
        // public ParseTree parseString(String input) throws ParserException
        public IParseTree parseString(String input) 
        {
            // char[] charArray = input.toCharArray();
            char[] charArray = input.ToCharArray();
            // ANTLRInputStream inputStream = new ANTLRInputStream(charArray, charArray.length);
            AntlrInputStream inputStream = new AntlrInputStream(charArray, charArray.Length);
            Lexer lex = createLexer(inputStream);
            TokenSubStream tokens = new TokenSubStream(lex);
            // ParseTree tree = parseTokenStream(tokens);
            IParseTree tree = parseTokenStream(tokens);
            return tree;
        }
    
        // protected TokenSubStream createTokenStreamFromFile(String filename) throws ParserException
        protected TokenSubStream createTokenStreamFromFile(String filename)
        {
        
            // ANTLRInputStream input;
            AntlrInputStream input;
            try {
                //input = new ANTLRFileStream(filename);
                input = new AntlrFileStream(filename);
            } catch (IOException e) {
                throw new ParserException();
            }
        
            Lexer lexer = createLexer(input);
            TokenSubStream tokens = new TokenSubStream(lexer);
            return tokens;
        
        }
    
        // protected void walkTree(ParseTree tree)
        protected void walkTree(IParseTree tree)
        {
            ParseTreeWalker walker = new ParseTreeWalker();
            // walker.walk(getListener(), tree);
            walker.Walk(getListener(), tree);
        }
    
    
        protected void initializeContextWithFile(String filename, TokenSubStream stream)
        {
            setContext(new CommonParserContext());
            getContext().filename = filename;
            getContext().stream = stream;
            initializeContext(getContext());
        }
    
        // protected bool isRecognitionException(RuntimeException ex)
        protected bool isRecognitionException(SystemException ex)
        {
        
            //return ex.getClass() == ParseCancellationException.class &&
            //       ex.getCause() instanceof RecognitionException;
            return (ex.GetType() == typeof(ParseCanceledException)) && (ex.InnerException is RecognitionException);
        }

        protected void setLLStarMode(Parser parser)
        {
            // parser.removeErrorListeners();
            parser.RemoveErrorListeners();
               
            // parser.addErrorListener(ConsoleErrorListener.INSTANCE);

            // parser.setErrorHandler(new DefaultErrorStrategy());
            parser.ErrorHandler = new DefaultErrorStrategy();

            // parser.getInterpreter().setPredictionMode(PredictionMode.LL);
        }

        protected void setSLLMode(Parser parser)
        {
            // parser.getInterpreter().setPredictionMode(PredictionMode.SLL);
            // parser.removeErrorListeners();
            parser.RemoveErrorListeners();
            // parser.setErrorHandler(new BailErrorStrategy());
            parser.ErrorHandler = new BailErrorStrategy();
        }

        public void initializeContext(CommonParserContext context)
        {
            filename = context.filename;
            stream = context.stream;
        }
    
        public void setStack(Stack<ASTNodeBuilder> aStack)
        {
            builderStack = aStack;
        }
    
    
        ////////////////////
    
        public void begin()
        {
            notifyObserversOfBegin();
        }

        public void end()
        {
            notifyObserversOfEnd();
        }

        private void notifyObserversOfBegin()
        {
            ASTWalkerEvent myevent = new ASTWalkerEvent(ASTWalkerEvent.eventID.BEGIN);
            // setChanged();
            setChanged();
            notifyObservers(myevent);
        }

        private void notifyObserversOfEnd()
        {
            ASTWalkerEvent myevent = new ASTWalkerEvent(ASTWalkerEvent.eventID.END);
            setChanged();
            notifyObservers(myevent);
        }
    
        public void notifyObserversOfUnitStart(ParserRuleContext ctx)
        {
            ASTWalkerEvent myevent = new ASTWalkerEvent(ASTWalkerEvent.eventID.START_OF_UNIT);
            myevent.ctx = ctx;
            myevent.filename = filename;
            setChanged();
            notifyObservers(myevent);
        }
    
        public void notifyObserversOfUnitEnd(ParserRuleContext ctx)
        {
            ASTWalkerEvent myevent = new ASTWalkerEvent(ASTWalkerEvent.eventID.END_OF_UNIT);
            myevent.ctx = ctx;
            myevent.filename = filename;
            setChanged();
            notifyObservers(myevent);
        }
    
        public void notifyObserversOfItem(ASTNode aItem)
        {
            ASTWalkerEvent myevent = new ASTWalkerEvent(ASTWalkerEvent.eventID.PROCESS_ITEM);
            myevent.item = aItem;
            myevent.itemStack = builderStack;
            setChanged();
            notifyObservers(myevent);
        }
    
        public CompoundStatement getResult()
        {
            // return (CompoundStatement) builderStack.peek().getItem();
            return (CompoundStatement) builderStack.Peek().getItem();
        }

        public Parser getAntlrParser() {
            return antlrParser;
        }

        public void setAntlrParser(Parser aParser) {
            antlrParser = aParser;
        }

        // ParseTreeListener getListener() {
        public IParseTreeListener getListener() {
            return listener;
        }

        // public void setListener(ParseTreeListener listener)
        public void setListener(IParseTreeListener listener) {
            this.listener = listener;
        }

        public CommonParserContext getContext() {
            return context;
        }

        public void setContext(CommonParserContext context) {
            this.context = context;
        }

    }
}
