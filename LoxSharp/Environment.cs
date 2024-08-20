namespace LoxSharp;

public class LoxEnvironment {
    private readonly Dictionary<string,object> values = new Dictionary<string,object>();

    private readonly LoxEnvironment enclosing;

    public LoxEnvironment(){
        enclosing = null;
    }
    public LoxEnvironment(LoxEnvironment enclosing){
        this.enclosing = enclosing;
    }

    public Object Get(Token name){
        if(values.ContainsKey(name.lexeme)){
            return values[name.lexeme];
        }

        if(enclosing != null){
            return enclosing.Get(name);
        }

        throw new RuntimeError(name, $" Undefined variable '{name.lexeme}.'");
    }

    public void Define(string name, object value) {
        values.Add(name, value);
    }

    public void Assign(Token name, Object value) {
        if(values.ContainsKey(name.lexeme)) {
            values[name.lexeme] = value;
            return;
        }
        if (enclosing != null){
            enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeError(name, $"Undefined variable {name.lexeme}.");
    }

    public object GetAt(int distance, string name)
    {
        return Ancestor(distance).values[name];
    }

    private LoxEnvironment Ancestor(int distance)
    {
        LoxEnvironment environment = this;
        for(int i = 0; i < distance; i++){
            environment = environment.enclosing;
        }

        return environment;
    }

    internal void AssignAt(int distance, Token name, object value)
    {
        Ancestor(distance).values[name.lexeme] = value;
    }
}