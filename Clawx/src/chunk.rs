use crate::value::{Value, ValueArray};


#[derive(Debug)]
pub enum Opcode {
    OpConstant(Value),
    OpReturn,
     
}

pub struct CodeLine{
    pub code: Opcode,
    pub line: i32,
}

pub struct Chunk{
    pub code: Vec<CodeLine>,
    pub constants: ValueArray,
}

impl Chunk {
    pub fn new_chunk() -> Chunk {
        Chunk {
            code: Vec::new(),
            constants: ValueArray::new(),
        }
    }

    pub fn write_chunk(&mut self, code: Opcode, line: i32){
        self.code.push(CodeLine{code,line})
    }
}