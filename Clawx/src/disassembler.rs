use crate::value::{Value};
use crate::chunk::{Chunk,Opcode};

pub fn disassemble_chunk(chunk: Chunk, name: &str) -> Result<(), std::fmt::Error> {

    println!("Chunk name: {}", name);

    let list = chunk.code.iter();

    for(index, _codeline) in list.enumerate(){
      dissasemble_instruction(&chunk,index);
    }

    Ok(())

}

pub fn dissasemble_instruction(chunk: &Chunk, offset: usize) -> () {
    print!("{:04} ", offset);//prints ofset to 4 places

    let code = &chunk.code[offset];
    let instruction = &code.code;

    match instruction{
      Opcode::OpConstant(constant) => constant_instruction("OP_CONSTANT".to_string(),*constant),
      Opcode::OpReturn => simple_instruction("OP_RETURN".to_string()),
      _ => {
         println!("Uknown opcode {:?}", instruction);         
      }
    }
}

fn constant_instruction(name: String, constant: Value) -> () {
    println!("{:<16} '{:4}'", name, constant);
    
}

fn simple_instruction(name: String) -> () {
   println!("{}", name); //prints named opcode
}