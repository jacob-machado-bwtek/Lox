

namespace LoxSharp;

public class Parser{
    private List<Token> tokens;

    private int current = 0;
    private bool IsAtEnd{get{return Peek().type == TokenType.EOF;}}

    private Token Peek()
    {
        return tokens.ElementAt(current);
    }

    public Parser(List<Token> tokens){
        this.tokens = tokens;
    }

    private Expr Expression(){
        return Equality();
    }

    private Expr Equality()
    {
        Expr expr = ExprComparison();


        while(Match(TokenType.BANG_EQUAL,TokenType.EQUAL_EQUAL)){
            Token op = Previous();
            Expr right = ExprComparison();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private Token Previous()
    {
        return tokens.ElementAt(current);
    }

    private Expr ExprComparison()
    {
        throw new NotImplementedException();
    }

    private bool Match(params TokenType[] tokens)
    {
        foreach(TokenType t in tokens){
            if(Check(t)){
                Advance();
                return true;
            }
        }

        return false;
    }

    private Token Advance()
    {
        if(!IsAtEnd){
            current++;
        }
        return Previous();
    }

    private bool Check(TokenType t)
    {
        if(IsAtEnd){
            return false;
        }
        return Peek().type == t;
    }
}