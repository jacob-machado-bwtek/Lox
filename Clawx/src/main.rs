use std::io::Write;
use vm::InterpretResult;

use crate::vm::VM;


mod scanner;
mod compiler;
mod types;
mod bitwise;
mod chunk;
mod value;
mod vm;

fn main() {
   match std::env::args().collect::<Vec<_>>().as_slice() {
       [_] => repl(),                // When no arguments are provided, only the program name is present.
       [_, file] => run_file(file),   // When one argument is provided, it will match as [program_name, file].
       _ => {
           eprintln!("Usage: clawx [path]");
           std::process::exit(64);
       }
   }
}  


fn repl() {
   let mut vm = VM::new();
   loop{
      print!("> ");
      std::io::stdout().flush().unwrap();
      let mut line = String::new();
      if std::io::stdin().read_line(&mut line).unwrap() > 0 {
         vm.interpret(line.as_bytes());
      } else {
         println!();
         break;
      }
   }
}

fn run_file(file: &str){
   match std::fs::read(file) {
      Err(e) => {
         eprintln!("{}",e);
         std::process::exit(74);
      },

      Ok(contents) =>  {
         let mut vm = VM::new();
         match vm.interpret(&contents) {
             InterpretResult::CompileError => std::process::exit(65),
             InterpretResult::RuntimeError => std::process::exit(70),
             InterpretResult::Ok => {}
         }
      },

      Err(e) => {
          eprintln!("Invalid UTF-8 sequence: {}", e);
          std::process::exit(74);
      }
  }    
}