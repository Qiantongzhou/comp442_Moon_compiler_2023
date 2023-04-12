
using _442_a2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Analyzer {
    class Mainscript {

        public Mainscript() {
            //get running dir
            string dir=Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length-9);
            Console.WriteLine("system dir: "+dir);

            //foreach file in list do analyze
            foreach (string file in ASTdriver.testfiles)
            {
                string str = System.IO.File.ReadAllText(dir + @"Testfiles/"+file);

                //analyze
                new Analyze().Lexicalanalyze(str, out Dictionary<Tokens, Dictionary<string, string>> outlexerrors, out Dictionary<Tokens, Dictionary<string, string>> lextokens,out List<Token>Tokenlist);

                //write error to file
                using (StreamWriter writer1 = new StreamWriter(dir + "OutputErrors/"+"outlexerrors_"+file))
                {
                    writer1.WriteLine("This is all the lexerrors");

                    foreach (var type in outlexerrors.Keys)
                    {
                        writer1.WriteLine("\n\n" + type.ToString() + ":\n");
                        foreach (var token in outlexerrors[type])
                        {

                            //Console.WriteLine(token.Key);
                            writer1.WriteLine(token.Key);
                        }

                    };
                }
                //write token to file
                using (StreamWriter writer2 = new StreamWriter(dir +"OutputTokens/" +"lextokens_"+file))
                {
                    writer2.WriteLine("This is all the Lexical Tokens");

                    foreach (var type in lextokens.Keys)
                    {
                        writer2.WriteLine("\n\n" + type.ToString() + ":\n");
                        foreach (var token in lextokens[type])
                        {

                            Console.WriteLine(token.Value);
                            writer2.WriteLine(token.Value + "       location: " + token.Key);
                        }

                    };
                }
                //write possible recovery to console
                Console.WriteLine("\nerror recovery:");
                foreach (var type in outlexerrors.Keys)
                {
                    
                    foreach (var token in outlexerrors[type])
                    {

                        //Console.WriteLine(token.Key);
                        if (token.Value!="")
                        {
                            errorrecovery(token.Value);
                        }
                    }

                };
                Console.WriteLine("\nsytexanlyzer:");
                Parser sytexanlyzer = new Parser(Tokenlist);
                sytexanlyzer.start();
                Console.Write("Start => \n");
                foreach(Token type in sytexanlyzer.Leftderive) {
                    Console.Write(type.Code.GetTerminal()+ " ");
                    if (type.Code.GetTerminal() == ";"|| type.Code.GetTerminal() == "{"||type.Code.GetTerminal()=="}")
                    {
                        Console.WriteLine();
                    }
                }
                Console.WriteLine("\n\nsyntex Errors:");
                foreach (string error in sytexanlyzer.Errors)
                {
                    Console.WriteLine(error);
                }
                using (StreamWriter writer3 = new StreamWriter(dir + "Outsyntaxerrors/" + "outsyntaxerrors_" + file))
                {
                    writer3.WriteLine("This is all the lexerrors");

                    foreach (string errors in sytexanlyzer.Errors)
                    {
                       
                            //Console.WriteLine(token.Key);
                            writer3.WriteLine(errors);
                        

                    };
                }
                //write token to file
                using (StreamWriter writer4 = new StreamWriter(dir + "Outderivation/" + "Outderivationtokens_" + file))
                {
                    writer4.WriteLine("Top-down syntax generation:");
                    foreach (var type in sytexanlyzer.Leafterivestring)
                    {
                        writer4.WriteLine(type);

                    };
                    writer4.WriteLine("\n\nSytax Tokens:");
                    foreach (var type in sytexanlyzer.Leftderive)
                    {
                            writer4.WriteLine(type);
                        
                    };
                    //
                    writer4.WriteLine("\n\nComplete Dervation tree, Code structure:");
                    writer4.Write("Start => \n");
                    foreach (Token type in sytexanlyzer.Leftderive)
                    {
                        writer4.Write(type.Code.GetTerminal() + " ");
                        if (type.Code.GetTerminal() == ";" || type.Code.GetTerminal() == "{" || type.Code.GetTerminal() == "}")
                        {
                            writer4.WriteLine();
                        }
                    }
                }
                PrintTree(sytexanlyzer.root);
                ASTdriver.GenerateDotFile(sytexanlyzer.root, dir + "DOT_syntaxtreefile/" + "syntaxtree_" + file + ".dot");
                ASTdriver.ExportAstToJson(sytexanlyzer.root, dir + "DOT_syntaxtreefile/" + "syntaxtree_" + file+".json");
                ASTdriver.GenerateDotFile(sytexanlyzer.root, dir + "AST_OUT/" + "" + file + ".outast");
                
                TraverseNodefortable<string> traverseNodefortable = new TraverseNodefortable<string>();
                sytexanlyzer.root.Accept(traverseNodefortable);
                globaltable symboltables = traverseNodefortable.table;
                outsymboltable(dir,file,symboltables);
                using (StreamWriter writer6 = new StreamWriter(dir + "OutSemeticErrors/" + "OutSemeticErrors_" + file))
                {
                    writer6.WriteLine("This is all the semeticerror");

                    foreach (string type in traverseNodefortable.errorlist)
                    {
                        writer6.WriteLine("" + type + "");
           
                    };
                }
                ASTdriver.ExportTableToJson(symboltables, dir + "OutSymboltables/" + "OutSymboltables_" + file+".json");
                //finalstep: assembly
                Console.WriteLine("\n\n\n Assembly:\n");
                Assemblygenerator<string> assemblygenerator = new Assemblygenerator<string>(symboltables);
                assemblygenerator.generateassembly();

                Console.WriteLine(assemblygenerator.getassemblycode());
                using (StreamWriter writer7 = new StreamWriter(dir + "A5_output_moon/" + file+".m"))
                {
                    writer7.WriteLine(assemblygenerator.getassemblycode());
                }
                Console.ReadLine();

            }

            


        }
        public static void outsymboltable(string dir,string file,globaltable symboltables)
        {
            string ot = "";
            ot+="======================================================================\n";
            ot += "| table: global\n";
            foreach ( classtable classtable in symboltables.classtables)
            {
                ot += "|=====================================================================\n";
                ot += "| class | " + classtable.Name+"\n";
                ot += "|    =================================================================\n";
                ot += "|    | table: " + classtable.Name+"\n";
                ot += "|    =================================================================\n";
                if (classtable.parent != null)
                {
                    ot += "|    | inherit: " + classtable.parent.Name + "\n";
                }
                else
                {
                    ot += "|    | inherit: None \n";
                }
                foreach (symbole symbole in classtable.symboltable.symbols)
                {
                    ot += "|    " +getindent(symbole)+"| "+symbole.Name+" | "+symbole.Symbol+" | "+symbole.Type+"\n";
                }
                if (classtable.functiontables != null)
                {
                    foreach (functiontable functiontable in classtable.functiontables)
                    {
                        ot += "|    | function  | " + functiontable.Name + "    | (";
                        if (functiontable.functionparams != null)
                        {
                            foreach (var symbole in functiontable.functionparams)
                            {
                                ot += symbole.Type;
                                if (symbole.Isarray)
                                {
                                    ot += "[]";
                                }
                                ot += ",";
                            }
                            ot = ot.Substring(0, ot.Length - 1);
                        }
                        ot += "):" + functiontable.returntype.Type + "\n";
                        
                        if (functiontable.symboltable != null&&functiontable.symboltable.symbols.Count>0)
                        {
                            ot += "|         ============================================================\n";
                            ot += "|         | table: " + functiontable.Name + "\n";
                            ot += "|         ============================================================\n";
                            foreach (symbole symbole in functiontable.functionparams)
                            {
                                ot += "|         " + getindent(symbole) + "| " + "Functionparameter" + " | " + symbole.Symbol + " | " + symbole.Type + "\n";
                            }
                            foreach (symbole symbole in functiontable.symboltable.symbols)
                            {
                                ot += "|      " + getindent(symbole) + "| " + symbole.Name + " | " + symbole.Symbol + " | " + symbole.Type + "\n";
                            }
                            ot += "|         ============================================================\n";
                        }
                        
                    }
                }
            }
            ot += "======================================================================\n";
            foreach (functiontable functiontable in symboltables.functiontables)
            {
                ot += "| function   | "+functiontable.Name+"    | (";
                if (functiontable.functionparams != null&&functiontable.functionparams.Count>0)
                {
                    foreach (var symbole in functiontable.functionparams)
                    {
                        ot += symbole.Type;
                        if (symbole.Isarray)
                        {
                            ot += "[]";
                        }
                        ot += ",";
                    }
                    ot = ot.Substring(0, ot.Length - 1);
                }
                ot += "):" + functiontable.returntype.Type+"\n";
                ot += "|    =================================================================\n";
                
                foreach (symbole symbole in functiontable.symboltable.symbols)
                {
                    ot += "| " + getindent(symbole) + "| " + symbole.Name + " | " + symbole.Symbol + " | " + symbole.Type;
                        if (symbole.Isarray)
                    {
                        ot += "[]";
                    }
                    ot+="\n";
                }
                ot += "|    =================================================================\n";
            }
            Console.WriteLine(ot);
            using (StreamWriter writer5 = new StreamWriter(dir + "OutSymboltables/" + "OutSymboltables_" + file))
            {
                writer5.Write(ot);
            }
           
        }
        public static string getindent(symbole symbole)
        {
            string x = "";
            for(int i = 0; i < symbole.Scope; i++)
            {
                x += "   ";
            }
            return x;
        }
        public static void PrintTree<T>(AstNode<T> node, int indent = 0)
        {
            Console.Write(new string(' ', indent));
            Console.WriteLine(node.Value);
            if (node.Left != null)
            {
                PrintTree(node.Left, indent + 2);
            }

            if (node.Right != null)
            {
                PrintTree(node.Right, indent + 2);
            }
            foreach (var child in node.Children)
            {
                PrintTree(child, indent + 2);
            }
        }

        public void errorrecovery(string token)
        {
            if (char.IsDigit(token[0]))
            {
                if (token.Substring(0, 1) == "0" && char.IsDigit(token[token.Length-1]) && !token.Contains("."))
                {
                    string fix = "[Invalid number: " + token + "] OR [integer: 0][integer: " + token.Substring(1, token.Length - 1) + "]";
                    Console.WriteLine(fix);
                }
                if (token.Substring(0, 1) == "0" && token.Substring(token.Length - 1, 1) == "0" && char.IsDigit(token[token.Length - 1]) && token.Contains("."))
                {
                    string fix = "[Invalid number: " + token + "] OR [integer: 0][float: " + token.Substring(1, token.Length - 2) + "][integer: 0]";
                    Console.WriteLine(fix);
                }else
                if (token.Substring(0, 1) == "0" && char.IsDigit(token[token.Length - 1]) && token.Contains("."))
                {
                    string fix = "[Invalid number: " + token + "] OR [integer: 0][float: " + token.Substring(1, token.Length - 1) + "]";
                    Console.WriteLine(fix);
                }
                else
                if (token.Substring(token.Length - 1, 1) == "0" && char.IsDigit(token[token.Length - 1]) && token.Contains("."))
                {
                    string fix = "[Invalid number: " + token + "] OR [float: " + token.Substring(0, token.Length - 2) + "][integer: 0]";
                    Console.WriteLine(fix);
                }

                //if (token.Substring(token.IndexOf("e") + 1, 1) == "0")
                //{
                //    string fix = "[Invalid number: " + token + "]";
                //    Console.WriteLine(fix);
                //}
                //if (token.Substring(token.IndexOf("e") + 2, 1) == "0" && (token.Substring(token.IndexOf("e") + 1, 1) == "-" || token.Substring(token.IndexOf("e") + 1, 1) == "+"))
                //{
                //    string fix = "[Invalid number: " + token + "]";
                //    Console.WriteLine(fix);
                //}
                    if (char.IsDigit(token[0]) && char.IsLetter(token[token.Length - 1]))
                {
                    int num=0;
                    for(int x=0;x<token.Length; x++)
                    {
                        
                        if (!char.IsDigit(token[x])){
                            break;
                        }
                        num++;
                    }
                    string fix = "[Invalid identifier: " + token + "] OR [integer: " + token.Substring(0, num) + "][identifier: " + token.Substring(num, token.Length - 1) + "]";
                    Console.WriteLine(fix);
                }
            }
            else
            {
                if (token.Substring(0, 1) == "_")
                {
                    string fix = "[Invalid identifier: " + token + "]";
                    Console.WriteLine(fix);
                }
            }

        }
       
        
    }
}
