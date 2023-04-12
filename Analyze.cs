using System;
using System.Collections.Generic;
namespace Analyzer {
    public enum AtomicElements
    {
        id,
        alphanum,
        integers,
        floats,
        fraction,
        letter,
        digit,
        nonzero,
    }
    //all possible token type
    public enum Tokens
    {
        id,
        integers,
        floats,

        Operators,
        Punctuation,
        ReservedWords,
    }
    class Analyze {
        public List<Token> Tokenlist=new List<Token>();
        List<List<int>> transitiontable = new List<List<int>>()
        {
            //q0             0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15    16       17   18 19
            //               (  )  *  +  ,  -  .  /  :  ;  <  =  >  [  ] ANY isdigit isletter {  }
            new List<int>(){ 0, 0,13,11,18,12,32,14,15,17, 8, 3, 6, 0, 0, 0,   30,       1,   0, 0 },
            //q1
            new List<int>(){ 0, 0, 0, 0, 0, 0,0, 0, 0, 0, 0 ,0 ,0, 0, 0, 0,    1,       1,   0, 0 },
            //q2
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q3
            new List<int>(){33,33,33,33,33,33,33,33,33,33,33, 4, 5,33,33,33,   33,      33,  33,33 },
            //q4,f,==
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q5,f,=>
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q6
            new List<int>(){34,34,34,34,34,34,34,34,34,34,34, 7,34,34,34,34,   34,      34,  34,34 },
            //q7,f,>=
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q8
            new List<int>(){35,35,35,35,35,35,35,35,35,35,35,10, 9,35,35,35,   35,      35,  35,35 },
            //q9,f,<>
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q10,f,<=
            //               (  )  *  +  ,  -  .  /  :  ;  <  =  >  [  ] ANY isdigit isletter {  }
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q11,f,+
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q12,f,-
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q13,f,*
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q14
            new List<int>(){ 2, 2,19, 2, 2, 2, 2,22, 2, 2, 2, 2, 2, 2, 2, 2,    2,       0,   0, 0 },
            //q15
            new List<int>(){37,37,37,37,37,37,37,37,16,37,37,37,37,37,37,37,   37,      37,  37,37 },
            //q16,f,::
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q17,f,;
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q18,f,,
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q19
            new List<int>(){ 0, 0,20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   19,      19,   0, 0 },
            //q20
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0,21, 0, 0, 0, 0, 0, 0, 0, 0,   19,      19,   0, 0 },
            //q21,f,/**/
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q22,f,//
            //               (  )  *  +  ,  -  .  /  :  ;  <  =  >  [  ] ANY isdigit isletter {  }
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q23
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q24
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q25
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q26
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q27
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q28
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q29
            new List<int>(){31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,   29,      38,  31,31 },
            //q30,f,digit
            new List<int>(){36,36,36,36,36,36,29,36,36,36,36,36,36,36,36,36,   30,       1,  36,36 },
            //q31,f,float
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q32
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       1,   0, 0 },
            //q33
             //              (  )  *  +  ,  -  .  /  :  ;  <  =  >  [  ] ANY isdigit isletter {  }
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q34
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q35
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q36
            new List<int>(){36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,36,   36,      36,  36,36 },
            //q37
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
            //q38
            new List<int>(){ 0, 0, 0,39, 0,39, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   39,       1,   0, 0 },
            //q39
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   40,       0,   0, 0 },
            //q40
            new List<int>(){41, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   40,       0,   0, 0 },
            //q41
            new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,    0,       0,   0, 0 },
        };
        IDictionary<string,bool> classes = new Dictionary<string, bool>() {
            { "no class declaration",false },
            { "class declaration",false },
            { "multiple class declarations",false },
            { "no data member declaration",false },
            { "data member declaration",false },
            { "multiple data member declaration",false },
            { "no member function declaration",false },
            { "member function declaration",false },
            { "multiple member function declaration",false },
            { "no member",false },
            { "no inherited class",false },
            { "one inherited class",false },
            { "multiple inherited classes",false },
            { "private member specifier",false },
            { "public member specifier",false },
        };
        IDictionary<string, bool> definitions = new Dictionary<string, bool>() {
            { "no main function definition",false },
            { "main function definition",false },
            { "no free function definition",false },
            { "free function definition",false },
            { "multiple free function definitions",false },
            { "no member function definition",false },
            { "member function definition",false },
            { "multiple member function definitions",false },
            { "return type: void",false },
            { "return type: integer",false },
            { "return type: float",false },
            { "return type: id",false },
            { "return type: array (not allowed)",false },
            
        };
        IDictionary<string, bool> formalparameters = new Dictionary<string, bool>() {
            { "type: integer",false },
            { "type: float",false },
            { "type: id",false },
            { "type: 1-dim array ",false },
            { "type: n-dim array",false },
            { "type: array (with size)",false },
            { "type: array (without size)",false },
        };
        IDictionary<string, bool> calls = new Dictionary<string, bool>() {
            { "free function call",false },
            { "member function call",false },
            { "parameters:0",false },
            { "parameters:1",false },
            { "parameters:n",false },
            { "array parameter - 1-dim",false },
            { "array parameter - n-dim",false },
            { "array parameter - with size",false },
            { "array parameter - without size",false },
            { "function call as statement",false },
            { "function call as expression factor",false },
            { "expression as parameter",false },
        };
        IDictionary<string, bool> variabledeclaration = new Dictionary<string, bool>() {
            { "type: integer",false },
            { "type: float",false },
            { "type: string",false },
            { "type: id",false },
            { "type: 1-dim array ",false },
            { "type: n-dim array",false },
            { "type: array (with size)",false },
            { "type: array (without size)",false },
           
        };
        IDictionary<string, bool> function_body_local_variable_declarations = new Dictionary<string, bool>() {
            { "no local variable declarations",false },
            { "local variable declarations",false },
            { "intertwined statements and variable declarations",false },
        };
        IDictionary<string, bool> function_body_statements = new Dictionary<string, bool>() {
            { "no statement",false },
            { "1 statement",false },
            { "n statements",false },
            { "if statement",false },
            { "if: empty then or else blocks",false },
            { "if: n-statements then or else blocks",false },
            { "while statement",false },
            { "while: empty block",false },
            { "while: 1-statement block",false },
            { "while: n-statement block",false },
            { "read(<variable>) statement",false },
            { "write(<expr>) statement",false },
            { "return(<expr>) statement",false },
            { "assignment statement",false },
        };
        IDictionary<string, bool> variable_idnest = new Dictionary<string, bool>() {
            { "id",false },
            { "id.id",false },
            { "id.id(id)",false },
            { "id(id).id",false },
            { "id(id).id()",false },
            { "id.id[id]",false },
            { "id[id].id",false },
            { "id[id].id[id]",false },
            { "id.id[id][id]",false },
            { "id[id][id].id",false },
            { "id[id][id].id[id][id]",false },
            { "id(id).id[id]",false },
            { "id(id).id[id][id]",false },
            { "expression as array index",false },
        };
        IDictionary<string, bool> expressions = new Dictionary<string, bool>() {
            { "single variable",false },
            { "involving addop",false },
            { "involving multop",false },
            { "involving relop",false },
            { "involving addop + multop",false },
            { "involving multop + relop",false },
            { "involving addop + multop + relop",false },
            { "involving parentheses",false },
            { "involving nested parenhteses",false },
            { "involving not",false },
            { "involving sign",false },
            { "involving literals",false },
            { "involving variable + idnest",false },
            { "involving function calls",false },
            { "involving all the above in one expression",false },
        };


        //               0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15    16       17   18 19
        //               (  )  *  +  ,  -  .  /  :  ;  <  =  >  [  ] ANY isdigit isletter {  }
        private int getNextState(int cState,char cChar) {
            if(cChar == '(')
                return transitiontable[cState][0];
            else if(cChar == ')')
                return transitiontable[cState][1];
            else if(cChar == '*')
                return transitiontable[cState][2];
            else if(cChar == '+')
                return transitiontable[cState][3];
            else if(cChar == ',')
                return transitiontable[cState][4];
            else if(cChar == '-')
                return transitiontable[cState][5];
            else if(cChar == '.')
                return transitiontable[cState][6];
            else if(cChar == '/')
                return transitiontable[cState][7];
            else if(cChar == ':')
                return transitiontable[cState][8];
            else if(cChar == ';')
                return transitiontable[cState][9];
            else if(cChar == '<')
                return transitiontable[cState][10];
            else if(cChar == '=')
                return transitiontable[cState][11];
            else if(cChar == '>')
                return transitiontable[cState][12];
            else if(cChar == '[')
                return transitiontable[cState][13];
            else if(cChar == ']')
                return transitiontable[cState][14];
            //15 is 16 + 17
            else if (char.IsDigit(cChar))
                return transitiontable[cState][16];
            else if (char.IsLetter(cChar)||cChar=='_')
                return transitiontable[cState][17];
            else if (cChar == '{')
                return transitiontable[cState][18];
            else if (cChar == '}')
                return transitiontable[cState][19];
            return transitiontable[cState][0];
        }

        private bool isReservedword(string word) {
            //no reserved word more than 16
            if((word).Length > 16 || (word).Length == 0)
                return false;
            //List check which one
            var Reservedwords = new List<string>(){
                "integer","float","void","class","isa","while","if","then","else",
            "read","write","return","localvar","constructor","attribute","function","public","private"};
            // Lambda
            return Reservedwords.Exists(element => (word.ToLower()) == element);
        }
        
        
        private void setReservedword(string CurrentToken,int line,int lineindex)
        {
            if (CurrentToken == "integer")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Integer, line, lineindex));
            }
            else
                                if (CurrentToken == "float")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Float, line, lineindex));
            }
            else
                                if (CurrentToken == "void")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Void, line, lineindex));
            }
            else
                                if (CurrentToken == "class")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Class, line, lineindex));
            }
            else
                                if (CurrentToken == "isa")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.isa, line, lineindex));
            }
            else
                                if (CurrentToken == "while")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.While, line, lineindex));
            }
            else
                                if (CurrentToken == "if")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.If, line, lineindex));
            }
            else
                                if (CurrentToken == "then")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.then, line, lineindex));
            }
            else
                                if (CurrentToken == "else")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Else, line, lineindex));
            }
            else
                                if (CurrentToken == "read")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Read, line, lineindex));
            }
            else
                                if (CurrentToken == "write")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Write, line, lineindex));
            }
            else
                                if (CurrentToken == "return")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Return, line, lineindex));
            }
            else
                                if (CurrentToken == "localvar")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.localVar, line, lineindex));
            }
            else
                                if (CurrentToken == "constructor")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.constructor, line, lineindex));
            }
            else
                                if (CurrentToken == "attribute")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.attribute, line, lineindex));
            }
            else
                                if (CurrentToken == "function")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.function, line, lineindex));
            }
            else
                                if (CurrentToken == "public")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Public, line, lineindex));
            }
            else
                                if (CurrentToken == "private")
            {
                Tokenlist.Add(new Token(CurrentToken, TokenCode.Private, line, lineindex));
            }
            else
              //if (CurrentToken == "self")
           // {
            //    Tokenlist.Add(new Token(CurrentToken, TokenCode.self, line, lineindex));
            //}
          //  else
            {
                Console.WriteLine("ERROR ID,PLZ FIX");
            }
        }
        public string Lexicalanalyze(string code,out Dictionary<Tokens, Dictionary<string, string>>  outlexerrors,out Dictionary<Tokens, Dictionary<string, string>> lextokens, out List<Token> Tokenlists) {
            outlexerrors = new Dictionary<Tokens, Dictionary<string, string>>();
            Tokenlists = Tokenlist;
            foreach(var token in Enum.GetValues(typeof(Tokens))){
                outlexerrors.Add((Tokens)token, new Dictionary<string, string>(){  });
            }

            lextokens = new Dictionary<Tokens,Dictionary<string,string>>();
            foreach (var token in Enum.GetValues(typeof(Tokens)))
            {
                lextokens.Add((Tokens)token, new Dictionary<string, string>() { });
            }

            if (code.Length == 0)
            {
                return "";
            }

            var Output = "";
            int Index = 0,CurrentState=0;
            char CharTemp = code[Index], cChar = ' ';
            string CurrentToken = "";
            bool NotBacktrack = true;
            int line = 0;
            int lineindex = 0;
            ///////
            code += " ";
            while(Index != code.Length) {

                if(NotBacktrack) {
                    cChar = CharTemp;
                    if(code.Length - 1 == Index)
                        return Output + cChar;
                    CharTemp = code[++Index];
                    lineindex++;
                    if(cChar== '\n') {
                        line++;
                        lineindex= 0;
                    }
                }
                else
                   NotBacktrack = true;
              
                //Do not process cmt
                if(cChar == '/' && CharTemp == '/') {
                    if(code.Length - 1 == Index)
                        return Output;
                    //remove everything till see enter
                    while(code[++Index] != '\n') {
                        if(code.Length == Index)
                            return Output;
                    }
                    Output += '\r';
                    if(code.Length - 1 != Index)
                        CharTemp = code[++Index];
                    continue;
                }
                //Do not process block cmt
                if(cChar == '/' && CharTemp == '*') {
                    if(code.Length - 1 == Index)
                        return Output;
                    CharTemp = code[++Index];
                    //remove everything till see */
                    do {
                        cChar = CharTemp;
                        if(code.Length - 1 == Index)
                            return Output + cChar;
                        CharTemp = code[++Index];
                        //without */ error
                        if (code.Length - 1 == Index)
                        {
                            Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid cmt\n");
                            outlexerrors[Tokens.Punctuation].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid cmt\n", CurrentToken);
                            return Output;
                        }
                    } while(!(cChar == '*' && CharTemp == '/'));
                    Output += '\r';
                    if(code.Length - 1 != Index)
                        CharTemp = code[++Index];
                    continue;
                }

                //Get next state from transition table
                CurrentState = getNextState(CurrentState,cChar);
                switch(CurrentState) {
                    case 0:
                        if (CurrentToken != "")
                        {
                            if (isReservedword(CurrentToken))
                            {
                                lextokens[Tokens.ReservedWords].Add(line + " " + lineindex, CurrentToken);
                                setReservedword(CurrentToken, line, lineindex);
                                Output += CurrentToken;
                                CurrentState = 0;
                                NotBacktrack = false;
                                CurrentToken = "";

                            }
                            else
                            {
                                //IF this is not reservered word, what error could happen?
                                if (char.IsDigit(CurrentToken[0]))
                                {
                                    Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with number at first\n");
                                    outlexerrors[Tokens.id].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with number at first\n", CurrentToken);
                                }
                                else
                                if (CurrentToken.Substring(0, 1) == "_")
                                {
                                    Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with char '_' at first\n");
                                    outlexerrors[Tokens.id].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with char '_' at first\n", CurrentToken);
                                }
                                else
                                if (!char.IsLetter(CurrentToken[0]))
                                {
                                    Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with not letter char at first\n");
                                    outlexerrors[Tokens.id].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with not letter char at first\n", CurrentToken);

                                }
                                else
                                {
                                    //no error? just treat as id
                                    lextokens[Tokens.id].Add(line + " " + lineindex, CurrentToken);
                                    Tokenlist.Add(new Token(CurrentToken, TokenCode.Id, line, lineindex));
                                    Output += CurrentToken;
                                    NotBacktrack = false;
                                    CurrentState = 0;
                                    CurrentToken = "";
                                }
                            }
                        }
                        else
                        {
                            // unreconized char
                            Output += cChar;
                            if (cChar == '[')
                            {
                                Tokenlist.Add(new Token(cChar.ToString(), TokenCode.zhongkuoL, line, lineindex));
                            }
                            if (cChar == ']')
                            {
                                Tokenlist.Add(new Token(cChar.ToString(), TokenCode.zhongkuoR, line, lineindex));
                            }
                            if (cChar == '(')
                            {
                                Tokenlist.Add(new Token(cChar.ToString(), TokenCode.LeftParenthesis, line, lineindex));
                            }
                            if (cChar == ')')
                            {
                                Tokenlist.Add(new Token(cChar.ToString(), TokenCode.RightParenthesis, line, lineindex));
                            }
                            if (cChar == '{')
                            {
                                Tokenlist.Add(new Token(cChar.ToString(), TokenCode.LeftBracket, line, lineindex));
                            }
                            if (cChar == '}')
                            {
                                Tokenlist.Add(new Token(cChar.ToString(), TokenCode.RightBracket, line, lineindex));
                            }

                            if (cChar == ';')
                            {
                                Tokenlist.Add(new Token(";", TokenCode.Semicolon, line, lineindex));
                            }
                            if (cChar == ',')
                            {
                                Tokenlist.Add(new Token(",", TokenCode.Comma, line, lineindex));
                            }
                        }
                        CurrentState = 0;
                        CurrentToken = "";
                        
                        break;
                    case 1:
                        // this word is reserved id
                        if (isReservedword(CurrentToken))
                        {
                            Output += CurrentToken;
                            lextokens[Tokens.ReservedWords].Add(line + " " + lineindex, CurrentToken);
                            setReservedword(CurrentToken, line, lineindex);
                            CurrentState = 0;
                            NotBacktrack = false;
                            CurrentToken = "";
                            break;
                        }
                        else
                        {
                            CurrentToken += cChar;
                        }
                        break;
                        
                    case 3:
                    case 6:
                    case 8:
                    case 15:
                    case 30:
                    case 29:
                    case 14:
                    
                        
                        CurrentToken += cChar;
                        break;
                    case 2:
                        
                        Output += "<divide>";

                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "/");
                        Tokenlist.Add(new Token("/", TokenCode.Division, line, lineindex));
                        CurrentState = 0;
                        NotBacktrack=false;
                        CurrentToken = "";
                        break;
                    case 4:
                        Output += "eq";
                        
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "==");
                        Tokenlist.Add(new Token("==", TokenCode.eq, line, lineindex));
                        CurrentState = 0;
                        CurrentToken = "";
                        break;
                    case 5:
                        Output += "=>";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "=>");
                        Tokenlist.Add(new Token("=>", TokenCode.arrow, line, lineindex));
                        CurrentState = 0;
                        CurrentToken = "";
                        break;
                    case 7:
                        Output += ">=";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, ">=");
                        Tokenlist.Add(new Token(">=", TokenCode.geq, line, lineindex));
                        CurrentState = 0;
                        CurrentToken = "";
                        break;
                    case 9:
                        Output += "<>";
                        lextokens[Tokens.Punctuation].Add(line + " " + lineindex, "<>");
                        //?
                        CurrentState = 0;
                        
                        CurrentToken = "";
                        break;
                    case 10:
                        Output += "<=";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "<=");
                        Tokenlist.Add(new Token("<=", TokenCode.leq, line, lineindex));
                        CurrentState = 0;
                        
                        CurrentToken = "";
                        break;
                    case 11:
                        Output += "+";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "+");
                        Tokenlist.Add(new Token("+", TokenCode.Plus, line, lineindex));
                        CurrentState = 0;
                        
                        CurrentToken = "";
                        break;
                    case 12:
                        Output += "-";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "-");
                        Tokenlist.Add(new Token("-", TokenCode.Minus, line, lineindex));
                        CurrentState = 0;

                        CurrentToken = "";
                        break;
                    case 13:
                        Output += "*";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, "*");
                        Tokenlist.Add(new Token("*", TokenCode.Multiplication, line, lineindex));
                        CurrentState = 0;

                        CurrentToken = "";
                        break;
                    case 16:
                        Output += "::";
                        lextokens[Tokens.Punctuation].Add(line + " " + lineindex, "::");
                        Tokenlist.Add(new Token("::", TokenCode.sr, line, lineindex));
                        CurrentState = 0;
                        
                        CurrentToken = "";
                        break;
                    case 17:
                        Output += ";";
                        lextokens[Tokens.Punctuation].Add(line + " " + lineindex, ";");
                        Tokenlist.Add(new Token(";", TokenCode.Semicolon, line, lineindex));
                        CurrentState = 0;
                        
                        CurrentToken = "";
                        break;
                    case 18:
                        Output += ",";
                        lextokens[Tokens.Punctuation].Add(line+" "+lineindex, ",");
                        Tokenlist.Add(new Token(",", TokenCode.Comma, line, lineindex));
                        CurrentState = 0;

                        CurrentToken = "";
                        break;
                    case 20:
                        break;
                    case 21:
                        break;
                    case 22:
                        break;
                    case 23:
                        break;
                    case 24:
                        break;
                    case 25:
                        break;
                    case 26:
                        break;
                    case 27:
                        break;
                    case 28:
                    
                        break;
                    case 31:
                        
                        if (CurrentToken.Substring(CurrentToken.Length - 1, 1) == "0")
                        {
                            if (char.IsDigit(CurrentToken.Substring(0, 1).ToCharArray()[0]))
                            {
                                Output += CurrentToken;
                                lextokens[Tokens.floats].Add(line + " " + lineindex, CurrentToken);
                                Tokenlist.Add(new Token(CurrentToken, TokenCode.floatLit, line, lineindex));
                            }
                            else
                            {
                                Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with 0 in end\n");
                                outlexerrors[Tokens.floats].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid idenification with 0 in end\n", CurrentToken);
                            }
                        }
                        else
                        {
                            Output += CurrentToken;
                            lextokens[Tokens.floats].Add(line + " " + lineindex, CurrentToken);
                            Tokenlist.Add(new Token(CurrentToken, TokenCode.floatLit, line, lineindex));
                        }
                        NotBacktrack = false;
                        CurrentState = 0;
                        CurrentToken = "";
                        break;
                    case 32:
                        Output+= "(id.id)";
                        
                        lextokens[Tokens.Punctuation].Add(line + " " + lineindex, ".");
                        //Tokenlist.Add(new Token(CurrentToken, TokenCode.Id, line, lineindex));
                        Tokenlist.Add(new Token(".", TokenCode.dot, line, lineindex));
                        CurrentState = 0;
                        CurrentToken = "";
                        break;
                    case 33:
                        Output += "=";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, CurrentToken);
                        Tokenlist.Add(new Token("=", TokenCode.Equal, line, lineindex));
                        CurrentState = 0;
                        NotBacktrack = false;
                        CurrentToken = "";
                        break;
                    case 34:
                        Output += ">";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, CurrentToken);
                        Tokenlist.Add(new Token(">", TokenCode.gt, line, lineindex));
                        CurrentState = 0;
                        NotBacktrack = false;
                        CurrentToken = "";
                        break;
                    case 35:
                        Output += "<";
                        lextokens[Tokens.Operators].Add(line + " " + lineindex, CurrentToken);
                        Tokenlist.Add(new Token("<", TokenCode.It, line, lineindex));
                        CurrentState = 0;
                        NotBacktrack = false;
                        CurrentToken = "";
                        break;
                    case 36:
                        //check float 
                        if (CurrentToken.Length > 1)
                        {
                            if (CurrentToken.Substring(0, 1) == "0")
                            {
                                Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid int with 0 at first\n");
                                outlexerrors[Tokens.floats].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid int with 0 at first\n", CurrentToken);

                            }
                            else
                            {
                                lextokens[Tokens.integers].Add(line + " " + lineindex, CurrentToken);
                                Tokenlist.Add(new Token(CurrentToken, TokenCode.intLit, line, lineindex));
                                Output += CurrentToken;
                            }
                        }
                        else
                        {
                            lextokens[Tokens.integers].Add(line + " " + lineindex, CurrentToken);
                            Tokenlist.Add(new Token(CurrentToken, TokenCode.intLit, line, lineindex));
                            Output += CurrentToken;
                        }
                        CurrentState = 0;
                        NotBacktrack = false;
                        CurrentToken = "";
                        break;
                    case 37:
                        Output += ":";
                        lextokens[Tokens.Punctuation].Add(line + " " + lineindex, ":");
                        Tokenlist.Add(new Token(":", TokenCode.Colon, line, lineindex));
                        CurrentState = 0;
                        NotBacktrack = false;
                        CurrentToken = "";
                        break;
                    case 38:
                        //float with e?
                        if (cChar == 'e')
                        {
                            CurrentToken += cChar;
                        }
                        else
                        {
                            Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with letter other than char 'e'\n");
                            outlexerrors[Tokens.floats].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with letter other than char 'e'\n", CurrentToken);
                        }
                        break;
                    case 39:
                        CurrentToken += cChar;
                        break;
                    case 40:
                        CurrentToken += cChar;
                        break;
                    case 41:
                        //exp float 
                        if (CurrentToken.Substring(CurrentToken.IndexOf("e") + 1, 1) == "0")
                        {
                            Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with 0 at first exp\n");
                            outlexerrors[Tokens.floats].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with 0 at first exp\n", CurrentToken);
                        }else
                        if (CurrentToken.Substring(CurrentToken.IndexOf("e") + 2, 1) == "0"&& (CurrentToken.Substring(CurrentToken.IndexOf("e") + 1, 1) == "-"|| CurrentToken.Substring(CurrentToken.IndexOf("e") + 1, 1) == "+"))
                        {
                            Console.WriteLine("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with 0 at first exp\n");
                            outlexerrors[Tokens.floats].Add("Error: at line:" + line + ", lineindex:" + (lineindex - 1) + ":\nToken:" + CurrentToken + " is not valid float with 0 at first exp\n", CurrentToken);
                        }
                        else
                        {

                            Output += CurrentToken;
                            lextokens[Tokens.floats].Add(line + " " + lineindex, CurrentToken);
                            Tokenlist.Add(new Token(CurrentToken, TokenCode.floatLit, line, lineindex));
                        }
                        CurrentState = 0;
                        NotBacktrack = false;
                        CurrentToken = "";
                        break;
                }
            }
            Console.WriteLine("result");
            Tokenlists = Tokenlist;
            return Output;
        }
    }
}