namespace LoxSharp;

public interface ICallable
{
    public Object Call(Interpreter interpreter, List<object> arguments);

    public int Arity {get; }
}