#[derive(Debug)]
pub enum Opcode {
    OpConstant,
    OpReturn,
     
}

pub struct Chunk{
    pub code: Vec<Opcode>,
}

impl Chunk {
    pub fn new_chunk() -> Chunk {
        Chunk {
            code: Vec::new(),
        }
    }

    pub fn write_chunk(&mut self, code: Opcode){
        self.code.push(code)
    }
}