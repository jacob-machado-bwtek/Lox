namespace LoxSharp
{
    public interface IScanner {
       
       /// <summary>
       /// Scans the inputted string
       /// </summary>
       /// <returns>A list of tokens</returns>
       public List<IToken> scanTokens();
    }

    public class DummyScanner : IScanner{
        public DummyScanner(string input){

        }

        /// <summary>
        /// Scans the inputted string
        /// </summary>
        /// <returns>an empty list of Dummy Tokens</returns>
        public List<IToken> scanTokens(){
            return new List<IToken>();
        }
    }
    
}