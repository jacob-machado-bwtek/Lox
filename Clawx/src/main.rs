use std::{process::ExitCode, vec};
use std::fmt::Write;

use chunk::Opcode;

mod value;
mod chunk;
fn main() -> ExitCode{
   let mut mychunk = chunk::Chunk::new_chunk();
   mychunk.write_chunk(Opcode::OpReturn);

   let myresult = disassemble_chunk(&mychunk, &"Test Chunk");
   ExitCode::SUCCESS
}

fn disassemble_chunk(chunk: &chunk::Chunk, name: &str) -> Result<(), std::fmt::Error> {

    println!("Chunk name: {}", name);

    let mut offset: usize = 0;
    while offset < chunk.code.len(){
      offset = dissasemble_instruction(chunk,offset);
    }

    Ok(())

}

fn dissasemble_instruction(chunk: &chunk::Chunk, offset: usize) -> usize {
    print!("{:04} ", offset);//prints ofset to 4 places

    let instruction = &chunk.code[offset];

    match instruction{
      chunk::Opcode::OpReturn => simple_instruction("OP_RETURN", offset),
      _ => {
         println!("Uknown opcode {:?}", instruction);
         offset+1
      }
    }
}

fn simple_instruction(name: &str, offset: usize) -> usize {
   println!("{}", name); //prints named opcode
   offset +1
}
