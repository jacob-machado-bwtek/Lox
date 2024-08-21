
namespace LoxSharp;

public class LoxFunction : ICallable
{
    private readonly Stmt.Function declaration;
    private readonly LoxEnvironment closure;
    public int Arity => declaration.parameters.Count;

    public bool isInitializer;

    public LoxFunction(Stmt.Function declaration, LoxEnvironment closure, bool isInitializer){
        this.isInitializer = isInitializer;
        this.declaration = declaration;
        this.closure = closure;
    }

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        
        LoxEnvironment env = new LoxEnvironment(closure);

        for(int i = 0;i < declaration.parameters.Count; i++){
            env.Define(declaration.parameters[i].lexeme, arguments[i]);            
        }

        try{
            interpreter.ExecuteBlock(declaration.body, env);
        }catch(Return returnValue){
            if(isInitializer) return closure.GetAt(0, "this");
            return returnValue.Value;
        }

        if(isInitializer) return closure.GetAt(0, "this");
        return null;
    }

    public override string ToString(){
        return $"<fn {declaration.name.lexeme}>";
    }

    internal LoxFunction Bind(LoxInstance loxInstance)
    {
        var env = new LoxEnvironment(closure);
        env.Define("this", loxInstance);

        return new LoxFunction(declaration, env, isInitializer);
    }
}