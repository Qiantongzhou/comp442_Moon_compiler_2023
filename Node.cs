using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{

    public class Token
    {
        public string Lexeme { get; set; }
        public TokenCode Code { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        /// <summary>
        /// Create a new Token.
        /// </summary>
        /// <param name="lexeme">Lexeme is the basic unit of the lexicon.</param>
        /// <param name="code">The token code to the formed lexeme.</param>
        /// <param name="line">The line location of the token.</param>
        /// <param name="column">The column location of the token.</param>
        public Token(string lexeme, TokenCode code = TokenCode.Id, int line = 1, int column = 0)
        {
            Lexeme = lexeme;
            Code = code;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return string.Format("Lexeme: {0}\tCode: {1}\tLine: {2}\tColumn: {3}", Lexeme, Code, Line, Column);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class TerminalAttribute : Attribute
    {
        public string Text { get; }

        public TerminalAttribute(string text)
        {
            Text = text;
        }
    }

   

    public enum TokenCode
    {
        [Terminal(",")] Comma,
        /// <summary>
        /// Keyword for return value.
        /// </summary>
        [Terminal("+")] Plus,
        /// <summary>
        /// Keyword for read a new data.
        /// </summary>
        [Terminal("-")] Minus,

        [Terminal("or")] Or,

        [Terminal("[")] zhongkuoL,
        [Terminal("]")] zhongkuoR,
        [Terminal("intLit")] intLit,

        [Terminal("=")] Equal,
        [Terminal("class")] Class,
        [Terminal("<identifier>")] Id,
        [Terminal("self")] self,

        /// <summary>
        /// Keyword for write an expression.
        /// </summary>
        [Terminal("<visibility>")] visibility,

        /// <summary>
        /// Code for left bracket symbol.
        /// </summary>
        [Terminal("{")] LeftBracket,
        /// <summary>
        /// Code for right bracket symbol.
        /// </summary>
        [Terminal("}")] RightBracket,


        /// <summary>
        /// End of line separator.
        /// </summary>
        [Terminal(";")] Semicolon,

        /// <summary>
        /// Code for left parenthesis symbol.
        /// </summary>
        [Terminal("(")] LeftParenthesis,
        /// <summary>
        /// Code for right parenthesis symbol.
        /// </summary>
        [Terminal(")")] RightParenthesis,

        [Terminal("floatLit")] floatLit,

        /// <summary>
        /// Logical negation operator.
        /// </summary>
        [Terminal("!")] not,

        /// <summary>
        /// Attribuition operator.
        /// </summary>
        [Terminal(":")] Colon,
        [Terminal("void")] Void,
        [Terminal(".")] dot,
        /// <summary>
        /// Multiplication operator.
        /// </summary>
        [Terminal("*")] Multiplication,
        /// <summary>
        /// Division operator.
        /// </summary>
        [Terminal("/")] Division,

        /// <summary>
        /// Logical intersection operator.
        /// </summary>
        [Terminal("and")] And,
        [Terminal("isa")] isa,


        [Terminal("eq")] eq,
        [Terminal("geq")] geq,
        [Terminal("gt")] gt,
        [Terminal("leq")] leq,
        [Terminal("It")] It,
        [Terminal("neq")] Neq,
        /// <summary>
        /// Keyword for "if" flow control.
        /// </summary>
        [Terminal("if")] If,
        [Terminal("then")] then,
        [Terminal("else")] Else,
        [Terminal("read")] Read,
        [Terminal("return")] Return,
        [Terminal("while")] While,
        [Terminal("write")] Write,
        [Terminal("float")] Float,
        [Terminal("integer")] Integer,
        [Terminal("private")] Private,
        [Terminal("public")] Public,
        /// <summary>
        /// Keyword for end of flow control.
        /// </summary>
        [Terminal("function")] function,
        /// <summary>
        /// Plus operator.
        /// </summary>
        [Terminal("arrow")] arrow,
        [Terminal("constructor")] constructor,
        [Terminal("attribute")] attribute,
        [Terminal("sr")] sr,
        [Terminal("localVar")] localVar,



        /// <summary>
        /// Module operator.
        /// </summary>
        [Terminal("%")] Module,
        /// <summary>
        /// Increment operator.
        /// </summary>
        [Terminal("++")] Increment,
        /// <summary>
        /// Decrement operator.
        /// </summary>
        [Terminal("--")] Decrement,
        

        /// <summary>
        /// Keyword for "for" flow control.
        /// </summary>
        [Terminal("for")] For,


        /// <summary>
        /// Equal operator.
        /// </summary>
        
        /// <summary>
        /// Less operator.
        /// </summary>
        [Terminal("<=")] LessOrEqual,
        /// <summary>
        /// Greater or equal operator.
        /// </summary>
        [Terminal(">=")] GreaterOrEqual,
        /// <summary>
        /// Less operator.
        /// </summary>
        [Terminal("<")] Less,
        /// <summary>
        /// Greater operator.
        /// </summary>
        [Terminal(">")] Greater,
        /// <summary>
        /// Different operator.
        /// </summary>
        [Terminal("!=")] Different,
       
        /// <summary>
        /// Logical union operator.
        /// </summary>
        
        /// <summary>
        /// Boolean value.
        /// </summary>
        [Terminal("true")] True,
        /// <summary>
        /// Boolean value.
        /// </summary>
        [Terminal("false")] False,
        /// <summary>
        /// Numerical value.
        /// </summary>
        [Terminal("<number>")] Number,
        /// <summary>
        /// Textual value.
        /// </summary>
        [Terminal("<text>")] Text,
        /// <summary>
        /// Parameters separator.
        /// </summary>
        

      
        /// <summary>
        /// Code for identification lexeme.
        /// </summary>
        [Terminal("null")] Null,
       
    }

    public static class TokenCodeExtensions
    {
        public static string GetTerminal(this TokenCode value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (TerminalAttribute)fieldInfo.GetCustomAttribute(typeof(TerminalAttribute));
            return attribute.Text;
        }
    }
}
