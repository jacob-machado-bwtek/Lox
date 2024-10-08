use std::ops::Add;

use num_enum::{IntoPrimitive, TryFromPrimitive};


use crate::{
    chunk::{self, Chunk, OpCode},
    scanner::{Scanner, Token, TokenKind},
    types::Line,
    value::Value,
};


#[derive(Debug, PartialEq, Eq, PartialOrd, Ord, TryFromPrimitive, IntoPrimitive)]
#[repr(u8)]
enum Precedence {
    None,
    Assignment,
    Or,
    And,
    Equality,
    Comparison,
    Term,
    Factor,
    Unary,
    Call,
    Primary,

}

struct Rule {
    precedence: Precedence,
}

pub struct Compiler<'a> {
    scanner: Scanner<'a>,
    previous: Option<Token<'a>>,
    current: Option<Token<'a>>,
    had_error: bool,
    panic_mode: bool,
    chunk: Chunk,
}



impl<'a> Compiler<'a> {
    #[must_use]
    fn new(source: &'a [u8]) -> Self{
        Self {
            chunk: Chunk::new("<main>"),
            scanner: Scanner::new(source),
            previous: None,
            current: None,
            had_error: false,
            panic_mode: false,
        }
    }

    pub fn compile(source: &'a [u8]) -> Option<Chunk>{
        Self::new(source).compile_()
    }
    
    fn compile_(mut self) -> Option<Chunk> {
        self.advance();
        self.expression();
        self.consume(TokenKind::Eof, "Expect end of expression");
        self.end();
        if self.had_error {
            None
        } else {
            Some(self.chunk)
        }
    }
    
    fn advance(&mut self) {
        self.previous = std::mem::take(&mut self.current);
        loop {
            let token = self.scanner.scan();
            self.current = Some(token);
            if self.current.as_ref().unwrap().kind != TokenKind::Error {
                break;
            }


            #[allow(clippy::unnecessary_to_owned)]
            self.error_at_current(&self.current.as_ref().unwrap().as_str().to_string());
        }
    }
    
    fn consume(&mut self, kind: TokenKind, msg: &str) {
        if self.current.as_ref().map(|t| &t.kind) == Some(&kind){
            self.advance();
            return;
        }
        self.error_at_current(msg);
    }
    
    fn expression(&self)  {
        self.parse_precedence(Precedence::Assignment);
    }
    
    fn end(&mut self) {
        self.emit_return();
    }
    
    fn error_at_current(&mut self, msg: &str) {
        self.error_at(self.current.clone(), msg);
    }
    
    fn error_at(&mut self, token: Option<Token>, msg: &str) {

        if self.panic_mode {
            return;
        }

        self.panic_mode = true;
        if let Some(token) = token.as_ref() {
            eprint!("[line {}] Error", *token.line);
            
            if token.kind == TokenKind::Eof {
                eprint!("at end");
            } else if token.kind != TokenKind::Error {
                eprint!("at '{}'", token.as_str());
            }
        } 
    }
    
    fn emit_return(&mut self)  {
        self.emit_byte(OpCode::Return,self.line());
    }

    fn line(&self) -> Line {
        self.previous.as_ref().unwrap().line
    }
    
    fn emit_byte<T>(&mut self, byte: T, line: Line)
    where T: Into<u8>, {
        self.current_chunk().write(byte,line)
    }
    fn emit_bytes<T1,T2>(&mut self, byte1: T1, byte2: T2, line: Line) 
    where 
        T1: Into<u8>,
        T2: Into<u8>,
    {
        self.emit_byte(byte1, line);
        self.emit_byte(byte2, line);
        
    }

    fn emit_constant(&mut self, value: Value) {
        if !self.chunk.write_constant(value, self.line()){
            self.error("Too many constants in one chunk");
        }
    }
    
    fn current_chunk(&mut self) -> &mut Chunk {
        &mut self.chunk
    }
    
    fn error(&mut self, msg: &str) {
        self.error_at(self.previous.clone(), msg);
    }

    fn grouping(&mut self) {
        self.expression();
        self.consume(TokenKind::RightParen, "Expect ')' after expression");
    }

    fn number(&mut self){
        let value: f64 = self.previous.as_ref().unwrap().as_str().parse().unwrap();
        self.emit_constant(value);
    }
    
    fn unary(&mut self) {
        let operator = &self.previous.as_ref().unwrap().kind;
        let line = self.line();

        self.parse_precedence(Precedence::Unary);

        match operator {
            TokenKind::Minus => self.emit_byte(OpCode::Negate, line),
            _ => unreachable!("unary but not negation??: {}", operator),
        }
    }

    fn binary(&mut self) {
        let operator = &self.previous.as_ref().unwrap().kind;
        let line = self.line();
        let rule = self.get_rule(operator);

        self.parse_precedence(
            Precedence::try_from_primitive(u8::from(rule.precedence) +1).unwrap()
        );

        match operator {
            TokenKind::Plus  => self.emit_byte(OpCode::Add, line),
            TokenKind::Minus => self.emit_byte(OpCode::Subtract, line),
            TokenKind::Star  => self.emit_byte(OpCode::Multiply, line),
            TokenKind::Slash => self.emit_byte(OpCode::Divide, line),

            _ => unreachable!("unkown binary op, how did u do this: {}",operator),
        }

    }
    
    fn parse_precedence(&self, precedence: Precedence) {
        todo!()
    }
    
    fn get_rule(&self, operator: &TokenKind) -> Rule {
        todo!()
    }

    
}



