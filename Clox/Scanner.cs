using System.Collections.Generic;

namespace Clox; 

internal class Scanner {
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    
    private int start;
    private int current;
    
    private int line = 1;

    private static readonly Dictionary<string, TokenType> keyWords = new Dictionary<string, TokenType>() {
        { "and", TokenType.And },
        { "class", TokenType.Class },
        { "else", TokenType.Else },
        { "false", TokenType.False },
        { "for", TokenType.For },
        { "fun", TokenType.Fun },
        { "if", TokenType.If },
        { "nil", TokenType.Nil },
        { "or", TokenType.Or },
        { "print", TokenType.Print },
        { "return", TokenType.Return },
        { "super", TokenType.Super },
        { "this", TokenType.This },
        { "true", TokenType.True },
        { "var", TokenType.Var },
        { "while", TokenType.While }
    };
    
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

    private void ScanToken() {
        char c = Advance();

        switch (c) {
            case '(':
                AddToken(TokenType.LeftParen);
                break;
            case ')':
                AddToken(TokenType.RightParen);
                break;
            case '{':
                AddToken(TokenType.LeftBrace);
                break;
            case '}':
                AddToken(TokenType.RightBrace);
                break;
            case ',':
                AddToken(TokenType.Comma);
                break;
            case '.':
                AddToken(TokenType.Dot);
                break;
            case '-':
                AddToken(TokenType.Minus);
                break;
            case '+':
                AddToken(TokenType.Plus);
                break;
            case ';':
                AddToken(TokenType.Semicolon);
                break;
            case '*':
                AddToken(TokenType.Star);
                break;
            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;
            case '/':
                if (Match('/')) {
                    while (Peek() != '\n' && !IsAtEnd())
                        Advance();
                } else {
                    AddToken(TokenType.Slash);
                }
                break;
            // ignore whitespaces
            case ' ':
                break;
            case '\r':
                break;
            case '\t':
                break;
            case '\n':
                line++;
                break;
            // strings
            case '"':
                ScanString();
                break;
            default:
                if (IsDigit(c)) {
                    ScanNumber();
                } else if (IsAlpha(c)) {
                    ScanIdentifier();
                } else {
                    Clox.Error(line, "Unexpected character.");
                }
                break;
        }
    }

    private void ScanIdentifier() {
        while (IsAlphaNumeric(Peek())) Advance();

        string text = source[start..current];
        if (!keyWords.TryGetValue(text, out TokenType type)) {
            type = TokenType.Identifier;
        }
        
        AddToken(type);
    }
    
    private void ScanNumber() {
        while (IsDigit(Peek())) Advance();
        
        // look for fractional part
        if (Peek() == '.' && IsDigit(PeekNext())) {
            Advance();

            while (IsDigit(Peek())) Advance();
        }

        double value = double.Parse(source[start..current]);
        AddToken(TokenType.Number, value);
    }

    private void ScanString() {
        while (Peek() != '"' && !IsAtEnd()) {
            if (Peek() == '\n') line++;
            Advance();
        }

        if (IsAtEnd()) {
            Clox.Error(line, "Unterminated string.");
            return;
        }
        
        Advance();

        string value = source[(start+1)..(current-1)];
        AddToken(TokenType.String, value);
    }

    private bool Match(char expected) {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private char Peek() {
        return IsAtEnd() ? '\0' : source[current];
    }

    private char PeekNext() {
        return current + 1 >= source.Length ? '\0' : source[current + 1];
    }
    
    private char Advance() {
        return source[current++];
    }

    private void AddToken(TokenType type, object? literal = null) {
        string text = source[start..current];
        tokens.Add(new Token(type, text, literal, line));
    }

    private bool IsDigit(char c) {
        return c is >= '0' && c <= '9';
    }

    private bool IsAlpha(char c) {
        return (c >= 'a' && c <= 'z') ||
               (c >= 'A' && c <= 'Z') ||
               (c == '_');
    }

    private bool IsAlphaNumeric(char c) {
        return IsAlpha(c) || IsDigit(c);
    }
    
    private bool IsAtEnd() {
        return current >= source.Length;
    }
}