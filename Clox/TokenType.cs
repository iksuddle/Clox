namespace Clox;

internal enum TokenType {
    // one character tokens
    LeftParen,  RightParen, LeftBrace, RightBrace,
    Comma, Dot, Minus, Plus, Semicolon, Slash, Star,
    
    // two character tokens
    Bang, BangEqual, 
    Equal, EqualEqual, 
    Greater, GreaterEqual, 
    Less, LessEqual,
    
    // literals
    Identifier, String, Number,
    
    // keywords
    And, Class, Else, False, Fun, For, If, Nil, Or,
    Print, Return, Super, This, True, Var, While,
    
    Eof
}