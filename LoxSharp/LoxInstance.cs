
namespace LoxSharp;

public class LoxInstance
{
    private LoxClass klass;

    private readonly Dictionary<string, object> _fields = new();

    public LoxInstance(LoxClass klass){
        this.klass = klass;
    }


    public override string ToString()
    {
        return $"{klass.name} instance";
    }

    public object Get(Token name){
        if(_fields.ContainsKey(name.lexeme)){
            return _fields[name.lexeme];    
        }

        LoxFunction method = klass.FindMethod(name.lexeme);
        if(method != null) return method.Bind(this);

        throw new RuntimeError(name, $"Undefined property '{name.lexeme}'.");
    }

    public void Set(Token name, object value){
        _fields[name.lexeme] = value;
    }
}