using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Analyzer
{
    public enum Modifier
    {
        Public,
        Private,
        

    }
    public class symbole
    {
        public string Name = "";
        public string Type = "";
        public bool Isarray= false;
        public bool Isfunction= false;
        public bool isclass = false;
        public string Symbol = "";
        public string tag = "";
        public int Scope = 0;
        public int size = 0;
        public int stack = 0;
        public Modifier modifier { get; set; }

        public symbole(string name, string type,string symbol,bool isarray, int scope, Modifier modifier)
        {
            Name = name;
            Type = type;
            Scope = scope;
            Symbol = symbol;
            Isarray= isarray;
            this.modifier = modifier;
        }
       
    }
    public class ArraySymbol : symbole
    {
        public int Dimensions { get; }
        public List<int> Sizes { get; }

        public ArraySymbol(string name, string type,string symbol,bool isarray, int scope,Modifier modifier, int dimensions, List<int> sizes)
            : base(name, type,symbol,isarray, scope,modifier)
        {
            Dimensions = dimensions;
            Sizes = sizes;
        }
    }
    public class globaltable:table
    {
        public List<classtable> classtables=new List<classtable>();
        public List<functiontable> functiontables=new List<functiontable>();
        public globaltable(string name,symboltable symboltable=null):base(name,symboltable)
        {


        }
        public bool findfunction(string name)
        {
            var temp=functiontables.FirstOrDefault(s=>s.Name==name);
            if (temp != null)
            {
                return true;
            }
            return false;
        }
        public bool findclass(string name)
        {
            var temp = classtables.FirstOrDefault(s => s.Name == name);
            if (temp != null)
            {
                return true;
            }
            return false;
        }

    }
    public abstract class table
    {
        public string Name { get; set; }
        public symboltable symboltable;
        public int currentScope = 0;
        public table(string Name, symboltable symboltable)
        {
            this.Name = Name;
            this.symboltable = symboltable;
        }
    }
    public class classtable:table
    {
        
        public classtable parent;
        
        public List<functiontable> functiontables;

        public classtable(string name, classtable parent=null, symboltable symboltable = null, List<functiontable> functiontable = null):base(name,symboltable)
        {
            this.parent = parent;
            this.functiontables = functiontable;
        }
    }
    public class functiontable:table
    {   
        public List<symbole> functionparams=new List<symbole>();
        public symbole returntype=new symbole("","","",false,0,Modifier.Public);
        public List<object> statements=new List<object>();
            
        public functiontable(string name, List<symbole> functionparams=null, symboltable symboltable= null):base(name,symboltable)
        {
            if (functionparams != null)
            {
                this.functionparams = functionparams;
            }
            if (symboltable != null)
            {
                this.symboltable = symboltable;
            }
        }
        public symbole Check(string value, int scope)
        {
            symbole temp= functionparams.FirstOrDefault(s => s.Symbol == value);
            if (temp != null)
            {
                return temp;
            }
            temp= symboltable.Check(value, scope);

            return temp;
        }
    }
    public class symboltable
    {
        public List<symbole> symbols = new List<symbole>();
        
        public void Add(symbole symbole)
        {
            symbols.Add(symbole);
        }

        public symbole Check(string value, int scope)
        {
            return symbols.FirstOrDefault(s => s.Symbol == value && s.Scope <= scope);
        }
    }

    public class TraverseNodefortable<T> : IAstVisitor<T>
    {
        public globaltable table;
        
        private functiontable currentfunctiontable;
        private bool isfree = true;

        private classtable currentclasstable;

        private string currenttype="";
        private Modifier currentvisibility=Modifier.Private;

        public List<string> errorlist=new List<string>();

        public void Visit(Start<T> node)
        {
            table=new globaltable("global");
            foreach (AstNode<T> child in node.Children)
            {
                child.Accept(this);
            }

        }
        public void Visit(functiondecl<T> node)
        {
            
            if (node.Name == "functiondecl")
            {
                
                node.Children[1].Accept(this);
                currentfunctiontable.currentScope++;
                if (isfree)
                {
                    if (table.findfunction(currentfunctiontable.Name))
                    {
                        errorlist.Add("multiple function declear:" + currentfunctiontable.Name);
                        Console.WriteLine("multiple function declear:" + currentfunctiontable.Name);
                    }
                    else
                    {
                        table.functiontables.Add(currentfunctiontable);
                    }
                }
                node.Children[2].Accept(this);
                currentfunctiontable.currentScope--;
                
            }
        }

        public void Visit(leaf<T> node)
        {
            if (node.Name == "<identifier>")
            {
                if (node.Value == "self") { 
                
                }
                else
                {


                    symbole temp = currentfunctiontable.symboltable.Check(node.Value, currentfunctiontable.currentScope);
                    if (temp != null)
                    {
                        if (temp.Isarray)
                        {
                            currenttype = "array";
                        }
                        else
                        {
                            currenttype = temp.Type;
                        }
                    }
                    else
                    {
                        if (currentfunctiontable != null)
                            temp = currentfunctiontable.functionparams.FirstOrDefault(s => s.Symbol == node.Value && s.Scope <= currentfunctiontable.currentScope);
                        if (temp != null)
                        {
                            currenttype = temp.Type;
                        }
                        else
                        {
                            if (currentclasstable != null)
                                temp = currentclasstable.symboltable.Check(node.Value, currentfunctiontable.currentScope);
                            if (temp != null)
                            {
                                currenttype = temp.Type;
                            }
                            else
                            {
                                if (currentclasstable != null)
                                {
                                    if (currentclasstable.parent != null)
                                    {

                                        temp = currentclasstable.parent.symboltable.Check(node.Value, currentfunctiontable.currentScope);
                                        if (temp != null)
                                        {
                                            currenttype = temp.Type;
                                        }
                                        else
                                        {
                                            errorlist.Add("unable to find:" + node.Value);
                                            Console.WriteLine("unable to find:" + node.Value);
                                        }
                                    }
                                    else
                                    {
                                        errorlist.Add("unable to find:" + node.Value);
                                        Console.WriteLine("unable to find:" + node.Value);
                                    }
                                }
                                else
                                {
                                    errorlist.Add("unable to find:" + node.Value);
                                    Console.WriteLine("unable to find:" + node.Value);
                                }
                            }
                        }
                    }
                }
                }else if (node.Name == "intLit")
                {
                    currenttype = "integer";
                } 
            
        }
       
        public void Visit(funcHead<T> node)
        {
            if (node.Name == "funcHead")
            {

                if (node.Children[1].Name == "sr") {
                    isfree = false;
                    //memberfunc
                    if (node.Children[2].Name == "<identifier>")
                    {
                        currentfunctiontable = new functiontable(node.Children[0].Value);
                        classtable link = table.classtables.FirstOrDefault(s => s.Name == node.Children[0].Value);
                        if (link != null)
                        {
                            for (int i = 0; i < link.functiontables.Count; i++)
                            {
                                if (link.functiontables[i].Name == node.Children[2].Value)
                                {
                                    var returntype = link.functiontables[i].returntype;
                                    link.functiontables[i] = currentfunctiontable;
                                    currentfunctiontable.returntype = returntype;
                                }
                            }

                            node.Children[3].Accept(this);
                        }
                        else
                        {
                            errorlist.Add("no class decl error:" + node.Children[0].Value);
                            Console.WriteLine("no class decl error:" + node.Children[0].Value);
                        }
                    }//constructor
                    else
                    {
                        currentfunctiontable = new functiontable(node.Children[2].Children[0].Value);
                        classtable link = table.classtables.FirstOrDefault(s => s.Name == node.Children[0].Value);
                        if (link != null)
                        {
                            for (int i = 0; i < link.functiontables.Count; i++)
                            {
                                if (link.functiontables[i].Name == node.Children[2].Children[0].Value)
                                {
                                    var returntype = link.functiontables[i].returntype;
                                    link.functiontables[i] = currentfunctiontable;
                                    currentfunctiontable.returntype= returntype;
                                }
                            }

                            node.Children[2].Accept(this);
                        }
                        else
                        {errorlist.Add("no class decl error:" + node.Children[0].Value);
                            Console.WriteLine("no class decl error:" + node.Children[0].Value);
                        }
                    }
                }
                else
                {
                    isfree = true;
                    currentfunctiontable = new functiontable(node.Children[0].Value);
                    node.Children[1].Accept(this);
                }
               
            }
        }

        public void Visit(funcheadtypeone<T> node)
        {
            
            if (node.Children[1].Name == "<identifier>")
            {
                currentfunctiontable.functionparams = new List<symbole>();
            }
            if (node.Children.Count > 4)
            {
                if (node.Children[4].Name == "Arraysize")
                {
                    currentfunctiontable.functionparams.Add(new symbole(node.Children[1].Name, node.Children[3].Value, node.Children[1].Value, true, currentfunctiontable.currentScope, Modifier.Private));
                }
                else
                {
                    currentfunctiontable.functionparams.Add(new symbole(node.Children[1].Name, node.Children[3].Value, node.Children[1].Value, false, currentfunctiontable.currentScope, Modifier.Private));
                }
                if (node.Children[5].Name == "FParamstail")
                {
                    for (int i = 5; i < node.Children.Count; i++)
                    {
                        node.Children[i].Accept(this);
                    }
                }
            }
            
                currentfunctiontable.returntype=new symbole("returntype",node.Children.Last().Value, node.Children.Last().Value, false, currentfunctiontable.currentScope, Modifier.Private);


        }

        public void Visit(LocalVarDecl<T> node)
        {
            if (node.Name == "LocalVarDecl")
            {
                if (currentfunctiontable.symboltable.Check(node.Children[1].Value, currentfunctiontable.currentScope) != null)
                {
                    errorlist.Add(node.Children[1].Value+": multiple localVar declear:" + currentfunctiontable.Name);
                    Console.WriteLine(node.Children[1].Value + ": multiple localVar declear:" + currentfunctiontable.Name);
                }
                else
                {
                    if (node.Children[4].Name == "Arraysize")
                    {
                        int dimension = (node.Children[4].Children.Count - 1) / 3;

                        if (dimension == 0)
                        {
                            currentfunctiontable.symboltable.Add(new ArraySymbol(node.Children[0].Value, node.Children[3].Value, node.Children[1].Value, true, currentfunctiontable.currentScope, Modifier.Private
                                 , dimension, null));
                        }
                        else
                        {
                            List<int> sizes = new List<int>();
                            for (int i = 0; i < dimension ; i++)
                            {
                                sizes.Add(int.Parse(node.Children[4].Children[1 + (i * 3)].Value));
                            }
                            currentfunctiontable.symboltable.Add(new ArraySymbol(node.Children[0].Value, node.Children[3].Value, node.Children[1].Value, true, currentfunctiontable.currentScope, Modifier.Private
                                , dimension, sizes));
                        }
                    }
                    else
                    {
                        currentfunctiontable.symboltable.Add(new symbole(node.Children[0].Value, node.Children[3].Value, node.Children[1].Value, false, currentfunctiontable.currentScope, Modifier.Private));
                    }
                }
            }
        }

        public void Visit(statement<T> node)
        {
            currentfunctiontable.statements.Add(node);
            node.Children[0].Accept(this);
        }

        public void Visit(function<T> function)
        {
            var temp=table.functiontables.FirstOrDefault(s => s.Name == function.Children[0].Value);
            if (temp != null)
            {
                if (function.Children[2].Name == "Expr")
                {
                    if (((function.Children.Count - 3) / 2) + 1 != temp.functionparams.Count)
                    {
                        errorlist.Add("functioncall parameter number not match:" + function.Children[0].Value);
                        Console.WriteLine("functioncall parameter number not match:" + function.Children[0].Value);
                    }
                    else
                    {
                        for (int i = 0; i < ((function.Children.Count - 3) / 2) + 1; i++)
                        {
                            function.Children[(i * 2) + 2].Accept(this);
                            if (temp.functionparams[i].Type == currenttype)
                            {
                                if (temp.functionparams[i].Isarray && currenttype != "array")
                                {
                                    errorlist.Add("functioncall parameter error:" + function.Children[0].Value);
                                    Console.WriteLine("functioncall parameter error:" + function.Children[0].Value);
                                }
                                else
                                {
                                   
                                }
                            }
                            else
                            {
                                if (temp.functionparams[i].Isarray && currenttype == "array")
                                {

                                }
                                else
                                {
                                    errorlist.Add("functioncall parameter error:" + function.Children[0].Value);
                                    Console.WriteLine("functioncall parameter error:" + function.Children[0].Value);
                                }
                            }

                        }
                    }
                }
            }
            else
            {errorlist.Add("cant find functioncall name:" + function.Children[0].Value);
                Console.WriteLine("cant find functioncall name:" + function.Children[0].Value);
            }
        }

        public void Visit(funcheadtypetwo<T> node)
        {
            if (node.Children[0].Value == "constructor")
            {
                if (node.Children[2].Name == "<identifier>")
                {
                    currentfunctiontable.functionparams = new List<symbole>();
                }
                if (node.Children.Count > 5)
                {
                    if (node.Children[5].Name == "Arraysize")
                    {
                        currentfunctiontable.functionparams.Add(new symbole(node.Children[2].Name, node.Children[4].Value, node.Children[2].Value, true, currentfunctiontable.currentScope, Modifier.Private));
                    }
                    else
                    {
                        currentfunctiontable.functionparams.Add(new symbole(node.Children[2].Name, node.Children[4].Value, node.Children[2].Value, false, currentfunctiontable.currentScope, Modifier.Private));
                    }
                    if (node.Children[5].Name == "FParamstail")
                    {
                        for (int i = 5; i < node.Children.Count; i++)
                        {
                            node.Children[i].Accept(this);
                        }
                    }
                }



            }
            else
            {
                if (node.Children[2].Name == "<identifier>")
                {
                    currentfunctiontable.functionparams = new List<symbole>();
                }
                if (node.Children.Count > 5)
                {
                    if (node.Children[5].Name == "Arraysize")
                    {
                        currentfunctiontable.functionparams.Add(new symbole(node.Children[1].Name, node.Children[3].Value, node.Children[1].Value, true, currentfunctiontable.currentScope, Modifier.Private));
                    }
                    else
                    {
                        currentfunctiontable.functionparams.Add(new symbole(node.Children[1].Name, node.Children[3].Value, node.Children[1].Value, false, currentfunctiontable.currentScope, Modifier.Private));
                    }
                    if (node.Children[5].Name == "FParamstail")
                    {
                        for (int i = 5; i < node.Children.Count; i++)
                        {
                            node.Children[i].Accept(this);
                        }
                    }
                }

                currentfunctiontable.returntype = new symbole("returntype", node.Children.Last().Value, node.Children.Last().Value, false, currentfunctiontable.currentScope, Modifier.Private);

            }
        }

        public void Visit(FParamstail<T> node)
        {
            currentfunctiontable.functionparams.Add(new symbole(node.Children[1].Name, node.Children[3].Value, node.Children[1].Value, false, currentfunctiontable.currentScope, Modifier.Private));
        }
    
    public void Visit(array<T> array)
    {
            var temp=lookup(array.Children[0], currentfunctiontable.currentScope);
            if (temp != null)
            {
                if (temp.GetType().Name == "ArraySymbol")
                {
                    ArraySymbol arraySymbol = (ArraySymbol)temp;
                    int dimen = (array.Children.Count - 1) / 3;
                    if (dimen == arraySymbol.Dimensions)
                    {
                        currenttype = temp.Type;
                    }
                    else
                    {
                        currenttype = "array";
                        errorlist.Add(currentfunctiontable.Name+": array dimension error:" + array.Children[0].Value);
                        Console.WriteLine(currentfunctiontable.Name + ": array dimension error:" + array.Children[0].Value);
                    }
                }
                else
                {
                    errorlist.Add(currentfunctiontable.Name + ": array error:" + array.Children[0].Value);
                    Console.WriteLine(currentfunctiontable.Name + ": array error:" + array.Children[0].Value);
                }
            }
            else
            {errorlist.Add(currentfunctiontable.Name + ": unable to find array:" + array.Children[0].Value);
                Console.WriteLine(currentfunctiontable.Name + ": unable to find array:" + array.Children[0].Value);
            }
    }
    public void Visit(idnest<T> node)
    {
        foreach(AstNode<T> child in node.Children)
            {
                child.Accept(this);
            }
            if (node.Children[0].Value == "self"&&node.Children.Count>2)
            {
                var temp=table.classtables.FirstOrDefault(s => s.Name == currentclasstable.Name);
                var st=temp.symboltable.Check(node.Children[2].Value, 0);
                if (st != null)
                {
                    currenttype = st.Type;
                }
                else
                {
                    errorlist.Add("unable to find idnest var:" + node.Children[2].Value);
                    Console.WriteLine("unable to find idnest var:" + node.Children[2].Value);
                }
            }
            else if(node.Children.Count>2)
            {
                var temp = currentfunctiontable.symboltable.Check(node.Children[0].Value,currentfunctiontable.currentScope);
                if (temp != null)
                {
                    var temp2 = table.classtables.FirstOrDefault(s => s.Name == temp.Type);
                    if (temp2 != null)
                    {
                        var st = temp2.symboltable.Check(node.Children[2].Value, 0);
                        if (st != null)
                        {
                            currenttype = st.Type;
                        }
                        else
                        {
                            errorlist.Add("unable to find idnest var:" + node.Children[2].Value);
                            Console.WriteLine("unable to find idnest var:" + node.Children[2].Value);
                        }
                    }
                }
            }
    }
    public void Visit(term<T> term)
    {
            foreach(AstNode<T>child in term.Children) { child.Accept(this); }
        }

        public void Visit(Expr<T> expr)
        {
            foreach(AstNode<T> child in expr.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(Arraysize<T> arraysize)
        {
            
        }

        public void Visit(stateBlock<T> stateBlock)
        {
            
        }

        public void Visit(whileloop<T> whileloop)
        {
            
        }

        public void Visit(variable<T> variable)
        {
            
        }

        public void Visit(If<T> @if)
        {
            
        }

        public void Visit(funcBody<T> funcBody)
        {
            currentfunctiontable.symboltable=new symboltable();
            foreach(AstNode<T> child in funcBody.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(memberVarDecl<T> node)
        {
            if (currentclasstable.symboltable.Check(node.Children[1].Value, currentclasstable.currentScope) != null)
            {
                errorlist.Add(node.Children[1].Value+": multiple class memberVar declear:" + currentclasstable.Name);
                Console.WriteLine(node.Children[1].Value+": multiple class memberVar declear:" + currentclasstable.Name);
            }
            else
            {
                var temp = new symbole(node.Children[0].Value, node.Children[3].Value, node.Children[1].Value, false, 0, currentvisibility);
                currentclasstable.symboltable.Add(temp);
            }
        }

        public void Visit(FParams<T> fParams)
        {
            currentfunctiontable.functionparams.Add(new symbole(fParams.Children[0].Value, fParams.Children[2].Value, fParams.Children[0].Value, false, 1, Modifier.Private));
        }

        public void Visit(memberdeclear<T> memberdeclear)
        {
            if (memberdeclear.Children[0].Name == "public")
            {
                currentvisibility = Modifier.Public;
            }
            else if (memberdeclear.Children[0].Name == "private")
            {
                currentvisibility = Modifier.Private;
            }
            else
            {
                currentvisibility = Modifier.Private;
            }
            foreach (AstNode<T> child in memberdeclear.Children)
            {
                
                child.Accept(this);
            }
        }

        public void Visit(classdeclss<T> classdeclss)
        {
            throw new NotImplementedException();
        }

        public void Visit(classdecls<T> classdecls)
        {
            
                var temp=table.classtables.FirstOrDefault(s => s.Name == classdecls.Children[1].Value);
            if (temp != null)
            {
                currentclasstable.parent = temp;
            }
            else
            {
                errorlist.Add("unclearedclass or circular declear not supported:" + classdecls.Children[1].Value);
                Console.WriteLine("unclearedclass or circular declear not supported:" + classdecls.Children[1].Value);
            }


        }

        public void Visit(classdecl<T> classdecl)
        {
            currentclasstable = new classtable(classdecl.Children[1].Value);
            currentclasstable.functiontables = new List<functiontable>();
            currentclasstable.symboltable=new symboltable();
            for(int i =2; i < classdecl.Children.Count; i++)
            {
                classdecl.Children[i].Accept(this);
            }
            if (table.findclass(currentclasstable.Name))
            {
                errorlist.Add("multiple class declear:" + currentclasstable.Name);
                Console.WriteLine("multiple class declear:" + currentclasstable.Name);
            }
            else
            {
                table.classtables.Add(currentclasstable);
            }
        }

        public void Visit(memberfuncdecl<T> node)
        {//member
            if (node.Children[0].Value == "function")
            {
                currentfunctiontable=new functiontable(node.Children[1].Value);
                currentfunctiontable.functionparams=new List<symbole>();
                for(int i = 4; i < node.Children.Count - 4; i++)
                {
                    node.Children[i].Accept(this);
                }
                currentfunctiontable.returntype = new symbole("returntype", node.Children.Last().Value, node.Children.Last().Value, false, currentfunctiontable.currentScope, Modifier.Private);

                currentclasstable.functiontables.Add(currentfunctiontable);

            }
            else
            {//constructor
                currentfunctiontable = new functiontable(node.Children[0].Value);
                if (node.Children[3].Name == "<identifier>")
                {
                    currentfunctiontable.functionparams = new List<symbole>();
                    if (node.Children[6].Name == "FParamstail")
                    {
                        for (int i = 6; i < node.Children.Count - 1; i++)
                        {
                            node.Children[i].Accept(this);
                        }
                    }
                }
                currentclasstable.functiontables.Add(currentfunctiontable);


            }
        }
        public symbole lookup(AstNode<T> node,int scope)
        {

            symbole temp=null;
            if (currentfunctiontable.functionparams != null)
            {
                temp = currentfunctiontable.functionparams.FirstOrDefault(s => s.Symbol == node.Value && s.Scope == scope);
            }
            if (temp != null)
            {
                return temp;
            }
            else
            {
                temp = currentfunctiontable.symboltable.Check(node.Value, scope);
                if(temp != null)
                {
                    return temp;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Visit(intLit<T> intLit)
        {
            intLit.Children[0].Accept(this);
        }

        public void Visit(floatLit<T> floatLit)
        {
            floatLit.Children[0].Accept(this);
        }

        public void Visit(factor<T> factor)
        {
            factor.Children[0].Accept(this);
        }

        public void Visit(BinaryExpressionNode_relop<T> node)
        {
            node.Left.Accept(this);
            string leftType = currenttype;
            node.Right.Accept(this);
            string rightType = currenttype;

            if (leftType != rightType)
            {
                Console.WriteLine(currentfunctiontable.Name + ": has binary operation error: (" + " leftType: " + leftType + " != rightType: " + rightType + ")");
                errorlist.Add(currentfunctiontable.Name + ": The left and right operands of the binary expression must be of the same type.");
            }
        }

        public void Visit(BinaryExpressionNode_multop<T> node)
        {
            node.Left.Accept(this);
            string leftType = currenttype;
            node.Right.Accept(this);
            string rightType = currenttype;

            if (leftType != rightType)
            {
                Console.WriteLine(currentfunctiontable.Name + ": has binary operation error: (" + " leftType: " + leftType + " != rightType: " + rightType + ")");
                errorlist.Add(currentfunctiontable.Name + ": The left and right operands of the binary expression must be of the same type.");
            }
        }

        public void Visit(BinaryExpressionNode_addop<T> node)
        {
            node.Left.Accept(this);
            string leftType = currenttype;
            node.Right.Accept(this);
            string rightType = currenttype;

            if (leftType != rightType)
            {
                Console.WriteLine(currentfunctiontable.Name + ": has binary operation error: (" + " leftType: " + leftType + " != rightType: " + rightType + ")");
                errorlist.Add(currentfunctiontable.Name + ": The left and right operands of the binary expression must be of the same type.");
            }
        }

        public void Visit(BinaryExpressionNode_assign<T> node)
        {
            node.Left.Accept(this);
            string leftType = currenttype;
            node.Right.Accept(this);
            string rightType = currenttype;

            if (leftType != rightType)
            {
                Console.WriteLine(currentfunctiontable.Name + ": has binary operation error: (" + " leftType: " + leftType + " != rightType: " + rightType + ")");
                errorlist.Add(currentfunctiontable.Name + ": The left and right operands of the binary expression must be of the same type.");
            }
        }

        public void Visit(write<T> node)
        {
            foreach(var item in node.Children)
            {
                item.Accept(this);
            }
        }
    } 


    }
