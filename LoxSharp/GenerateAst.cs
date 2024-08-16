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
            "Variable : Token name",

            "Expression : Expr expression",
            "Print : Expr expression ",
            "Var : Token name,Expr initializer",
        };


        DefineAst(outputDir, "Stmt", types);
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        String path = outputDir + "/" + baseName + ".cs";

        TextWriter writer = new StreamWriter(path,false,Encoding.UTF8);
        

        writer.WriteLine("namespace LoxSharp;");
        writer.WriteLine("\n \n");
        writer.WriteLine($"public abstract class {baseName} {{");

        defineVisitor(writer,baseName,types);

        foreach (string type in types){
            string className = type.Split(':')[0].Trim();
            string fields = type.Split(':')[1].Trim();
            defineType(writer,baseName, className, fields);
        }

        writer.WriteLine();
        writer.WriteLine("public abstract R accept<R>(IVisitor<R> visitor);");

        writer.WriteLine("}");

        writer.Close();

    }

    private static void defineVisitor(TextWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine("public interface IVisitor<R>{");
        foreach (string type in types) {
            string typeName = type.Split(':')[0].Trim();
            writer.WriteLine($"\t\tR visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
        }

        writer.WriteLine("}");
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
            writer.WriteLine($"\t\tthis.{name} = {name};");
        }

        writer.WriteLine("\t}");

        writer.WriteLine();
        writer.WriteLine("public override  R accept<R>(IVisitor<R> visitor) {");
        writer.WriteLine($"\t\t\t return visitor.visit{className}{baseName}(this);");
        writer.WriteLine("\t\t }");


        //make the fields
        writer.WriteLine("\n");

        foreach (string field in fields){
            writer.WriteLine($"\t public {field} {{ get; }}");
        }

        writer.WriteLine("}");

    }
}