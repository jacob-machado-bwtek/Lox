namespace LoxSharp;


public interface IToken {

    public TokenType type { get;}

    public string lexeme {get;}

    public Object? literal {get;}

    public int line {get;}
    string ToString();


}
    

public class Token : IToken {
    public TokenType type {get;}
    public  string lexeme {get;}
    public object? literal
    {
        get;
        private set;
    }
    public  int line {get;}

    public Token(TokenType type, string lexeme, Object? literal, int line) {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }


    public override string ToString(){
        var s = $"{type} {lexeme} {literal}";
        return s;
    }

}




    
