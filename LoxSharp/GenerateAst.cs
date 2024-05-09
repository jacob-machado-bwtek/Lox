using System.Text;

namespace LoxSharp.Tools;

 public class GenerateAst{
    public static void GenerateAstMain(string[] args){
        if (args.Length != 2){
            Console.Error.WriteLine("Usage: loxsharp -generate-ast <output directory>");
            Environment.Exit(64);
        }
        string outputDir = args[1];

        List<string> types = new List<string>{
            "Binary : Expr left,Token Op,Expr right",
            "Grouping : Expr expression",
            "Literal : Object value",
            "Unary : Token op,Expr right",
        };


        DefineAst(outputDir, "Expr", types);
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        String path = outputDir + "/" + baseName + ".cs";

        TextWriter writer = new StreamWriter(path,false,Encoding.UTF8);
        

        writer.WriteLine("namespace LoxSharp;");
        writer.WriteLine("\n \n");
        writer.WriteLine($"abstract class {baseName} {{");

        foreach (string type in types){
            string className = type.Split(':')[0].Trim();
            string fields = type.Split(':')[1].Trim();
            defineType(writer,baseName, className, fields);
        }

        writer.WriteLine("}");

        writer.Close();

    }

    private static void defineType(TextWriter writer, string baseName, string className, string fieldList)
    {
        writer.WriteLine($"public class {className} : {baseName} {{");

        writer.WriteLine($"\t public {className}({fieldList}){{");

        string[] fields = fieldList.Split(",");
        // each is of the form {Type name}


        //put each param in field
        foreach(string field in fields){
            string name = field.Split(" ")[1];
            writer.WriteLine($"this.{name} = {name};");
        }

        writer.WriteLine("}");

        //make the fields


        writer.WriteLine("\n");

        foreach (string field in fields){
            writer.WriteLine($"\t public {field} {{ get; }}");
        }

        writer.WriteLine("}");

    }
}