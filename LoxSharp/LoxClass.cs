

using System.ComponentModel;

namespace LoxSharp;

public class LoxClass : ICallable
{
    public readonly string name;
    private readonly Dictionary<string,LoxFunction> methods;


        public LoxClass(string name, Dictionary<string, LoxFunction> methods)
    {
        this.name = name;
        this.methods = methods;
    }


    public int Arity {get{
        LoxFunction init = FindMethod("init");
        if (init == null) return 0;
        return init.Arity;

    } }

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        LoxInstance instance = new LoxInstance(this);

        LoxFunction initializer = FindMethod("init");
        if(initializer != null){
            initializer.Bind(instance).Call(interpreter, arguments);
        }

        
        return instance;
    }

    public override string ToString(){
        return name;
    }

    internal LoxFunction FindMethod(string name)
    {
        if(methods.ContainsKey(name)){
            return methods[name];
        }

        return null;
    }
}
