
namespace LoxSharp;

public class Interpreter : Expr.Visitor<Object> {

 public Interpreter() {}


    public void Interpret(Expr expression){
        try{
            Object value = Evaluate(expression);
            Console.WriteLine(Stringify(value));
        } catch(RuntimeError re) {
            Lox.runtimeError(re);
        }
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
}