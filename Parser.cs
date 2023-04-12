using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analyzer
{

    public partial class Parser
    {
        private bool IsStatement(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.If:
                case TokenCode.While:
                case TokenCode.Read:
                case TokenCode.Write:
                case TokenCode.Return:
                
                

                    return true;
                default:
                    return false;
            }
        }
        private bool IsVisibility(Token token)
        {

            switch (token.Code)
            {
                case TokenCode.Public:
                case TokenCode.Private:
               
               



                    return true;
                default:
                    return false;
            }
        } 
        
        private bool IsClassDeclOrFuncDef(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Class:
                case TokenCode.function:
                
                    return true;
                default:
                    return false;
            }
        }

        private bool IsClassDecl(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Class:
                

                    return true;
                default:
                    return false;
            }
        }
        private bool IsFuncDef(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.function:


                    return true;
                default:
                    return false;
            }
        }
        private bool IsId(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:



                    return true;
                default:
                    return false;
            }
        }
        private bool isMemberdeclear(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.function:
                case TokenCode.constructor:
                case TokenCode.attribute:
                case TokenCode.Public:
                case TokenCode.Private:


                    return true;
                default:
                    return false;
            }
        }
        private bool isMemberFuncDecl(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.function:
                case TokenCode.constructor:
                    return true;
                default:
                    return false;
            }
        }
        private bool isMemberVarDecl(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.attribute:
                    return true;
                default:
                    return false;
            }
        }
        private bool isFParams(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                    return true;
                default:
                    return false;
            }
        }
        private bool isreturntype(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Float:
                case TokenCode.Void:
                case TokenCode.Integer:
                case TokenCode.Id:
                    return true;
                default:
                    return false;
            }
        }
        private bool istype(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Float:
                case TokenCode.Integer:
                case TokenCode.Id:
                    return true;
                default:
                    return false;
            }
        }
        private bool isArrysize(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.zhongkuoL:
                    return true;
                default:
                    return false;
            }
        }
        private bool isFParamstail(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Comma:
                    return true;
                default:
                    return false;
            }
        }
        private bool isLocalVarDecl(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.localVar:
                    return true;
                default:
                    return false;
            }
        }
        private bool isStatBlock(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.LeftBracket:
                    return true;
                default:
                    return false;
            }
        }
        private bool isLocalVarDeclOrStmt(Token token)
        {
            return isLocalVarDecl(token)||IsStatement(token);
        }

        private bool IsIf(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.If:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsElse(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Else:
                    return true;
                default:
                    return false;
            }
        }



        private bool IsFor(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.For:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsAttribuition(Token token)
        {
            if (token.Code.Equals(TokenCode.Id))
            {
                if (LookAhead(token, 1).Code.Equals(TokenCode.Colon))
                {
                    return true;
                }
            }

            return false;
        }
        private bool isSign(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Plus:
                case TokenCode.Minus:
                    return true;
                default:
                    return false;
            }
        }
        private bool isAssignOp(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Equal:
                
                    return true;
                default:
                    return false;
            }
        }
        private bool isRelOp(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.eq:
                case TokenCode.Neq:
                case TokenCode.It:
                case TokenCode.gt:
                case TokenCode.leq:
                case TokenCode.geq:
                    return true;
                default:
                    return false;
            }
        }
        private bool isAddOp(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.Or:
                
                    return true;
                default:
                    return false;
            }
        }
        private bool isMultOp(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Multiplication:
                case TokenCode.Division:
                case TokenCode.And:

                    return true;
                default:
                    return false;
            }
        }
        private bool isFactor(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.intLit:
                case TokenCode.floatLit:
                case TokenCode.LeftParenthesis:
                case TokenCode.not:
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.Integer:
                case TokenCode.Float:
                    return true;
                default:
                    return false;

            }
        }
        private bool isterm(Token token)
        {
            return isFactor(token);
        }
        private bool isArithExpr(Token token)
        {
            return isterm(token);
        }
        private bool isRelExpr(Token token)
        {
            return isterm(token);
        }
        private bool isExpr(Token token)
        {
            return isArithExpr(token)||isRelExpr(token);
        }
        private bool isAParams(Token token)
        {
            return isExpr(token);
        }
        private bool isAParamsTail(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Comma:
                

                    return true;
                default:
                    return false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsIncrement(Token token)
        {
            if (token.Code.Equals(TokenCode.Id))
            {
                if (LookAhead(token, 1).Code.Equals(TokenCode.Increment))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsDecrement(Token token)
        {
            if (token.Code.Equals(TokenCode.Id))
            {
                if (LookAhead(token, 1).Code.Equals(TokenCode.Decrement))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsReturn(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Return:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsRead(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Read:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsWrite(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Write:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsExpression(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.True:
                case TokenCode.False:
                case TokenCode.Number:
                case TokenCode.Text:
                case TokenCode.Null:
                case TokenCode.LeftParenthesis:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsBodyFirst(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.LeftBracket:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsVarDecl(Token token)
        {
            if (IsExpression(LookAhead(token, 2)))
            {
                return true;
            }

            return false;
        }

        private bool IsFunctionDecl(Token token)
        {
            if (IsFunction(LookAhead(token, 2)))
            {
                return true;
            }

            return false;
        }

        private bool IsFunction(Token token)
        {
            if (token.Code.Equals(TokenCode.LeftParenthesis))
            {
                if (LookAhead(token, 2).Code.Equals(TokenCode.RightParenthesis) || LookAhead(token, 2).Code.Equals(TokenCode.Comma) || LookAhead(token, 3).Code.Equals(TokenCode.LeftBracket))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSide(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.True:
                case TokenCode.False:
                case TokenCode.Number:
                case TokenCode.Text:
                case TokenCode.Null:
                case TokenCode.LeftParenthesis:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsParam(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsParamCall(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.True:
                case TokenCode.False:
                case TokenCode.Number:
                case TokenCode.Text:
                case TokenCode.Null:
                case TokenCode.LeftParenthesis:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsTerm(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.True:
                case TokenCode.False:
                case TokenCode.Number:
                case TokenCode.Text:
                case TokenCode.Null:
                case TokenCode.LeftParenthesis:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsUnaryExprFirst(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Id:
                case TokenCode.Plus:
                case TokenCode.Minus:
                case TokenCode.True:
                case TokenCode.False:
                case TokenCode.Number:
                case TokenCode.Text:
                case TokenCode.Null:
                case TokenCode.LeftParenthesis:
                    return true;
                default:
                    return false;
            }
        }

       

        private bool IsBool(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.True:
                case TokenCode.False:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsFunctionCall(Token token)
        {
            if (token.Code.Equals(TokenCode.Id))
            {
                if (LookAhead(token, 1).Code.Equals(TokenCode.LeftParenthesis))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsConditionalOperator(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.Equal:
                case TokenCode.Different:
                case TokenCode.Less:
                case TokenCode.Greater:
                case TokenCode.LessOrEqual:
                case TokenCode.GreaterOrEqual:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsLogicalOperator(Token token)
        {
            switch (token.Code)
            {
                case TokenCode.And:
                case TokenCode.Or:
                    return true;
                default:
                    return false;
            }
        }
    }
    public partial class Parser
    {
        private void NextToken()
        {
            try
            {
                addleaf();
                currentstring += CurrentToken.Lexeme+" ";
               currentindex = currentindex + 1;
                if (currentindex < Tokens.Count) {
                    Leftderive.Add(CurrentToken);
                    String text = currentstring + "?<< current: " + CurrentToken.Lexeme + " expected: " + Tokens[currentindex].Code.GetTerminal()+" >>";
                    Leafterivestring.Add(text);

                CurrentToken = Tokens[currentindex];

                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Token LookAhead(Token current, int index)
        {
           index= Tokens.IndexOf(current)+index;
            if (index >= Tokens.Count)
            {
                Console.WriteLine(string.Format("Attempting to read the parser to remove ambiguity after token {0} in line {1}, column {2}. No token was found.", Tokens[currentindex].Lexeme, Tokens[currentindex].Line, Tokens[currentindex].Column));
            }
                return Tokens[index];
          
        }

        public void SyntaxError(Token token, TokenCode expected)
        {
            Console.WriteLine(string.Format("Syntax error at line {0}, column {1}: The token {2} provided is invalid. Expected: {3}", token.Line, token.Column, token.Lexeme, expected.GetTerminal()));
        }

        public void SyntaxError(Token token, TokenCode[] expected)
        {
            string expectedText = "";

            if (expected.Length < 2)
            {
                expectedText += expected[0].GetTerminal();
            }
            else
            {
                expectedText += expected[0].GetTerminal();

                for (int i = 1; i < expected.Length; ++i)
                {
                    if (i == expected.Length - 1)
                    {
                        expectedText = " ou " + expected[i].GetTerminal();
                    }
                    else
                    {
                        expectedText = ", " + expected[i].GetTerminal();
                    }
                }
            }

            throw new SyntaxErrorException(string.Format("Syntax error at line {0}, column {1}: The token {2} provided is invalid. Expected: {3}", token.Line, token.Column, token.Lexeme, expectedText));
        }
    }


}
