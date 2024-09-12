use crate::chunk::{Line, OpCode};

mod bitwise;
mod chunk;
mod value;

fn main() {
    let mut chunk = chunk::Chunk::new("test chunk");

    println!("{:?}", chunk);
}