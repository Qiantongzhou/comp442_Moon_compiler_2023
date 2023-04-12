using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _442_a2
{
    // this method is abandoned with the following issue.
    //how to use sw (a stack value)(r14), r13, if I have the stack value in a register?
    //stack based call only 2 out of 100 mark, not worth the try.
    public class Assemblygenerator_stackbase<T> : IAstVisitor<T>
    {
        private Analyzer.Stack<register> registerpoll;
        globaltable table;
        private int stackOffset = 0;
        private IndentedStringBuilder assemblyCode = new IndentedStringBuilder();
        functiontable currentfunctiontable;
        public register currentinofregister;
        public register currentregister;
        bool haswritelink = false;
        private int currenttempvalue = 1;
        private string currentstring = "";
        private int currentarrayheight = 0;
        private int currentexprstack;
        private string currentsymbol = "";
        private bool isretrivel = false;
        private bool isarray = false;
        private bool isfunctioncall = false;
        private int currentelsevalue = 0;
        private int currentifvalue = 0;
        private int currentwhilevalue = 0;
        private Dictionary<int, string> callstack = new Dictionary<int, string>();
        static List<int> FindFirstContinuousMatch(Dictionary<int, string> dictionary, string targetValue)
        {
            var result = new List<int>();

            KeyValuePair<int, string>? previousPair = null;
            foreach (KeyValuePair<int, string> pair in dictionary)
            {
                if (previousPair != null && previousPair.Value.Value == targetValue && pair.Value == targetValue)
                {
                    result.Add(previousPair.Value.Key);
                    result.Add(pair.Key);
                    break;
                }
                previousPair = pair;
            }

            return result;
        }
        public Assemblygenerator_stackbase(globaltable t)
        {
            table = t;
        }
        public void generateassembly()
        {
            generatemain();
            //generatefreefunc();
            assemblyCode.AppendLine("hlt");
        }
        public void generatemain()
        {
            currentfunctiontable = table.functiontables.FirstOrDefault(s => s.Name == "main");
            assemblyCode.IncreaseIndent();
            assemblyCode.AppendLine("entry");

            foreach (var i in currentfunctiontable.symboltable.symbols)
            {
                if (i.Type == "float")
                {
                    i.size = 8;
                    i.stack = stackOffset;
                    stackOffset -= i.size;

                }
                else if (i.Type == "integer")
                {
                    if (i.Isarray)
                    {

                        ArraySymbol temp = (ArraySymbol)i;
                        int sizeofs = 4;
                        foreach (var arraysize in temp.Sizes)
                        {
                            sizeofs = sizeofs * arraysize;
                        }
                        for (int j = 1; j <= sizeofs; j++)
                        {
                            if (j % 4 == 0)
                            {
                                callstack.Add(j, i.Symbol);
                            }
                        }
                        i.size = sizeofs;
                        i.stack = stackOffset;
                        stackOffset -= i.size;
                        //assemblyCode.AddEndText($"{i.Symbol} res {i.size}\n");
                    }
                    else
                    {
                        i.size = 4;
                        i.stack = stackOffset;
                        stackOffset -= i.size;
                        //assemblyCode.AddEndText($"{i.Symbol} res {i.size}\n");
                    }
                }


            }
            currentinofregister = new register("r14", 0);
            assemblyCode.AppendLine("addi r1,r0,topaddr");
            assemblyCode.AppendLine($"addi {currentinofregister.name},r0,topaddr");
            assemblyCode.AppendLine($"subi r1,r1,{stackOffset}");

            foreach (var i in currentfunctiontable.statements)
            {
                var x = i as AstNode<T>;
                x.Accept(this);
            }
        }
        public IndentedStringBuilder getassemblycode()
        {
            return assemblyCode;
        }
        public void Visit(Start<T> node)
        {

            node.Accept(this);
        }

        public void Visit(functiondecl<T> node)
        {
            throw new NotImplementedException();
        }

        public void Visit(leaf<T> node)
        {
            //Console.WriteLine(node.Name);
            if (node.Name == "<identifier>")
            {

                var temp = currentfunctiontable.Check(node.Value, 1);
                //node.localregister=registerpoll.Pop() ;
                //assemblyCode.AppendLine("lw " + node.localregister.name + ", " + "0");
                //currentregister = node.localregister;
                currentsymbol = temp.Symbol;
            }
        }

        public void Visit(funcHead<T> node)
        {
            throw new NotImplementedException();
        }

        public void Visit(funcheadtypeone<T> node)
        {
            throw new NotImplementedException();
        }

        public void Visit(LocalVarDecl<T> node)
        {
            throw new NotImplementedException();
        }

        public void Visit(statement<T> node)
        {
            node.Children[0].Accept(this);
        }



        public void Visit(function<T> function)
        {
            
            int currentstackoffset = -8;
            for (int i = 2; i < function.Children.Count - 1; i++)
            {
                function.Children[i].Accept(this);
                i++;
                var temp = currentfunctiontable.Check(currentsymbol, 1);
                if (temp.Isarray)
                {
                    if (temp.Type == "integer")
                    {
                        var keylist = FindFirstContinuousMatch(callstack, temp.Symbol);
                        foreach (int key in keylist)
                        {
                            assemblyCode.AppendLine($"addi r12,r0,{key}");
                            assemblyCode.AppendLine($"lw r11,{temp.Symbol}(r12)");
                            assemblyCode.AppendLine($"sw {stackOffset + currentstackoffset}(r14),r11");
                            currentstackoffset += 4;
                        }
                    }
                }
                else
                {
                    assemblyCode.AppendLine($"lw r11,{currentsymbol}(r0)");
                    assemblyCode.AppendLine($"sw {stackOffset + currentstackoffset}(r14),r11");
                    currentstackoffset += 4;
                }

            }


            assemblyCode.AppendLine($"addi r14,r14,{stackOffset}");
            assemblyCode.AppendLine($"jl r15,{function.Children[0].Value}");
            assemblyCode.AppendLine($"subi r14,r14,{stackOffset}");
        }

        public void Visit(funcheadtypetwo<T> funcheadtypetwo)
        {
            throw new NotImplementedException();
        }

        public void Visit(FParamstail<T> fParamstail)
        {
            throw new NotImplementedException();
        }

        public void Visit(array<T> node)
        {
            isarray = true;
            int tempvar = 4;
            for (int i = 2; i < node.Children.Count; i++)
            {
                node.Children[i].Accept(this);
                tempvar = tempvar * int.Parse(currentstring);

                i++;
            }
            currentsymbol = node.Children[0].Value;
            tempvar += 4;
            currentregister = registerpoll.Pop();

            assemblyCode.AppendLine($"addi {currentregister.name}, r0, {tempvar}");

            currentarrayheight = tempvar;
            registerpoll.Push(currentregister);
            //assemblyCode.AppendLine($"sw {temp.Symbol}(r1),t{currenttempvalue}(r0)");
        }

        public void Visit(idnest<T> idnest)
        {
            foreach (var child in idnest.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(term<T> term)
        {
            term.Children[0].Accept(this);
        }

        public void Visit(Expr<T> expr)
        {
            expr.Children[0].Accept(this);
        }

        public void Visit(Arraysize<T> arraysize)
        {
            throw new NotImplementedException();
        }

        public void Visit(stateBlock<T> node)
        {
            for (int i = 1; i < node.Children.Count; i++)
            {
                node.Children[i].Accept(this);
            }
        }

        public void Visit(whileloop<T> node)
        {
            currentwhilevalue++;
            assemblyCode.Append($"gowhile{currentwhilevalue}", 0);
            node.Children[2].Accept(this);
            assemblyCode.AppendLine($"lw r1, t{currenttempvalue}(r0)");
            assemblyCode.AppendLine($"bz r1, endwhile{currentwhilevalue}");
            node.Children[4].Accept(this);
            assemblyCode.AppendLine($"j gowhile{currentwhilevalue}");
            assemblyCode.Append($"endwhile{currentwhilevalue}", 0);
        }

        public void Visit(variable<T> node)
        {



        }

        public void Visit(If<T> node)
        {
            currentifvalue++;
            node.Children[2].Accept(this);
            assemblyCode.AppendLine($"lw r1, t{currenttempvalue}(r0)");


            if (node.Children[6].Value == "else")
            {
                currentelsevalue++;
                assemblyCode.AppendLine($"bz r1, else{currentelsevalue}");
                node.Children[5].Accept(this);
                assemblyCode.AppendLine($"j endif{currentifvalue}");
                assemblyCode.Append($"else{currentelsevalue}", 0);
                node.Children[7].Accept(this);
                assemblyCode.Append($"endif{currentifvalue}", 0);
            }
            else
            {
                assemblyCode.AppendLine($"bz r1, endif{currentelsevalue}");
                node.Children[5].Accept(this);
                assemblyCode.Append($"endif{currentifvalue}", 0);
            }

        }

        public void Visit(funcBody<T> funcBody)
        {
            throw new NotImplementedException();
        }

        public void Visit(memberVarDecl<T> memberVarDecl)
        {
            throw new NotImplementedException();
        }

        public void Visit(FParams<T> fParams)
        {
            throw new NotImplementedException();
        }

        public void Visit(memberdeclear<T> memberdeclear)
        {
            throw new NotImplementedException();
        }

        public void Visit(classdeclss<T> classdeclss)
        {
            throw new NotImplementedException();
        }

        public void Visit(classdecls<T> classdecls)
        {
            throw new NotImplementedException();
        }

        public void Visit(classdecl<T> classdecl)
        {
            throw new NotImplementedException();
        }

        public void Visit(memberfuncdecl<T> memberfuncdecl)
        {
            throw new NotImplementedException();
        }

        public void Visit(intLit<T> intLit)
        {
            if (!isretrivel)
            {
                currenttempvalue++;
                currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                currentregister = registerpoll.Pop();
                assemblyCode.AppendLine($"addi {currentregister.name}," + "r0," + intLit.Children[0].Value);
                assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),{currentregister.name}");
            }
            currentstring = intLit.Children[0].Value;
            ///currentsymbol = $"t{currenttempvalue}(r0)";

        }

        public void Visit(floatLit<T> floatLit)
        {
            throw new NotImplementedException();
        }

        public void Visit(factor<T> factor)
        {
            factor.Children[0].Accept(this);

        }

        public void Visit(BinaryExpressionNode_relop<T> node)
        {

            isarray = false;
            node.Left.Accept(this);
            currentregister = registerpoll.Pop();
            if (isarray)
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}(r0)");
            }
            register temp = currentregister;
            isarray = false;
            node.Right.Accept(this);
            currentregister = registerpoll.Pop();
            currentregister = registerpoll.Pop();
            if (isarray)
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}(r0)");
            }
            register temp2 = currentregister;
            switch (node.Value)
            {
                case "==":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"ceq  r3, {temp2.name}, {temp.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r3");
                    break;
                case "<":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"clt  r3, {temp2.name}, {temp.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r3");
                    break;
                case "<=":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"cle  r3, {temp2.name}, {temp.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r3");
                    break;
                case ">":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"cgt  r3, {temp2.name}, {temp.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r3");
                    break;
                case ">=":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"cge  r3, {temp2.name}, {temp.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r3");
                    break;
                default:
                    throw new NotSupportedException($"Operator {node.Value} is not supported.");
            }
            currentregister = temp;
        }
        public void Visit(BinaryExpressionNode_multop<T> node)
        {
            isarray = false;
            node.Left.Accept(this);
            currentregister = registerpoll.Pop();
            if (isarray)
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}(r0)");
            }
            register temp = currentregister;
            isarray = false;
            node.Right.Accept(this);
            currentregister = registerpoll.Pop();
            if (isarray)
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}(r0)");
            }
            register temp2 = currentregister;
            switch (node.Value)
            {
                case "*":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"mul  r9, {temp.name}, {temp2.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r9");
                    break;
                case "/":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"div  r9, {temp.name}, {temp2.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r9");
                    break;
                default:
                    throw new NotSupportedException($"Operator {node.Value} is not supported.");
            }
            currentregister = temp;
        }

        public void Visit(BinaryExpressionNode_addop<T> node)
        {
            isarray = false;
            node.Left.Accept(this);
            currentregister = registerpoll.Pop();
            if (isarray)
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}(r0)");
            }
            register temp = currentregister;
            isarray = false;
            node.Right.Accept(this);
            currentregister = registerpoll.Pop();
            if (isarray)
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw {currentregister.name}, {currentsymbol}(r0)");
            }
            register temp2 = currentregister;
            switch (node.Value)
            {
                case "+":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"add  r9, {temp.name}, {temp2.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r9");
                    break;
                case "-":
                    currenttempvalue++;
                    currentfunctiontable.symboltable.Add(new symbole("intval", "integer", $"t{currenttempvalue}", false, 0, Modifier.Private));
                    assemblyCode.AddEndText($"t{currenttempvalue} res 4\n");
                    assemblyCode.AppendLine($"sub  r9, {temp.name}, {temp2.name}");
                    assemblyCode.AppendLine($"sw t{currenttempvalue}(r0),r9");
                    break;
                default:
                    throw new NotSupportedException($"Operator {node.Value} is not supported.");
            }
            currentregister = temp;
        }

        public void Visit(BinaryExpressionNode_assign<T> node)
        {
            isretrivel = false;
            node.Right.Accept(this);
            int tempvar = currenttempvalue;
            if (node.Left.Children[0].Name == "array")
            {
                isretrivel = true;
                var temp = currentfunctiontable.symboltable.Check(node.Left.Children[0].Children[0].Value, 1);
                node.Left.Children[0].Accept(this);
                assemblyCode.AppendLine($"lw r13,t{tempvar}(r0)");
                assemblyCode.AppendLine($"sw {temp.Symbol}({currentregister.name}),r13");

            }
            else
            {
                symbole temp = currentfunctiontable.symboltable.Check(node.Left.Children[0].Value, 1);

                assemblyCode.AppendLine("lw r10," + $"t{tempvar}(r0)");
                assemblyCode.AppendLine($"sw {temp.Symbol}(r0),r10");

            }
            registerpoll.Push(currentregister);

        }

        public void Visit(write<T> write)
        {
            write.Children[2].Accept(this);
            if (isarray)
            {
                assemblyCode.AppendLine($"lw r10,{currentsymbol}({currentregister.name})");
            }
            else
            {
                assemblyCode.AppendLine($"lw r10,{currentsymbol}(r0)");
            }
            assemblyCode.AppendLine("sw " + "-8(" + currentinofregister.name + "), " + $"r10");
            assemblyCode.AppendLine("addi r1,r0,buf");
            assemblyCode.AppendLine("sw -12(r14),r1");
            assemblyCode.AppendLine("jl r15, intstr");
            assemblyCode.AppendLine("sw -8(r14),r13");
            assemblyCode.AppendLine("jl r15,putstr");
            if (!haswritelink)
            {
                haswritelink = true;
                assemblyCode.AddEndText("buf res 20\n");
                assemblyCode.AddEndText("\nputstr    lw    r1,-8(r14)    % i := r1\r\n          addi  r2,r0,0\r\nputstr1   lb    r2,0(r1)      % ch := B[i]\r\n          ceqi  r3,r2,0\r\n          bnz   r3,putstr2    % branch if ch = 0\r\n          putc  r2\r\n          addi  r1,r1,1       % i++\r\n          j     putstr1\r\nputstr2   jr    r15");
                assemblyCode.AddEndText("\nintstr    lw    r13,-12(r14)\r\n          addi  r13,r13,11    % r13 points to end of buffer\r\n          sb    0(r13),r0     % store terminator\r\n          lw    r1,-8(r14)    % r1 := N (to be converted)\r\n          addi  r2,r0,0       % S := 0 (sign)\r\n          cgei  r3,r1,0\r\n          bnz   r3,intstr1    % branch if N >= 0\r\n          addi  r2,r2,1       % S := 1\r\n          sub   r1,r0,r1      % N := -N\r\nintstr1   addi  r3,r1,0       % D := N (next digit)\r\n          modi  r3,r3,10      % D mod= 10\r\n          addi  r3,r3,48      % D += \"0\"\r\n          subi  r13,r13,1     % i--\r\n          sb    0(r13),r3     % B[i] := D\r\n          divi  r1,r1,10      % N div= 10\r\n          cnei  r3,r1,0\r\n          bnz   r3,intstr1    % branch if N != 0\r\n          ceqi  r3,r2,0\r\n          bnz   r3,intstr2    % branch if S = 0\r\n          subi  r13,r13,1     % i--\r\n          addi  r3,r0,45\r\n          sb    0(r13),r3     % B[i] := \"-\"\r\nintstr2   jr    r15\n");
            }

        }
    }
}
