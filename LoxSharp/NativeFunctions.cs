
using LoxSharp;

namespace LoxSharp.NativeFunctions;


class Clock : ICallable
{
    public int Arity {get{return 0;}}

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        return (double)DateTime.UtcNow.Millisecond/1000.0;
    }

    public override string ToString(){
        return "<native fn>";
    }

}