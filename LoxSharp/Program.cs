namespace CraftingInterpreters.CSLox;


using System;
using System.IO;
using System.Text;

public class Lox{
    public static void Main(string[] args) {
        if(args.Length > 1){
            Console.WriteLine("Usage: CSLox[Script]");
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

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null) break;
            Run(line);
        }
    }
    private static void RunFile(string path)
    {
        string content = File.ReadAllText(path, Encoding.Default);
        Run(content);
    }

    private static void Run(string input)
    {
        Console.WriteLine($"Running: {input}");
        
    }
}