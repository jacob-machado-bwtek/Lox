namespace LoxSharp;

class LoxEnvironment {
    private readonly Dictionary<string,object> values = new Dictionary<string,object>();


    public Object Get(Token name){
        if(values.ContainsKey(name.lexeme)){
            return values[name.lexeme];
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

        throw new RuntimeError(name, $"Undefined variable {name.lexeme}.");
    }

}