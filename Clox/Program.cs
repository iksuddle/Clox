using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Clox;

public class Clox {
    internal static bool hadError = false;
    
    public static void Main(string[] args) {
        if (args.Length > 1) {
            Console.WriteLine("usage: clox [script]");
            Environment.Exit(64);
        } else if (args.Length == 1) {
            RunFile(args[0]);
        } else {
            RunPrompt();
        }
    }

    private static void RunFile(string path) {
        byte[] bytes = File.ReadAllBytes(path);
        Run(Encoding.UTF8.GetString(bytes));
        if (hadError) Environment.Exit(65);
    }

    private static void RunPrompt() {
        for (;;) {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line is null) break;
            Run(line);
            hadError = false;
        }
    }

    private static void Run(string source) {
        Scanner scanner = new Scanner();
        List<Token> tokens = scanner.ScanTokens();

        foreach (Token token in tokens) {
            Console.WriteLine(token);
        }
    }

    internal static void Error(int line, string message) {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message) {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        hadError = true;
    }
}