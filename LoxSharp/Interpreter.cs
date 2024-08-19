
using System.Security.Cryptography.X509Certificates;

namespace LoxSharp;

public class Interpreter : Expr.IVisitor<Object>, Stmt.IVisitor<Object> {

    private LoxEnvironment environment = new LoxEnvironment();
 public Interpreter() {}


    public void Interpret(List<Stmt> stmts){
        try{
            foreach (Stmt statement in stmts){
                Execute(statement);
            }
        } catch (RuntimeError error) {
            Lox.runtimeError(error);
        }
        
    }

    private void Execute(Stmt statement)
    {
        statement.accept(this);
    }

    private string Stringify(object value)
    {
        if(value == null) return "nil";

        if(value.GetType() == typeof(double)){
            string text = value.ToString();

            if(text.EndsWith(".0")){
                text = text.Substring(0, text.Length-2);
            }

            return text;
        }

        return value.ToString();
        
    }

    public object visitBinaryExpr(Expr.Binary expr)
    {
        object left = Evaluate(expr.left);
        object right = Evaluate(expr.right);

        switch (expr.Op.type) {
            case TokenType.GREATER:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left > (double)right;

            case TokenType.GREATER_EQUAL:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left >= (double)right;

            case TokenType.LESS:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left < (double)right;

            case TokenType.LESS_EQUAL:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left <= (double)right;   

            case TokenType.MINUS:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left - (double)right;

            case TokenType.SLASH:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left / (double)right;

            case TokenType.STAR:
                CheckNumberOperand(expr.Op,left,right);
                return (double)left * (double)right;

            case TokenType.BANG_EQUAL:
                return !IsEqual(left,right);

            case TokenType.EQUAL_EQUAL:
                return IsEqual(left,right);
            
            
            case TokenType.PLUS:
                if(left.GetType() == typeof(double) && right.GetType() == typeof(double)) {
                    //numeric addition
                    return (double)left + (double)right;
                }

                if(left.GetType() == typeof(string) && right.GetType() == typeof(string)){
                    //string concatonation 
                    return (string)left + (string)right;
                }

                if(left.GetType() == typeof (string) && right.GetType() == typeof(double)){
                    return (string)left + right.ToString();
                }
                if(left.GetType() == typeof(double) && right.GetType() == typeof(string)){
                    return left.ToString() + (string)right;
                }

                //throw new RuntimeError(expr.Op, "operands must be two numbers or two strings");
                break;
                
                
        }

        //unreachable, probably
        return null;
    }

    private void CheckNumberOperand(Token op, object left, object right)
    {
        if(left.GetType() == typeof(double) && right.GetType() == typeof (double)) {
            return;
        }

        throw new RuntimeError(op,"Operands must be numbers");
    }

    private bool IsEqual(object left, object right)
    {
        //Lox does not follow IEEE 754: NaN != NaN
        if(left == null && right == null) return true;

        if(left == null) return false;

        return left.Equals(right);
    }

    public object visitGroupingExpr(Expr.Grouping expr)
    {
        return Evaluate(expr);
    }

    private object Evaluate(Expr expr)
    {
        return expr.accept(this);
    }

    public object visitLiteralExpr(Expr.Literal expr)
    {
        return expr.value;
    }

    public object visitUnaryExpr(Expr.Unary expr)
    {
        object right = Evaluate(expr.right);

        switch (expr.op.type){
            case TokenType.BANG:
                return !isTruthy(right);
            case TokenType.MINUS:
                CheckNumberOperand(expr.op,right);
                return -(double)right;
            
        }


        //unreachable, probably
        return null;
    }

    private void CheckNumberOperand(Token op, object right)
    {
        if(op.GetType() == typeof(double)){
            return;
        }
        throw new RuntimeError(op,"Operand must be a number");
    }

    private bool isTruthy(object @object)
    {
        if(@object == null){
            return false;
        }
        if(@object.GetType() == typeof(bool)){
            return (bool)@object;
        }

        return true;
    }

    public object visitBinaryStmt(Expr.Binary stmt)
    {
        throw new NotImplementedException();
    }

    public object visitGroupingStmt(Expr.Grouping stmt)
    {
        throw new NotImplementedException();
    }

    public object visitLiteralStmt(Expr.Literal stmt)
    {
        throw new NotImplementedException();
    }

    public object visitUnaryStmt(Expr.Unary stmt)
    {
        throw new NotImplementedException();
    }

    public object visitExpressionStmt(Stmt.Expression stmt)
    {
        Evaluate(stmt.expression);
        return null;
    }

    public object visitPrintStmt(Stmt.Print stmt)
    {
        Object val = Evaluate(stmt.expression);
        Console.WriteLine(Stringify(val));
        return null;
    }

    public object visitVariableStmt(Expr.Variable stmt)
    {
        throw new NotImplementedException();
    }

    public object visitVarStmt(Stmt.Var stmt)
    {
        Object val = null;
        if(stmt.initializer != null){
            val = Evaluate(stmt.initializer);
        }

        environment.Define(stmt.name.lexeme,val);
        return null; 
    }

    public object visitVariableExpr(Expr.Variable expr)
    {
        return environment.Get(expr.name);
    }

    public object visitAssignExpr(Expr.Assign expr)
    {
        Object value = Evaluate(expr.value);
        environment.Assign(expr.name,value);
        return null;
    }

    public object visitBlockStmt(Stmt.Block stmt)
    {
        ExecuteBlock(stmt.statements, new LoxEnvironment(environment));
        return null;
    }

    private void ExecuteBlock(List<Stmt> statements, LoxEnvironment environment)
    {
        LoxEnvironment prev = this.environment;

        try{
            this.environment = environment;

            foreach(Stmt stmt in statements){
                Execute(stmt);
            }
        }finally{
            this.environment = prev;
        }
    }

    public object visitIfStmt(Stmt.If stmt)
    {
        if(isTruthy(Evaluate(stmt.condition))){
            Execute(stmt.thenBranch);
        }else if(stmt.elseBranch != null){
            Execute(stmt.elseBranch);
        }

        return null;
    }

    public object visitLogicalExpr(Expr.Logical expr)
    {
        Object left = Evaluate(expr.left);

        if(expr.Op.type == TokenType.OR){
            if(isTruthy(left)){
                return left;
            }
        } else{
            if(!isTruthy(left)){
                return left;
            }
        }
        return Evaluate(expr.right);
    }

    public object visitWhileStmt(Stmt.While stmt)
    {
        while(isTruthy(Evaluate(stmt.condition))) {
            Execute(stmt.body);
        }
        return null;
    }
}