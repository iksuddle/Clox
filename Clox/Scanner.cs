using System.Collections.Generic;

namespace Clox; 

internal class Scanner {
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    
    private int start = 0;
    private int current = 0;
    private int line = 1;
    
    internal Scanner(string source) {
        this.source = source;
    }

    internal List<Token> ScanTokens() {
        while (!IsAtEnd()) {
            // beginning of next lexeme
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.Eof, "", null, line));
        return tokens;
    }

    private bool IsAtEnd() {
        return current >= source.Length;
    }
}