namespace LoxSharp;

 

public abstract class Expr {
public interface Visitor<R>{
		R visitBinaryExpr(Binary expr);
		R visitGroupingExpr(Grouping expr);
		R visitLiteralExpr(Literal expr);
		R visitUnaryExpr(Unary expr);
}
public class Binary : Expr {
	 public Binary(Expr left,Token Op,Expr right){
		this.left = left;
		this.Op = Op;
		this.right = right;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitBinaryExpr(this);
		 }


	 public Expr left { get; }
	 public Token Op { get; }
	 public Expr right { get; }
}
public class Grouping : Expr {
	 public Grouping(Expr expression){
		this.expression = expression;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitGroupingExpr(this);
		 }


	 public Expr expression { get; }
}
public class Literal : Expr {
	 public Literal(Object value){
		this.value = value;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitLiteralExpr(this);
		 }


	 public Object value { get; }
}
public class Unary : Expr {
	 public Unary(Token op,Expr right){
		this.op = op;
		this.right = right;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitUnaryExpr(this);
		 }


	 public Token op { get; }
	 public Expr right { get; }
}

public abstract R accept<R>(Visitor<R> visitor);
}
