using System.Text;

namespace LoxSharp.Tools;

 public static class GenerateAst{
    public static void Main(string[] args){
        if (args.Length != 1){
            Console.Error.WriteLine("Usage: loxsharp -generate-ast <output directory>");
            Environment.Exit(64);
        }
        string outputDir = args[0];

        List<string> ExprTypes = new List<string>{
            "Assign : Token name,Expr value",
            "Binary : Expr left,Token Op,Expr right",
            "Call : Expr callee,Token paren,List<Expr> arguments",
            "Grouping : Expr expression",
            "Get : Expr obj,Token name",
            "Literal : Object value",
            "Logical : Expr left,Token Op,Expr right",
            "Set : Expr obj,Token name,Expr value",
            "Super : Token keyword,Token method",
            "This : Token keyword",
            "Unary : Token op,Expr right",
            "Variable : Token name",
        };

        foreach(string e in ExprTypes){
            Console.WriteLine(e);
        }
        List<string> StmtTypes = new List<string>{
            "Block : List<Stmt> statements",
            "Class : Token name,Expr.Variable superclass,List<Stmt.Function> methods",
            "Expression : Expr expression",
            "Function : Token name,List<Token> parameters,List<Stmt> body",
            "If : Expr condition,Stmt thenBranch,Stmt elseBranch", 
            "Print : Expr expression ",
            "Return : Token keyword,Expr value",
            "Var : Token name,Expr initializer",
            "While : Expr condition,Stmt body",

        };
        
        foreach(string s in StmtTypes){
            Console.WriteLine(s);
        }
        Console.WriteLine(StmtTypes);

        DefineAst(outputDir, "Expr", ExprTypes);
        Console.WriteLine("Expr.cs Generated");
        DefineAst(outputDir, "Stmt", StmtTypes);
        Console.WriteLine("Stmt.cs Generated");

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