namespace LoxSharp;

 

public abstract class Expr {
public interface IVisitor<R>{
		R visitAssignExpr(Assign expr);
		R visitBinaryExpr(Binary expr);
		R visitGroupingExpr(Grouping expr);
		R visitLiteralExpr(Literal expr);
		R visitLogicalExpr(Logical expr);
		R visitUnaryExpr(Unary expr);
		R visitVariableExpr(Variable expr);
}
public class Assign : Expr {
	 public Assign(Token name,Expr value){
		this.name = name;
		this.value = value;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitAssignExpr(this);
		 }


	 public Token name { get; }
	 public Expr value { get; }
}
public class Binary : Expr {
	 public Binary(Expr left,Token Op,Expr right){
		this.left = left;
		this.Op = Op;
		this.right = right;
	}

public override  R accept<R>(IVisitor<R> visitor) {
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

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitGroupingExpr(this);
		 }


	 public Expr expression { get; }
}
public class Literal : Expr {
	 public Literal(Object value){
		this.value = value;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitLiteralExpr(this);
		 }


	 public Object value { get; }
}
public class Logical : Expr {
	 public Logical(Expr left,Token Op,Expr right){
		this.left = left;
		this.Op = Op;
		this.right = right;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitLogicalExpr(this);
		 }


	 public Expr left { get; }
	 public Token Op { get; }
	 public Expr right { get; }
}
public class Unary : Expr {
	 public Unary(Token op,Expr right){
		this.op = op;
		this.right = right;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitUnaryExpr(this);
		 }


	 public Token op { get; }
	 public Expr right { get; }
}
public class Variable : Expr {
	 public Variable(Token name){
		this.name = name;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitVariableExpr(this);
		 }


	 public Token name { get; }
}

public abstract R accept<R>(IVisitor<R> visitor);
}
