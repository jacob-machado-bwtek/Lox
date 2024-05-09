using LoxSharp;

namespace LoxSharp;


public abstract class Expr {
    


public class Binary : Expr {
        public Binary(Expr left, Token op, Expr right) {
            this.Left = left;
            Operator = op;
            Right = right;
        }

        public Expr Left { get; }
        public Token Operator { get; }

        public Expr Right {get; }
    }

}