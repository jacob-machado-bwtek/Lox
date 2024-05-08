using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using CraftingInterpreters.LoxSharp;

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

                //single character operators
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

                //two character operators
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);       break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);     break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);       break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;

                //whitespace
                case ' ':
                case '\t':
                case '\r':
                
                break;


                //newline
                case '\n': line++; break;

                case '/':
                    if(Match('/')){
                        while(Peek() != '\n' && !IsAtEnd){
                            advance();
                        }
                    }
                    //handle block comments
                    if(Match('*')){
                        bool endComment = (Peek() == '*' && peekNext() == '/');
                        while(!endComment&& !IsAtEnd){
                            endComment = (Peek() == '*' && peekNext() == '/');
                            if(Peek() == '\n'){
                                line++;
                            }
                            advance();
                        } 

                    }
                    else{
                        AddToken(TokenType.SLASH);
                    }
                    
                break;




                case '"': StringHandler(); break;



                default: 
                    if(char.IsLetter(c)){
                        number();
                    }
                    else if(char.IsLetter(c) || c=='_'){
                        Identifier();
                    }
                    else{
                        Lox.error(line, "Unexpected Character"); 
                    }
                break;

            }
        }

        private void Identifier()
        {
            while(char.IsLetterOrDigit(Peek()) || Peek() == '_'){
                advance();
            }
            string text = source.Substring(start, current-start);
            TokenType type;
            
            bool hit = LoxKeywords.KeywordsDict.TryGetValue(text, out type);

            if(!hit){
                type = TokenType.IDENTIFIER;
            }
            AddToken(type);
        }

        private void number()
        {
            while(char.IsDigit(Peek())){
                advance();
            }
            if(Peek() == '.' && char.IsDigit(peekNext())){

                advance();

                while(char.IsDigit(Peek())){
                    advance();
                }
            }

            double x = Double.Parse(source.Substring(start, current-start));
            AddToken(TokenType.NUMBER, x);

        }

        private char peekNext()
        {
            if(current +1 >= source.Length){
                return '\0';
            }
            return source.ElementAt(current+1);
        }

        private void StringHandler()
        {
            while(Peek() != '"' && !IsAtEnd){
                if(Peek() == '\n'){
                    line++;
                }
                advance();
            }

            if(IsAtEnd){
                //TODO: remember starting position of unterminated string for debugging
                Lox.error(line, "Unterminated String");
                return;
            }

            advance();

            String value = source.Substring(start+1,current-start-1);
            AddToken(TokenType.STRING,value);
            
        }

        private char Peek()
        {
            if(IsAtEnd) return '\0';
            return source.ElementAt(current);

        }

        private bool Match(char expected)
        {
            if(IsAtEnd) return false;

            if(source.ElementAt(current) != expected) return false; 

            current++;
            return true;
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
            String text = source.Substring(start,current-start);
            tokens.Add(new Token(type,text,value,line));
        }
    }
    
}