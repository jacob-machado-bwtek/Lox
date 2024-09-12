use crate::value::{Value};
use crate::chunk::{Chunk,Opcode};
use super::chunk::{Chunk, OpCodeLine};
use smol_str::SmolStr;


pub fn disassemble_chunk(chunk: &Chunk, name: &Option<StrId>) {
  if cfg!(debug_assertions) {
      println!(
          "== {} ==",
          name.map(interner::str)
              .unwrap_or_else(|| SmolStr::new_inline("SCRIPT"))
      );
      for (index, instruction) in chunk.code.iter().enumerate() {
          disassemble_instruction(index, instruction);
      }
  }
}



fn disassemble_instruction(index: usize, instruction: &OpCodeLine) {
  println!(
      "{:04}  L{:03}  {:?}",
      index, instruction.line, instruction.code
  );
}


fn simple_instruction(name: String) -> () {
   println!("{}", name); //prints named opcode
}