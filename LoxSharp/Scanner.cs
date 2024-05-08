


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

    public class Scanner : IScanner {
        private readonly string source;
        private readonly List<IToken> tokens = new List<IToken>();

        private int start = 0;
        private int current = 0;
        private int line = 1;

        private bool IsAtEnd {get{return current >= source.Length;}}
        
        public Scanner(string input){
            source = input;
        } 

        public List<IToken> scanTokens(){
            while(!IsAtEnd){
                start = current;
                scanToken(); 
            }
            tokens.Add(new Token(TokenType.EOF,"",null,line));
            return tokens;
        }

        private void scanToken()
        {
            char c = advance();

            switch(c){
                case '(': AddToken(TokenType.LEFT_PAREN);  break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE);  break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA);       break;
                case '.': AddToken(TokenType.DOT);         break;
                case '-': AddToken(TokenType.MINUS);       break;
                case '+': AddToken(TokenType.PLUS);        break;
                case ';': AddToken(TokenType.SEMICOLON);   break;
                case '*': AddToken(TokenType.STAR);        break;
            }
        }

        private char advance(){
            var c = source.ElementAt(current);
            current++;
            return c;
        }

        private void AddToken(TokenType type){
            AddToken(type,null);
        }

        private void AddToken(TokenType type, object? value)
        {
            String text = source.Substring(start,current);
            tokens.Add(new Token(type,text,value,line));
        }
    }
    
}