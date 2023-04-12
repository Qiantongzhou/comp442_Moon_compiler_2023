using _442_a2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public abstract class AstNode<T>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public register localregister { get; set; }
        public AstNode<T> Left { get; set; }
        public AstNode<T> Right { get; set; }
        public List<AstNode<T>> Children { get; set; }

        public AstNode(string value, string name)
        {
            Value = value;
            Children = new List<AstNode<T>>();
            Name = name;
        }

        public void AddChild(AstNode<T> child)
        {
            Children.Add(child);
        }
        public static void PrintTree<T>(AstNode<T> node, int indent = 0)
        {
            Console.Write(new string(' ', indent));
            Console.WriteLine(node.Value);

            foreach (var child in node.Children)
            {
                PrintTree(child, indent + 2);
            }
        }

        public abstract void Accept(IAstVisitor<T> visitor);
    }

    public interface IAstVisitor<T>
    {
        void Visit(Start<T> node);
        void Visit(functiondecl<T> node);
        void Visit(leaf<T> node);
        void Visit(funcHead<T> node);
        void Visit(funcheadtypeone<T> node);
        void Visit(LocalVarDecl<T> node);
        void Visit(statement<T> node);
        
        void Visit(function<T> function);
        void Visit(funcheadtypetwo<T> funcheadtypetwo);
        void Visit(FParamstail<T> fParamstail);
        void Visit(array<T> array);
        void Visit(idnest<T> idnest);
        void Visit(term<T> term);
        void Visit(Expr<T> expr);
        void Visit(Arraysize<T> arraysize);
        void Visit(stateBlock<T> stateBlock);
        void Visit(whileloop<T> whileloop);
        void Visit(variable<T> variable);
        void Visit(If<T> @if);
        void Visit(funcBody<T> funcBody);
        void Visit(memberVarDecl<T> memberVarDecl);
        void Visit(FParams<T> fParams);
        void Visit(memberdeclear<T> memberdeclear);
        void Visit(classdeclss<T> classdeclss);
        void Visit(classdecls<T> classdecls);
        void Visit(classdecl<T> classdecl);
        void Visit(memberfuncdecl<T> memberfuncdecl);
        void Visit(intLit<T> intLit);
        void Visit(floatLit<T> floatLit);
        void Visit(factor<T> factor);
        void Visit(BinaryExpressionNode_relop<T> binaryExpressionNode_relop);
        void Visit(BinaryExpressionNode_multop<T> binaryExpressionNode_multop);
        void Visit(BinaryExpressionNode_addop<T> binaryExpressionNode_addop);
        void Visit(BinaryExpressionNode_assign<T> binaryExpressionNode_assign);
        void Visit(write<T> write);
    }
    

    public class Start<T> : AstNode<T>
    {

        public Start(string name,string value) : base(value,name)
        {
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
            
        }
    }
    public class intLit<T>: AstNode<T>
    {

        public intLit(string name, string value) : base(value, name)
        {
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class floatLit<T> : AstNode<T>
    {

        public floatLit(string name, string value) : base(value, name)
        {
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class factor<T> : AstNode<T>
    {

        public factor(string name, string value) : base(value, name)
        {
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class leaf<T> : AstNode<T>
    {

        public leaf(string name, string value) : base(value,name)
        {
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
            
        }
    }
    public class classdecl<T> : AstNode<T>
    {


        public classdecl(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class classdecls<T> : AstNode<T>
    {


        public classdecls(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class classdeclss<T> : AstNode<T>
    {


        public classdeclss(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class memberdeclear<T> : AstNode<T>
    {
        public memberdeclear(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class memberfuncdecl<T> : AstNode<T>
    {
        public memberfuncdecl(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class FParams<T> : AstNode<T>
    {
        public FParams(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class memberVarDecl<T> : AstNode<T>
    {
        public memberVarDecl(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class functiondecl<T> : AstNode<T>
    {


        public functiondecl(string name, string value) : base(value,name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class funcHead<T> : AstNode<T>
    {


        public funcHead(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class funcheadtypeone<T> : AstNode<T>
    {


        public funcheadtypeone(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class funcBody<T> : AstNode<T>
    {


        public funcBody(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class LocalVarDecl<T> : AstNode<T>
    {


        public LocalVarDecl(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class statement<T> : AstNode<T>
    {


        public statement(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class If<T> : AstNode<T>
    {


        public If(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class variable<T> : AstNode<T>
    {


        public variable(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class whileloop<T> : AstNode<T>
    {


        public whileloop(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class stateBlock<T> : AstNode<T>
    {


        public stateBlock(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class Arraysize<T> : AstNode<T>
    {


        public Arraysize(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class Expr<T> : AstNode<T>
    {


        public Expr(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class term<T> : AstNode<T>
    {


        public term(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class write<T> : AstNode<T>
    {


        public write(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class idnest<T> : AstNode<T>
    {


        public idnest(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class array<T> : AstNode<T>
    {


        public array(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }

    public class function<T> : AstNode<T>
    {


        public function(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class funcheadtypetwo<T> : AstNode<T>
    {


        public funcheadtypetwo(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    public class FParamstail<T> : AstNode<T>
    {


        public FParamstail(string name, string value) : base(value, name)
        {

        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class BinaryExpressionNode_assign<T> : AstNode<T>
    {


        public BinaryExpressionNode_assign(string name, string value, AstNode<T> left, AstNode<T> right) : base(value, name)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class BinaryExpressionNode_relop<T> : AstNode<T>
    {


        public BinaryExpressionNode_relop(string name, string value, AstNode<T> left, AstNode<T> right) : base(value, name)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class BinaryExpressionNode_addop<T> : AstNode<T>
    {


        public BinaryExpressionNode_addop(string name, string value, AstNode<T> left, AstNode<T> right) : base(value, name)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class BinaryExpressionNode_multop<T> : AstNode<T>
    {


        public BinaryExpressionNode_multop(string name, string value, AstNode<T> left, AstNode<T> right) : base(value, name)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(IAstVisitor<T> visitor)
        {
            visitor.Visit(this);

        }
    }
    public class Stack<T>
    {
        private List<T> _items;

        public Stack()
        {
            _items = new List<T>();
        }

        public void Push(T item)
        {
            _items.Add(item);
        }

        public T Pop()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            int index = _items.Count - 1;
            T item = _items[index];
            _items.RemoveAt(index);
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            int index = _items.Count - 1;
            return _items[index];
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool IsEmpty()
        {
            return _items.Count == 0;
        }

        public int Count()
        {
            return _items.Count;
        }
    }

}
