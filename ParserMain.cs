using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Analyzer
{

    public partial class Parser
    {
        public List<Token> Tokens;
        public int currentindex;
        public Token CurrentToken;
        public List<string> Errors;
        public List<Token> Leftderive;
        public List<string> Leafterivestring;
        public string currentstring="";
        public AstNode<string> root;
        public AstNode<string> currentNode;
        public Stack<AstNode<string>> stack=new Stack<AstNode<string>>();
        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            currentindex = 0;
            CurrentToken = Tokens[currentindex];
            Errors = new List<string>();
            Leftderive= new List<Token>();
            Leafterivestring= new List<string>();
        }
        public void addsubtree(AstNode<string> node)
        {
            var temp = node;
            //temp.AddChild(new leaf<string>(CurrentToken.Code.GetTerminal(), CurrentToken.Lexeme));
            currentNode.AddChild(temp);
            stack.Push(temp);
            currentNode = temp;
        }
        public void addleaf() {
            currentNode.AddChild(new leaf<string>(CurrentToken.Code.GetTerminal(), CurrentToken.Lexeme));
        }
        public void Poptree() {
            stack.Pop();
            currentNode=stack.Peek();
        }
        public void start()
        {
            Leafterivestring.Add("<Start> =>");
            root = new Start<string>("start","");
            stack.Push(root);
            currentNode= root;
            while (currentindex<Tokens.Count|| IsClassDeclOrFuncDef(CurrentToken))
            {
               
                ClassDeclOrFuncDef();

            }
        }
        public void ClassDeclOrFuncDef()
        {
            if (IsClassDecl(CurrentToken))
            {
                addsubtree(new classdecl<string>("classdecl", ""));
                
                NextToken();
                
                ClassDecl();
                Poptree();
            }
            else if (IsFuncDef(CurrentToken))
            {
                addsubtree(new functiondecl<string>("functiondecl", ""));
                
                FuncDef();
                Poptree();
            }
            else
            {
                Error("ClassDeclOrFuncDef");
            }
        }

        public void ClassDecl()
        {
            if (IsId(CurrentToken))
            {
                
                NextToken();
                ClassDecls();
                if (CurrentToken.Code == TokenCode.LeftBracket)
                {
                    
                    NextToken();
                    //need loop
                    
                    if (isMemberdeclear(CurrentToken))
                    {
                        while (isMemberdeclear(CurrentToken))
                        {
                            addsubtree(new memberdeclear<string>("memberdeclear",""));
                            if (IsVisibility(CurrentToken))
                            {   
                                NextToken();
                            }
                            if (isMemberFuncDecl(CurrentToken))
                            {
                                addsubtree(new memberfuncdecl<string>("memberfuncdecl",""));
                                if (CurrentToken.Code == TokenCode.function)
                                {
                                    NextToken();
                                    if (CurrentToken.Code == TokenCode.Id)
                                    {
                                        NextToken();
                                        if (CurrentToken.Code == TokenCode.Colon)
                                        {
                                            NextToken();
                                            if (CurrentToken.Code == TokenCode.LeftParenthesis)
                                            {
                                                NextToken();
                                                if (isFParams(CurrentToken))
                                                {
                                                    addsubtree(new FParams<string>("FParams",""));
                                                    FParams();
                                                    Poptree();
                                                }
                                                if (CurrentToken.Code == TokenCode.RightParenthesis)
                                                {
                                                    NextToken();
                                                    if (CurrentToken.Code == TokenCode.arrow)
                                                    {
                                                        NextToken();
                                                        if (isreturntype(CurrentToken))
                                                        {
                                                            NextToken();
                                                            
                                                        }
                                                        else
                                                        {
                                                            Error("return");
                                                            //error
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                else if (CurrentToken.Code == TokenCode.constructor)
                                {
                                    NextToken();
                                    if (CurrentToken.Code == TokenCode.Colon)
                                    {
                                        NextToken();
                                        if (CurrentToken.Code == TokenCode.LeftParenthesis)
                                        {
                                            NextToken();
                                            if (isFParams(CurrentToken))
                                            {
                                                FParams();
                                            }
                                            if (CurrentToken.Code == TokenCode.RightParenthesis)
                                            {
                                                NextToken();
                                               
                                            }
                                        }
                                    }
                                }
                                Poptree();
                                if (CurrentToken.Code == TokenCode.Semicolon)
                                {

                                    NextToken();

                                }
                                else
                                {
                                    //error
                                    Error("semicolon");
                                }
                            }
                            if (isMemberVarDecl(CurrentToken))
                            {
                                addsubtree(new memberVarDecl<string>("memberVarDecl",""));
                                NextToken();
                                if (CurrentToken.Code == TokenCode.Id)
                                {
                                    NextToken();
                                    if (CurrentToken.Code == TokenCode.Colon)
                                    {
                                        NextToken();
                                        if (istype(CurrentToken))
                                        {
                                            NextToken();
                                            while (isArrysize(CurrentToken))
                                            {
                                                arraySize();
                                            }
                                            if (CurrentToken.Code == TokenCode.Semicolon)
                                            {
                                                //membervardecl complete.
                                                NextToken();
                                            }
                                            else
                                            {
                                                Error("semicolon");
                                            }
                                        }
                                        else
                                        {
                                            Error("type");
                                        }


                                    }
                                    else
                                    {
                                        Error("colon");
                                    }

                                }
                                else
                                {
                                    Error("ID");
                                }
                                Poptree();
                            }
                            
                           Poptree();
                        }




                    }
                     if(CurrentToken.Code == TokenCode.RightBracket)
                    {

                            //membervardecl complete.
                            NextToken();
                        if(CurrentToken.Code == TokenCode.Semicolon)
                            {
                                
                                NextToken();
                            }
                    }
                    else
                    {
                        Error("Memberdecl");
                    }

                }
                else
                {
                    Error("{");
                }
            }
            else
            {
                Error("ID");
            }
        }
        private void Error(string message)
        {

            Errors.Add(CurrentToken.ToString() + " syntax error, expected: "+ message);
            NextToken();
            while((CurrentToken.Code!= TokenCode.RightBracket) && currentindex<Tokens.Count)
            {
                NextToken();
            }
            NextToken();
        }
        private void arraySize()
        {
            if (CurrentToken.Code == TokenCode.zhongkuoL)
            {
                NextToken();
                arraySizes();
            }
        }
        private void arraySizes()
        {
            if (CurrentToken.Code == TokenCode.intLit)
            {
                NextToken();
                if (CurrentToken.Code == TokenCode.zhongkuoR)
                {
                    NextToken();
                }
            }

            if (CurrentToken.Code == TokenCode.zhongkuoR)
            {
                NextToken();
            }
        }
        public void ClassDecls()
        {
            if (CurrentToken.Code == TokenCode.isa)
            {
                addsubtree(new classdecls<string>("classdecls",""));
                

                NextToken();
                if (CurrentToken.Code == TokenCode.Id)
                {
                   

                    NextToken();
                    ClassDeclss();
                    
                }
                else
                {
                    Error("ID");
                }
                Poptree();
            }
            
        }
        public void ClassDeclss()
        {
            if (CurrentToken.Code == TokenCode.dot)
            {
                addsubtree(new classdeclss<string>("classdeclss", ""));
                NextToken();
                if (CurrentToken.Code == TokenCode.Id)
                {
                    NextToken();
                    ClassDeclss();
                    
                }
                else
                {
                    Error("ID");
                }
                Poptree();
            }
          
        }
        public void FuncDef()
        {
            if (CurrentToken.Code == TokenCode.function)
            {
                NextToken();
                
                funcHead();
                
                funcBody();
            }
            else
            {
                Error("function");
            }

        }
        public void funcBody()
        {
            if(CurrentToken.Code==TokenCode.LeftBracket)
            {
                addsubtree(new funcBody<string>("funcBody",""));
                NextToken();
                while (isLocalVarDeclOrStmt(CurrentToken))
                {
                    if (isLocalVarDecl(CurrentToken))
                    {
                        
                        LocalVarDecl();
                        
                        if (CurrentToken.Code == TokenCode.RightBracket)
                        {
                            
                            break;
                        }

                    }
                    else
                    if (IsStatement(CurrentToken))
                    {
                        
                        statement();
                        
                        if (CurrentToken.Code == TokenCode.RightBracket)
                        {
                           
                            break;
                        }
                    }
                    else if(CurrentToken.Code == TokenCode.RightBracket)
                    {
                        break;
                    }
                    else
                    {
                        Error("}");
                    }
                }
                if (CurrentToken.Code == TokenCode.RightBracket)
                {
                    NextToken();
                }
                else
                {
                    Error("}");
                }
                Poptree();
            }
            else
            {
                Error("{");
            }
        }
        public void statement()
        {
            addsubtree(new statement<string>("statement",""));
            if (IsId(CurrentToken))
            {
                idnest();
                if (isAssignOp(CurrentToken))
                {
                    
                    NextToken();
                    if (isExpr(CurrentToken))
                    {
                        Expr();
                        if (CurrentToken.Code == TokenCode.Semicolon)
                        {
                            //complete
                            NextToken();
                        }
                        else
                        {
                            //error
                            Error(";");
                        }
                    }
                    else
                    {
                        //error
                        Error("Expr");
                    }
                    var temp = new BinaryExpressionNode_assign<string>("AssignOp", currentNode.Children[1].Value, currentNode.Children[0], currentNode.Children[2]);
                    currentNode.Children.Clear();
                    currentNode.Children.Add(temp);
                }
                else
                {
                    if (CurrentToken.Code == TokenCode.Semicolon)
                    {
                        //complete
                        NextToken();
                    }
                    else
                    {
                        //error
                        Error(";");
                    }
                    //not error
                }
            }
            else
            if (CurrentToken.Code == TokenCode.If)
            {
                addsubtree(new If<string>("If",""));
                NextToken();
                if (CurrentToken.Code == TokenCode.LeftParenthesis)
                {
                    NextToken();
                    if (isExpr(CurrentToken))
                    {
                        Expr();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {
                            NextToken();
                            if (CurrentToken.Code == TokenCode.then)
                            {
                                NextToken();
                                if (isStatBlock(CurrentToken))
                                {
                                    stateBlock();
                                    if (CurrentToken.Code == TokenCode.Else)
                                    {
                                        NextToken();
                                        if (isStatBlock(CurrentToken))
                                        {
                                            stateBlock();
                                        }else
                                        if (IsStatement(CurrentToken))
                                        {
                                            statement();
                                        }
                                        if(CurrentToken.Code==TokenCode.Semicolon)
                                        {
                                            //complete
                                            NextToken() ;
                                        }
                                        else
                                        {
                                            //error
                                            Error(";");
                                        }

                                    }
                                }else
                                if (IsStatement(CurrentToken))
                                {
                                    statement();
                                    if (CurrentToken.Code == TokenCode.Else)
                                    {
                                        NextToken();
                                        if (isStatBlock(CurrentToken))
                                        {
                                            stateBlock();

                                        }else
                                        if (IsStatement(CurrentToken))
                                        {
                                            statement();
                                        }
                                        if (CurrentToken.Code == TokenCode.Semicolon)
                                        {
                                            //complete
                                            NextToken();
                                        }
                                        else
                                        {
                                            //error
                                            Error(";");
                                        }
                                    }
                                }
                                else
                                {
                                    //error
                                    Error("statement");
                                }
                            }
                            else
                            {
                                //error
                                Error("then");
                            }
                        }
                        else
                        {
                            //error
                            Error(")");
                        }
                    }
                    else
                    {
                        //error
                        Error("Expr");
                    }
                }
                else
                {
                    //error
                    Error("(");
                }
                Poptree();
            }
            else
            if (CurrentToken.Code == TokenCode.While)
            {
                addsubtree(new whileloop<string>("whileloop",""));
                NextToken();
                if (CurrentToken.Code == TokenCode.LeftParenthesis)
                {
                    NextToken();
                    if (isExpr(CurrentToken))
                    {
                        Expr();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {NextToken ();
                            if (IsStatement(CurrentToken))
                            {
                                statement();
                            }else
                           if (isStatBlock(CurrentToken))
                            {
                                stateBlock();
                            }
                            if (CurrentToken.Code == TokenCode.Semicolon)
                            {
                                //complete
                                NextToken();
                            }
                            else
                            {
                                //error
                                Error(";");
                            }
                        }
                        else
                        {
                            //error
                            Error(")");
                        }
                    }
                    else
                    {
                        //error
                        Error("Expr");
                    }
                }
                else
                {
                    //error
                    Error("(");
                }
                Poptree();
            }
            else
            if (CurrentToken.Code == TokenCode.Read)
            {
                NextToken ();
                if (CurrentToken.Code == TokenCode.LeftParenthesis) {
                    NextToken();
                    if (CurrentToken.Code == TokenCode.Id)
                    {
                        variable();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {
                            NextToken();
                            if (CurrentToken.Code == TokenCode.Semicolon)
                            {
                                NextToken();
                                //complete
                            }
                            else
                            {
                                //error
                                Error(";");
                            }
                        }
                        else
                        {
                            //error
                            Error(")");
                        }
                    }
                    else
                    {
                        //error
                        Error("var");
                    }
                }
                else
                {
                    //error
                    Error("(");
                }
            }
            else
            if (CurrentToken.Code == TokenCode.Write)
            {
                addsubtree(new write<string>("write", ""));
                NextToken();
                if (CurrentToken.Code == TokenCode.LeftParenthesis)
                {
                    NextToken();
                    if (isExpr(CurrentToken))
                    {
                        Expr();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {
                            NextToken();
                            if (CurrentToken.Code == TokenCode.Semicolon)
                            {
                                NextToken();
                                //complete
                            }
                            else
                            {
                                Error(";");
                            }
                        }
                        else
                        {
                            Error(")");
                        }
                    }
                    else
                    {
                        //error
                        Error("Expr");
                    }
                }
                else
                {
                    //error
                    Error("(");
                }
                Poptree();
            }
            else
            if (CurrentToken.Code == TokenCode.Return)
            {
                NextToken();
                if (CurrentToken.Code == TokenCode.LeftParenthesis)
                {
                    NextToken();
                    if (isExpr(CurrentToken))
                    {
                        Expr();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {
                            NextToken();
                            if (CurrentToken.Code == TokenCode.Semicolon)
                            {
                                NextToken();
                                //complete
                            }
                            else
                            {
                                //error
                                Error(";");
                            }
                        }
                        else
                        {
                            Error(")");
                        }
                    }
                    else
                    {
                        //error
                        Error("Expr");
                    }
                }
                else
                {
                    //error
                    Error("(");
                }
            }
            else
            {
                //error
                Error(" statement");
            }
            Poptree();
        }
        public void variable()
        {
            while (IsId(CurrentToken))
            {
                addsubtree(new variable<string>("variable",""));
                NextToken();
                //var
                if (CurrentToken.Code == TokenCode.zhongkuoL)
                {
                    while (CurrentToken.Code == TokenCode.zhongkuoL)
                    {
                        indice();
                        if (CurrentToken.Code == TokenCode.dot)
                        {
                            NextToken();
                        }
                        else
                        {
                            break;

                        }
                    }
                }

                else
                if (CurrentToken.Code == TokenCode.dot)
                {
                    NextToken();
                }
                else
                {
                    break;
                }
                Poptree();
            }
        }
        public void stateBlock()
        {
            if(CurrentToken.Code==TokenCode.LeftBracket)
            {
                addsubtree(new stateBlock<string>("stateBlock",""));
                NextToken();
                while (IsStatement(CurrentToken))
                {
                    statement();
                    if (CurrentToken.Code == TokenCode.RightBracket)
                    {
                        NextToken();
                        break;
                    }
                }
                Poptree();
            }
            else
            {
                //error
                Error("{");
            }
        }
        public void LocalVarDecl()
        {
            if(CurrentToken.Code==TokenCode.localVar)
            {
                addsubtree(new LocalVarDecl<string>("LocalVarDecl",""));
                NextToken();
                if(CurrentToken.Code==TokenCode.Id)
                {
                    NextToken();
                    if(CurrentToken.Code==TokenCode.Colon)
                    {
                        NextToken();
                        if(istype(CurrentToken))
                        {
                            NextToken();
                            if (isArrysize(CurrentToken))
                            {addsubtree(new Arraysize<string>("Arraysize",""));
                                while (isArrysize(CurrentToken))
                                {
                                    arraySize();
                                }
                                if (CurrentToken.Code == TokenCode.Semicolon)
                                {
                                    NextToken();
                                    //complete
                                }
                                else
                                {
                                    Error(";");
                                }
                                Poptree();
                            }
                            else if(CurrentToken.Code==TokenCode.LeftParenthesis)
                            {
                                NextToken();
                                if (isAParams(CurrentToken))
                                {
                                    aParams();
                                }
                                
                                if (CurrentToken.Code == TokenCode.RightParenthesis)
                                {
                                    NextToken();
                                    if (CurrentToken.Code == TokenCode.Semicolon)
                                    {
                                        NextToken();
                                        //complete
                                    }
                                    else
                                    {
                                        Error(";");
                                    }
                                }
                                else
                                {
                                    Error(")");
                                }
                            }
                            else if (CurrentToken.Code == TokenCode.Semicolon)
                            {
                                NextToken();
                            }
                            else
                            {
                                Error(";");
                            }
;                        }
                        else
                        {
                            //error
                            Error("type");
                        }
                    }
                    else
                    {
                        //error
                        Error(":");
                    }
                }
                else
                {
                    //error
                    Error("id");
                }
                Poptree();
            }
            else
            {
                //error
                Error("localvar");
            }
        }
        public void aParams()
        {
            if(isExpr(CurrentToken))
            {
                Expr();
                while (isAParamsTail(CurrentToken))
                {
                    NextToken();
                    if (isExpr(CurrentToken))
                    {
                        Expr();
                    }
                }
            }
            else
            {
                Error("expr");
            }
        }
        public void Expr()
        {
            addsubtree(new Expr<string>("Expr",""));
            if (isArithExpr(CurrentToken))
            {
                
                ArithExpr();
                if (isRelOp(CurrentToken))
                {
                    
                    NextToken();
                    ArithExpr();
                    var temp = new BinaryExpressionNode_relop<string>("RelOp", currentNode.Children[1].Value, currentNode.Children[0], currentNode.Children[2]);
                    currentNode.Children.Clear();
                    currentNode.Children.Add(temp);
                }

            }
            Poptree();
        }
        public void ArithExpr()
        {
            if (isterm(CurrentToken))
            {
                addsubtree(new term<string>("term",""));
                Term();
                if (isAddOp(CurrentToken))
                {
                    NextToken();
                    ArithExpr();
                    var temp = new BinaryExpressionNode_addop<string>("AddOp", currentNode.Children[1].Value, currentNode.Children[0], currentNode.Children[2]);
                    currentNode.Children.Clear();
                    currentNode.Children.Add(temp);

                }
                else
                {
                    //complete
                }
                Poptree();
            }
            else
            {
                Error("term");
            }
        }
        public void Term()
        {
            if (isFactor(CurrentToken))
            {
                Factor();
                if (isMultOp(CurrentToken))
                {
                    
                    NextToken();
                    Term();
                    var temp = new BinaryExpressionNode_multop<string>("MultOp", currentNode.Children[1].Value, currentNode.Children[0], currentNode.Children[2]);
                    currentNode.Children.Clear();
                    currentNode.Children.Add(temp);

                }
                else
                {
                    //term complete
                }
            }
            

        }
        public void Factor()
        {
            addsubtree(new factor<string>("factor", ""));
            if(IsId(CurrentToken))
            {
                idnest();
            }else if(CurrentToken.Code==TokenCode.intLit)
            {
                addsubtree(new intLit<string>("integer", ""));
                NextToken();
                Poptree();
            }
            else if(CurrentToken.Code == TokenCode.floatLit)
            {
                addsubtree(new floatLit<string>("float", ""));
                NextToken();
                Poptree();
            }
            else if (CurrentToken.Code == TokenCode.LeftParenthesis)
            {
                NextToken();
                if (isArithExpr(CurrentToken))
                {
                    ArithExpr();
                }
                if(CurrentToken.Code == TokenCode.RightParenthesis)
                {
                    NextToken();
                    //complete
                }
                else
                {
                    Error(")");
                    //error
                }
            }else if (CurrentToken.Code == TokenCode.not)
            {
                NextToken();
                if (isFactor(CurrentToken))
                {
                    Factor();
                }
                else
                {
                    Error("factor");
                }
            }else if (isSign(CurrentToken))
            {
                
                sign();
                if (isFactor(CurrentToken))
                {
                    Factor();
                }
                else
                {
                    Error("factor");
                }
            }
            Poptree();

        }
        /// <summary>
        /// function or var is start with id
        /// </summary>
        public void idnest()
        {
            addsubtree(new idnest<string>("idnest",""));
            while (IsId(CurrentToken))
            {
                
                NextToken();
               //var
                if (CurrentToken.Code == TokenCode.zhongkuoL)
                {
                    var temp = new array<string>("array", "");
                    temp.Children.Add(currentNode.Children[0]);
                    currentNode.Children.Clear();
                    addsubtree(temp);
                    while(CurrentToken.Code == TokenCode.zhongkuoL)
                    {
                        indice();
                        if(CurrentToken.Code==TokenCode.dot)
                        {
                            NextToken();
                        }
                        else
                        {
                            

                        }
                    }
                    Poptree();
                }else
                //func
                if (CurrentToken.Code == TokenCode.LeftParenthesis)
                {
                    var temp = new function<string>("functioncall", "");
                    temp.Children.Add(currentNode.Children[0]);
                    currentNode.Children.Clear();
                    addsubtree(temp);
                    NextToken();
                    if(isAParams(CurrentToken))
                    {
                        aParams();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {
                            NextToken();
                            if(CurrentToken.Code==TokenCode.dot)
                            {
                                NextToken();
                            }
                            
                        }
                    }else if (CurrentToken.Code == TokenCode.RightParenthesis)
                    {
                        NextToken();
                        if (CurrentToken.Code == TokenCode.dot)
                        {
                            NextToken();
                        }
                        
                    }
                    else
                    {
                        Error(")");
                    }

                    Poptree();
                }
                else
                if (CurrentToken.Code == TokenCode.dot)
                {
                    NextToken();
                }
                

            }
            Poptree();
        }
        public void indice()
        {
            if (CurrentToken.Code == TokenCode.zhongkuoL)
            {
                NextToken();
                if (isAParams(CurrentToken))
                {
                    aParams();
                    if (CurrentToken.Code == TokenCode.zhongkuoR)
                    {
                        NextToken();
                        //indice complete
                    }
                    else
                    {
                        Error("]");
                    }
                }
            }
        }
        public void sign()
        {
            if(isSign(CurrentToken)) {
            NextToken();
            }
        }
        public void funcHead()
        {
            if (CurrentToken.Code == TokenCode.Id)
            {
                addsubtree(new funcHead<string>("funcHead",""));
                NextToken();
                if (CurrentToken.Code == TokenCode.sr)
                {
                    NextToken();
                    if (CurrentToken.Code == TokenCode.Id)
                    {
                        NextToken();
                        if (CurrentToken.Code == TokenCode.LeftParenthesis)
                        {
                            funcheadtypeone();

                        }
                        else
                        {
                            Error("(");
                        }


                    }
                    else if (CurrentToken.Code == TokenCode.constructor)
                    {
                        funcheadtypetwo();
                    }
                    else
                    {
                        Error("ID or constructor");
                    }
                }
                else
                {
                    funcheadtypeone();
                }
                Poptree();
            }
            else
            {
                Error("ID");
            }
        }
        public void funcheadtypeone()
        {
            if (CurrentToken.Code == TokenCode.LeftParenthesis)
            {
                addsubtree(new funcheadtypeone<string>("funcheadtypeone",""));
                NextToken();
                if (isFParams(CurrentToken))
                {
                    FParams();
                    if (CurrentToken.Code == TokenCode.RightParenthesis)
                    {
                        NextToken();
                        if (CurrentToken.Code == TokenCode.arrow)
                        {
                            NextToken();
                            if (isreturntype(CurrentToken))
                            {
                                NextToken();
                                //complete
                            }
                            else
                            {
                                Error("type");
                            }
                        }
                        else
                        {
                            Error("=>");
                        }
                    }
                    else
                    {
                        Error(")");
                    }
                }
                else
                if (CurrentToken.Code == TokenCode.RightParenthesis)
                {
                    NextToken();
                    if (CurrentToken.Code == TokenCode.arrow)
                    {
                        NextToken();
                        if (isreturntype(CurrentToken))
                        {
                            NextToken();
                            //complete
                        }
                        else
                        {
                            Error("TYPE");
                        }
                    }
                    else
                    {
                        Error("=>");
                    }
                }
                else
                {
                    Error(")");
                }
                Poptree();
            }
            else
            {
                Error("(");
            }
        }
        public void funcheadtypetwo()
        {
            if (CurrentToken.Code == TokenCode.constructor)
            {
                addsubtree(new funcheadtypetwo<string>("funcheadtypetwo",""));
                NextToken();
                if (CurrentToken.Code == TokenCode.LeftParenthesis)
                {
                    NextToken();
                    if (isFParams(CurrentToken))
                    {
                        FParams();
                        if (CurrentToken.Code == TokenCode.RightParenthesis)
                        {
                            NextToken();
                            //complete
                        }
                    }else if (CurrentToken.Code == TokenCode.RightParenthesis)
                    {
                        NextToken();
                        //complete
                    }
                    else
                    {
                        Error(")");
                    }
                }
                else
                {
                    Error("(");
                }
                Poptree();
            }
            else
            {
                Error("constructor");
            }

        }
        public void FParams()
        {
            if (CurrentToken.Code == TokenCode.Id)
            {
                NextToken();
                if (CurrentToken.Code == TokenCode.Colon)
                {
                    NextToken();
                    if (istype(CurrentToken))
                    {
                        NextToken();
                        while (isArrysize(CurrentToken))
                        {
                            addsubtree(new Arraysize<string>("Arraysize",""));
                            arraySize();
                            Poptree();
                        }
                        while (isFParamstail(CurrentToken))
                        {
                            addsubtree(new FParamstail<string>("FParamstail",""));
                            fParamsTail();
                            Poptree();
                        }

                    }
                }
            }
        }
        public void fParamsTail()
        {
            if (CurrentToken.Code == TokenCode.Comma)
            {
                NextToken();
                if (CurrentToken.Code == TokenCode.Id)
                {
                    NextToken();
                    if (CurrentToken.Code == TokenCode.Colon)
                    {
                        NextToken();
                        if (istype(CurrentToken))
                        {
                            NextToken();
                            while (isArrysize(CurrentToken))
                            {
                                arraySize();
                            }
                        }
                    }
                }
            }


        }
    }
}
