namespace LoxSharp;

public class LoxFunction : ICallable
{
    private readonly Stmt.Function declaration;
    private readonly LoxEnvironment closure;
    public int Arity => declaration.parameters.Count;

    public LoxFunction(Stmt.Function declaration, LoxEnvironment closure){
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
            return returnValue.Value;
        }

        return null;
    }

    public override string ToString(){
        return $"<fn {declaration.name.lexeme}>";
    }


}