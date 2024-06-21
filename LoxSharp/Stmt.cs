namespace LoxSharp;

 

public abstract class Stmt {
public interface Visitor<R>{
		R visitBinaryStmt(Binary stmt);
		R visitGroupingStmt(Grouping stmt);
		R visitLiteralStmt(Literal stmt);
		R visitUnaryStmt(Unary stmt);
		R visitExpressionStmt(Expression stmt);
		R visitPrintStmt(Print stmt);
}
public class Binary : Stmt {
	 public Binary(Expr left,Token Op,Expr right){
		this.left = left;
		this.Op = Op;
		this.right = right;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitBinaryStmt(this);
		 }


	 public Expr left { get; }
	 public Token Op { get; }
	 public Expr right { get; }
}
public class Grouping : Stmt {
	 public Grouping(Expr expression){
		this.expression = expression;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitGroupingStmt(this);
		 }


	 public Expr expression { get; }
}
public class Literal : Stmt {
	 public Literal(Object value){
		this.value = value;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitLiteralStmt(this);
		 }


	 public Object value { get; }
}
public class Unary : Stmt {
	 public Unary(Token op,Expr right){
		this.op = op;
		this.right = right;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitUnaryStmt(this);
		 }


	 public Token op { get; }
	 public Expr right { get; }
}
public class Expression : Stmt {
	 public Expression(Expr expression){
		this.expression = expression;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitExpressionStmt(this);
		 }


	 public Expr expression { get; }
}
public class Print : Stmt {
	 public Print(Expr expression){
		this.expression = expression;
	}

public override  R accept<R>(Visitor<R> visitor) {
			 return visitor.visitPrintStmt(this);
		 }


	 public Expr expression { get; }
}

public abstract R accept<R>(Visitor<R> visitor);
}
