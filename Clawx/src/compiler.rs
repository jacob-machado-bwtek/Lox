
use crate::{
    chunk::{self, Chunk, OpCode},
    scanner::{Scanner, Token, TokenKind},
};

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
        todo!()
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
        self.emit_byte(OpCode::Return);
    }
    
    fn emit_byte<T>(&mut self, byte: T)
    where T: Into<u8>, {
        let line = self.previous.as_ref().unwrap().line;
        self.current_chunk().write(byte,line)
    }
    
    fn current_chunk(&mut self) -> &mut Chunk {
        &mut self.chunk
    }
    
}



