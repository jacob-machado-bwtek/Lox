
using System.Text;

namespace LoxSharp;

public class AstPrinter : Expr.IVisitor<string>{
    public string Print(Expr expr) {
        return expr.accept(this);
    }


     public string visitGroupingExpr(Expr.Grouping expr) {
        return parenthesize("group", expr.expression);
    }

    public string visitBinaryExpr(Expr.Binary expr)
    {
        return parenthesize(expr.Op.lexeme,expr.left,expr.right);
    }

    public string visitLiteralExpr(Expr.Literal expr)
    {
        if(expr.value == null){
            return "nil";
        }
        return expr.value.ToString() ?? "nil";
    }

    public string visitUnaryExpr(Expr.Unary expr)
    {
        return parenthesize(expr.op.lexeme,expr.right);
    }

    private string parenthesize(string name, params Expr[] exprs)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("(").Append(name);
        foreach(Expr e in exprs) {
            builder.Append(" ");
            builder.Append(e.accept(this));
        }
        builder.Append(")");

        return builder.ToString(); 
    }

    public string visitVariableExpr(Expr.Variable expr)
    {
        throw new NotImplementedException();
    }
}