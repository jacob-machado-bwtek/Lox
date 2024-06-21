
﻿using System;
using System.Collections.Generic;
using System.Text;


namespace LoxSharp;


public abstract class LoxError : Exception{
    protected LoxError() : base()
    {}
    public LoxError(string message) : base(message){}

    public LoxError(string message, Exception innerException) : base(message, innerException){}
}
    
public class ParseError : LoxError {
    public ParseError(string message) :base(message){}

    public ParseError(string message, Exception innerException) :base(message, innerException){}

    public ParseError() : base(){}
}

public class RuntimeError : Exception {
    public readonly Token Token;

    public RuntimeError(Token token, string message) : base(message) {
        this.Token = token;
    }
} 