namespace LoxSharp;


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using global::LoxSharp;
using LoxSharp.Tools;

public class Lox{
    private static bool hadError = false;
    private static bool hadRuntimeError = false;

    private static readonly Interpreter myInterpreter = new Interpreter();

    public static void Main(string[] args) {
        
        if(args.Length > 1){

            if(args.Length == 2){
                if(args[0].ToLower() == "-generate-ast"){
                    GenerateAst.GenerateAstMain(args);
                }
            }
            else{
                            Console.WriteLine("Usage: LoxSharp[Script]");
            }
            Environment.Exit(64);
        } else if (args.Length == 1){
            RunFile(args[0]);
        }else{
            RunPrompt();
        }

        
    }

 private static void RunPrompt()
    {
        Console.InputEncoding = Encoding.Default;
        Console.OutputEncoding = Encoding.Default;
            Console.WriteLine("Entering LoxSharp");

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null) break;
            Run(line);
            hadError = false;
        }
    }
    private static void RunFile(string path)
    {
        string content = File.ReadAllText(path, Encoding.Default);
        Run(content);

        if(hadError){
            Environment.Exit(65);
        }
        if(hadRuntimeError){
            Environment.Exit(70);
        }
    }

    private static void Run(string input)
    {
        IScanner scanner = new Scanner(input);
        List<Token> tokens = scanner.scanTokens();
        Parser parser = new Parser(tokens);

        Expr expression = parser.Parse();

        if(hadError){
            return;
        }


        myInterpreter.Interpret(expression);

        //Console.WriteLine(new AstPrinter().Print(expression));
    }

    public static void Error(int line, string message){
        Report(line, message, "");
    }

    public static void Error(Token token, string message){
        if(token.type == TokenType.EOF){
            Report(token.line, "at end", message);
        }
        else{
            Report(token.line, $"at '{token.lexeme}'", message);
        }

    }

    private static void Report(int line, string message, string where)
    {
        TextWriter errorWriter = Console.Error;
        errorWriter.WriteLine($"[line {line}] Error {where} : {message}");

        hadError = true;
    }

    internal static void runtimeError(RuntimeError re)
    {
       Console.Error.WriteLine($"{re.Message} \n[line {re.Token.line}]");
       hadRuntimeError = true;
    }
}