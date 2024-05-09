namespace LoxSharp;


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using global::LoxSharp;
using LoxSharp.Tools;

public class Lox{
    private static bool hadError;

    public static void Main(string[] args) {
                Console.WriteLine("running...");

        /*
        if(args.Length > 1){
            Console.WriteLine("Usage: LoxSharp[Script]");
            Environment.Exit(64);
        } else if (args.Length == 1){
            RunFile(args[0]);
        }else{
            RunPrompt();
        }*/
        Console.WriteLine("entering generateAst");
        GenerateAst.Main(args);
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
    }

    private static void Run(string input)
    {
        IScanner scanner = new DummyScanner(input);

        List<IToken> tokens = scanner.scanTokens();

        foreach(var t in tokens){
            Console.WriteLine(t);
        }
    }

    public static void error(int line, string message){
        report(line, message, "");
    }

    private static void report(int line, string message, string where)
    {
        TextWriter errorWriter = Console.Error;
        errorWriter.WriteLine($"[line {line}] Error {where} : {message}");

        hadError = true;
    }
}