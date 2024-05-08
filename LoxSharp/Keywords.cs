namespace LoxSharp;

static class LoxKeywords{
    public static Dictionary<string,TokenType> KeywordsDict = new Dictionary<string,TokenType>();
    static LoxKeywords(){
        KeywordsDict.Add("and", TokenType.AND);
        KeywordsDict.Add("class", TokenType.CLASS);
        KeywordsDict.Add("else", TokenType.ELSE);
        KeywordsDict.Add("false", TokenType.FALSE);
        KeywordsDict.Add("true", TokenType.TRUE);
        KeywordsDict.Add("nil", TokenType.NIL);
        KeywordsDict.Add("for", TokenType.FOR);
        KeywordsDict.Add("fun", TokenType.FUN);
        KeywordsDict.Add("or", TokenType.OR);
        KeywordsDict.Add("if", TokenType.IF);
        KeywordsDict.Add("print", TokenType.PRINT);
        KeywordsDict.Add("return", TokenType.RETURN);
        KeywordsDict.Add("super", TokenType.SUPER);
        KeywordsDict.Add("this", TokenType.THIS);
        KeywordsDict.Add("var", TokenType.VAR);
        KeywordsDict.Add("while", TokenType.WHILE);

    }
}