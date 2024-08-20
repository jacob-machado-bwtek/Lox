namespace LoxSharp;

[Serializable]
internal class Return : LoxError
{
    public object Value {get; set;}

    public Return() : base() { 

    }
    public Return(string message) : base(message) {

    }   

    public Return(string message, Exception innerException) : base(message,innerException){

    }
    public Return(object value){
        this.Value = value;
    }
}