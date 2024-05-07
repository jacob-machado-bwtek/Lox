namespace LoxSharp
{
    public interface IScanner {
       
       public List<Token> scanTokens();
    }

    public class DummyScanner : IScanner{
        public DummyScanner(string input){

        }

        public List<Token> scanTokens(){
            return new List<Token>();
        }
    }
    
}