
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace LoxSharp;

public class Interpreter : Expr.IVisitor<Object>, Stmt.IVisitor<Object> {

    public readonly LoxEnvironment globals = new LoxEnvironment();
    private readonly Dictionary<Expr,int> _locals = new();
    private LoxEnvironment _environment;
 public Interpreter() {
    _environment = globals;

   DefineNativeFunctions();
 }

    private void DefineNativeFunctions()
    {
        globals.Define("clock", new NativeFunctions.Clock());

    }

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

        _environment.Define(stmt.name.lexeme,val);
        return null; 
    }

    public object visitVariableExpr(Expr.Variable expr)
    {
        return LookUpVariable(expr.name,expr);
    }

    private object LookUpVariable(Token name, Expr expr)
    {
        int distance = _locals[expr];

        if(distance != null){
            return _environment.GetAt(distance,name.lexeme);
        }else{
            return globals.Get(name);
        }
    }

    public object visitAssignExpr(Expr.Assign expr)
    {
        Object value = Evaluate(expr.value);

        int distance = _locals[expr];
        if(distance !=null){
            _environment.AssignAt(distance,expr.name,value);
        }else{
            globals.Assign(expr.name,value);
        }

        _environment.Assign(expr.name,value);
        return null;
    }

    public object visitBlockStmt(Stmt.Block stmt)
    {
        ExecuteBlock(stmt.statements, new LoxEnvironment(_environment));
        return null;
    }

    public void ExecuteBlock(List<Stmt> statements, LoxEnvironment environment)
    {
        LoxEnvironment prev = this._environment;

        try{
            this._environment = environment;

            foreach(Stmt stmt in statements){
                Execute(stmt);
            }
        }finally{
            this._environment = prev;
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

    public object visitCallExpr(Expr.Call expr)
    {
        Object callee = Evaluate(expr.callee);

        List<object> args = new List<object>();

        foreach(Expr arg in expr.arguments){
            args.Add(Evaluate(arg));
        }

        if(callee is not ICallable){
            throw new RuntimeError(expr.paren, "can only call functions and classes");
        }

        ICallable function = (ICallable)callee;

        if(args.Count != function.Arity){
            throw new RuntimeError(expr.paren, $"Expected {function.Arity} arguments but got {args.Count}.");
        }

        return function.Call(this, args);
    }

    public object visitFunctionStmt(Stmt.Function stmt)
    {
        LoxFunction function= new LoxFunction(stmt, _environment, false);

        _environment.Define(stmt.name.lexeme, function);
        return null;
    }

    public object visitReturnStmt(Stmt.Return stmt)
    {
        object val = null;

        if(stmt.value != null) val = Evaluate(stmt.value);

        throw new Return(val);
    }

    internal void Resolve(Expr expr, int depth)
    {
        _locals[expr]=depth;
    }

    public object visitClassStmt(Stmt.Class stmt)
    {
        object superclass = null;
        if(stmt.superclass != null) {
            superclass = Evaluate(stmt.superclass);
            if(superclass is not LoxClass){
                throw new RuntimeError(stmt.superclass.name,"Superclass must be a class");
            }
        }

        _environment.Define(stmt.name.lexeme, null);

        if(stmt.superclass != null){
            _environment = new LoxEnvironment(_environment);
            _environment.Define("super",superclass);
        }

        Dictionary<string, LoxFunction> methods = new();
        foreach(var method in stmt.methods){
            LoxFunction func = new LoxFunction(method, _environment, method.name.lexeme.Equals("init"));
            methods.Add(method.name.lexeme, func);
        }

        LoxClass klass = new LoxClass(stmt.name.lexeme,(LoxClass)superclass, methods);

        if(superclass != null){
            _environment = _environment.enclosing;
        }
        _environment.Assign(stmt.name, klass);
        return null;
    }

    public object visitGetExpr(Expr.Get expr)
    {
        object obj = Evaluate(expr.obj);
        if(obj is LoxInstance){
            return (obj as LoxInstance).Get(expr.name);
        }

        throw new RuntimeError(expr.name,"Only instances have properties");
    }

    public object visitSetExpr(Expr.Set expr)
    {
        object obj = Evaluate(expr.obj);

        if(obj is not LoxInstance){
            throw new  RuntimeError(expr.name,"Only instances have fields");
        }

        object value = Evaluate(expr.value);
        (obj as LoxInstance).Set(expr.name,value);

        return value;
    }

    public object visitThisExpr(Expr.This expr)
    {
        return LookUpVariable(expr.keyword, expr);
    }

    public object visitSuperExpr(Expr.Super expr)
    {
        int distance = _locals[expr];
        LoxClass superclass = (LoxClass)_environment.GetAt(distance, "super");

        LoxInstance obj = (LoxInstance)_environment.GetAt(distance-1, "this");
        
        LoxFunction method = superclass.FindMethod(expr.method.lexeme);

        if(method == null){
            throw new RuntimeError(expr.method,$"Undefined property '{expr.method.lexeme}'.");
        }

        return method.Bind(obj);
    }
}
