

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LoxSharp;

public partial class Parser{
    private List<Token> tokens;

    private int current = 0;
    private bool IsAtEnd{get{return Peek().type == TokenType.EOF;}}


    public Expr Parse(){
        try{
            return Expression();
        }catch(ParseError e){
            return null;
        }
    }

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
        Expr expr = Comparison();


        while(Match(TokenType.BANG_EQUAL,TokenType.EQUAL_EQUAL)){
            Token op = Previous();
            Expr right = Comparison();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private Token Previous()
    {
        return tokens.ElementAt(current -1);
    }

    private Expr Comparison()
    {
        Expr expr = Term();

        while(Match(TokenType.GREATER,TokenType.GREATER_EQUAL,TokenType.LESS, TokenType.LESS_EQUAL)){
            Token op = Previous();
            Expr right = Term();
            expr = new Expr.Binary(expr, op, right);

        }

        return expr;
    }

    private Expr Term()
    {
        Expr expr = Factor();

        while(Match(TokenType.MINUS,TokenType.PLUS)){
            Token op = Previous();
            Expr right = Factor();
            expr = new Expr.Binary(expr, op, right);

        }
        return expr;
    }

    private Expr Factor()
    {
       Expr expr = Unary();
        while(Match(TokenType.SLASH,TokenType.STAR)){
            Token op = Previous();
            Expr right = Unary();
            expr = new Expr.Binary(expr, op, right);
        }

       return expr;
    }

    private Expr Unary()
    {
        if(Match(TokenType.BANG, TokenType.MINUS)){
            Token op = Previous();
            Expr right = Unary();
            return new Expr.Unary(op, right);
        }

        return Primary();
    }

    private Expr Primary()
    {
        if(Match(TokenType.FALSE)){
            return new Expr.Literal(false);
        }
        if(Match(TokenType.TRUE)){
            return new Expr.Literal(true);
        }
        if(Match(TokenType.NIL)){
            return new Expr.Literal(null);
        }

        if(Match(TokenType.NUMBER,TokenType.STRING)){
            return new Expr.Literal(Previous().literal);
        }

        if(Match(TokenType.LEFT_PAREN)){
            Expr expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
            return new Expr.Grouping(expr);
        }

           throw Error(Peek(), "Expect expression.");
    }

    private Token Consume(TokenType type, string message)
    {
        if(Check(type)){
            return Advance();
        }
        throw Error(Peek(), message);
    }

    private ParseError Error(Token token, string message){
        Lox.Error(token, message);
        return new ParseError();
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

    private void Synchronize(){
        Advance(); 

        while(!IsAtEnd){
            if(Previous().type == TokenType.SEMICOLON){
                return;
            }

            switch(Peek().type) {

                case TokenType.CLASS:
                case TokenType.FUN:
                case TokenType.VAR:
                case TokenType.FOR:
                case TokenType.WHILE:
                case TokenType.IF:
                case TokenType.PRINT:
                case TokenType.RETURN:

                return;
            }

            Advance();
        }
    }
}