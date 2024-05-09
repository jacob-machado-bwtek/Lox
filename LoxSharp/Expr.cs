namespace LoxSharp;

 

abstract class Expr {
public class Binary : Expr {
	 public Binary(Expr left,Token Op,Expr right){
this.left = left;
this.Op = Op;
this.right = right;
}


	 public Expr left { get; }
	 public Token Op { get; }
	 public Expr right { get; }
}
public class Grouping : Expr {
	 public Grouping(Expr expression){
this.expression = expression;
}


	 public Expr expression { get; }
}
public class Literal : Expr {
	 public Literal(Object value){
this.value = value;
}


	 public Object value { get; }
}
public class Unary : Expr {
	 public Unary(Token op,Expr right){
this.op = op;
this.right = right;
}


	 public Token op { get; }
	 public Expr right { get; }
}
}
