using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;

namespace Analyzer
{
    internal class ASTdriver
    {
        //text file name
        public static string[] testfiles =
        {
            //"Test_array.src",
            //"Test_assginment.src",
            //"Test_classdeclarations.src",
            //"Test_complexexpression.src",
            //"Test_complexindest.src",
            //"Test_datamemberdeclarations.src",
            //"Test_free function.src",
            //"Test_if.src",
            //"Test_inheritancelist.src",
            //"Test_localvardecl.src",
            //"Test_memberfunction.src",
            //"Test_memberfunctiondeclarations.src",
            //"Test_ppmembers.src",
            //"Test_readwrite.src",
            //"Test_return.src",
            //"Test_variable.src",
            //"Test_while.src",
            //"Error_array.src",
            //"Error_circulardependency.src",
            //"Error_fucntioncall.src",
            //"Error_multiplydeclaredclass.src",
            //"Error_multiplydeclaredfreefunc.src",
            //"Error_multiplydeclareddatamember.src",
            //"Error_type.src",
            //"Error_undeclared.src",
            //"Error_undeclaredmemberfuction.src",
            //"Error_dotoperator_idnest.src",
            //"example-bubblesort.src",
            //"polynomialsemanticerrors.src",
            @"test4.src",
            //"test2.src"
            //"test4.src"
            //@"example-polynomial.src",

           // @"float-test.src",
           // "id-test.src",
           // "test-cmt.src",
           // "test-whole-code.src"
        };
        //start program
        static void Main()
        {

            new Mainscript();
            new StringBuilder();
        }
        public static void ExportAstToJson(AstNode<string> rootNode, string filePath)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting =Formatting.Indented
            };
            
            string json = JsonConvert.SerializeObject(rootNode, settings);
            File.WriteAllText(filePath, json);
        }
        public static void ExportTableToJson(globaltable globaltable, string filePath)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(globaltable, settings);
            File.WriteAllText(filePath, json);
        }
        public static void GenerateDotFile<T>(AstNode<T> node, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph AST {");

            GenerateDotFileHelper(node, sb);

            sb.AppendLine("}");

            File.WriteAllText(fileName, sb.ToString());
        }

        private static void GenerateDotFileHelper<T>(AstNode<T> node, StringBuilder sb)
        {
            sb.AppendLine($"node{node.GetHashCode()} [label=\"{node.Value}\"]");
            if (node.Left != null)
            {
                sb.AppendLine($"node{node.GetHashCode()} -> node{node.Left.GetHashCode()} [label=\"left\"]");
                GenerateDotFileHelper(node.Left, sb);
            }

            if (node.Right != null)
            {
                sb.AppendLine($"node{node.GetHashCode()} -> node{node.Right.GetHashCode()} [label=\"right\"]");
                GenerateDotFileHelper(node.Right, sb);
            }
            foreach (var child in node.Children)
            {
                sb.AppendLine($"node{node.GetHashCode()} -> node{child.GetHashCode()}");
                GenerateDotFileHelper(child, sb);
            }
        }

    }
}
