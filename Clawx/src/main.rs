use std::{process::ExitCode, vec};
use std::fmt::Write;

use chunk::Opcode;
use disassembler::disassemble_chunk;

mod value;
mod chunk;
mod disassembler;
fn main() -> ExitCode{
   let mut mychunk = chunk::Chunk::new_chunk();
   mychunk.write_chunk(chunk::Opcode::OpConstant(1.2345), 143);
   mychunk.write_chunk(Opcode::OpReturn,123);

   let myresult =  disassemble_chunk(mychunk, &"Test Chunk");
   ExitCode::SUCCESS
}