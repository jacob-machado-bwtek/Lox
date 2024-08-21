
namespace LoxSharp;

public class Resolver : Expr.IVisitor<object>, Stmt.IVisitor<object>
{
    private readonly Interpreter _interpreter;
    private readonly Stack<Dictionary<string, bool>> _scopes = new();

    private FunctionType currentFunction = FunctionType.NONE;

    private ClassType currentClass = ClassType.NONE;

    public Resolver(Interpreter interpreter)
    {
        _interpreter = interpreter;
    }

    private enum FunctionType
    {
        NONE,
        FUNCTION,

        METHOD,

        INITIALIZER,
    }

    public object visitAssignExpr(Expr.Assign expr)
    {
        Resolve(expr.value);
        resolveLocal(expr, expr.name);
        return null;
    }

    public object visitBinaryExpr(Expr.Binary expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null;
    }

    public object visitBlockStmt(Stmt.Block stmt)
    {
        BeginScope();
        Resolve(stmt.statements);
        EndScope();
        return null;
    }

    private void EndScope()
    {
        _scopes.Pop();
    }

    private void BeginScope()
    {
        _scopes.Push(new Dictionary<string, bool>());
    }

    public void Resolve(List<Stmt> statements)
    {
        foreach (Stmt stmt in statements)
        {
            Resolve(stmt);
        }
    }

    public void Resolve(Stmt stmt)
    {
        stmt.accept(this);
    }
    public void Resolve(Expr expr)
    {
        expr.accept(this);
    }

    public object visitCallExpr(Expr.Call expr)
    {
        throw new NotImplementedException();
    }

    public object visitExpressionStmt(Stmt.Expression stmt)
    {
        Resolve(stmt.expression);
        return null;
    }

    public object visitFunctionStmt(Stmt.Function stmt)
    {
        Declare(stmt.name);
        Define(stmt.name);

        ResolveFunction(stmt, FunctionType.FUNCTION);
        return null;
    }

    private void ResolveFunction(Stmt.Function stmt, FunctionType functionType)
    {
        FunctionType enclosingFunction = currentFunction;
        currentFunction = functionType;

        BeginScope();

        foreach (Token parameter in stmt.parameters)
        {
            Declare(parameter);
            Define(parameter);
        }

        Resolve(stmt.body);
        EndScope();
        currentFunction = enclosingFunction;
    }

    public object visitGroupingExpr(Expr.Grouping expr)
    {
        Resolve(expr.expression);
        return null;
    }

    public object visitIfStmt(Stmt.If stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.thenBranch);
        if (stmt.elseBranch != null) Resolve(stmt.elseBranch);
        return null;
    }

    public object visitLiteralExpr(Expr.Literal expr)
    {
        return null;
    }

    public object visitLogicalExpr(Expr.Logical expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null;
    }

    public object visitPrintStmt(Stmt.Print stmt)
    {
        Resolve(stmt.expression);
        return null;
    }

    public object visitReturnStmt(Stmt.Return stmt)
    {
        if (currentFunction == FunctionType.NONE)
        {
            Lox.Error(stmt.keyword, "Can't return from top-level code.");
        }

        if (stmt.value != null)
        {
            if(currentFunction == FunctionType.INITIALIZER){
                Lox.Error(stmt.keyword,"Can't return a value from an initializer.");
            }

            Resolve(stmt.value);
        }

        return null;
    }

    public object visitUnaryExpr(Expr.Unary expr)
    {
        Resolve(expr.right);
        return null;
    }

    public object visitVariableExpr(Expr.Variable expr)
    {
        if (_scopes.Count != 0 && _scopes.Peek()[expr.name.lexeme] == false)
        {
            Lox.Error(expr.name, $"Can't read local variable in its own initializer.");
        }

        resolveLocal(expr, expr.name);
        return null;
    }

    private void resolveLocal(Expr expr, Token name)
    {
        for (int i = _scopes.Count - 1; i >= 0; i--)
        {
            if (_scopes.ToArray()[i].ContainsKey(name.lexeme))
            {
                _interpreter.Resolve(expr, _scopes.Count - 1 - i);
                return;
            }
        }
    }

    public object visitVarStmt(Stmt.Var stmt)
    {
        Declare(stmt.name);
        if (stmt.initializer != null)
        {
            Resolve(stmt.initializer);
        }
        Define(stmt.name);
        return null;
    }

    private void Define(Token name)
    {
        if (_scopes.Count == 0) return;

        _scopes.Peek()[name.lexeme] = true;
    }

    private void Declare(Token name)
    {
        if (_scopes.Count == 0) return;

        Dictionary<string, bool> scope = _scopes.Peek();

        if (scope.ContainsKey(name.lexeme))
        {
            Lox.Error(name, "Already a variable with this name in this scope");
        }
        scope[name.lexeme] = true;
    }

    public object visitWhileStmt(Stmt.While stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.body);
        return null;
    }

    public object visitClassStmt(Stmt.Class stmt)
    {
        ClassType enclosingClass = currentClass;
        currentClass = ClassType.CLASS;

        Declare(stmt.name);
        Define(stmt.name);


        BeginScope();
        _scopes.Peek()["this"] = true;

        foreach (Stmt.Function method in stmt.methods)
        {
            FunctionType declaration = FunctionType.METHOD;

            if(method.name.lexeme.Equals("init")){
                declaration = FunctionType.INITIALIZER;
            }

            ResolveFunction(method, declaration);
        }

        EndScope();

        currentClass = enclosingClass;
        return null;
    }

    public object visitGetExpr(Expr.Get expr)
    {
        Resolve(expr.obj);
        return null;
    }

    public object visitSetExpr(Expr.Set expr)
    {
        Resolve(expr.value);
        Resolve(expr.value);
        return null;
    }

    public object visitThisExpr(Expr.This expr)
    {
        if(currentClass == ClassType.NONE){
            Lox.Error(expr.keyword,"Can't use 'this' outside of a class.");
            return null;
        }


        resolveLocal(expr, expr.keyword);
        return null;
    }

    private enum ClassType
    {
        NONE,
        CLASS,
    }
}

