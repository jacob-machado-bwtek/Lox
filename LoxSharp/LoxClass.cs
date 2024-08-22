

using System.ComponentModel;

namespace LoxSharp;

public class LoxClass : ICallable
{
    public readonly string name;
    private readonly Dictionary<string,LoxFunction> methods;

    private readonly LoxClass superclass;


    public LoxClass(string name,LoxClass superclass, Dictionary<string, LoxFunction> methods)
    {
        this.name = name;
        this.superclass = superclass;
        this.methods = methods;
    }


    public int Arity {
        get{
        LoxFunction init = FindMethod("init");
        if (init == null) return 0;
        return init.Arity;
        } 
    }

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

        if(superclass != null){
            return superclass.FindMethod(name);
        }

        return null;
    }
}
