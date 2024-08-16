namespace LoxSharp;

 

public abstract class Stmt {
public interface IVisitor<R>{
		R visitBinaryStmt(Binary stmt);
		R visitGroupingStmt(Grouping stmt);
		R visitLiteralStmt(Literal stmt);
		R visitUnaryStmt(Unary stmt);
		R visitVariableStmt(Variable stmt);
		R visitExpressionStmt(Expression stmt);
		R visitPrintStmt(Print stmt);
		R visitVarStmt(Var stmt);
}
public class Binary : Stmt {
	 public Binary(Expr left,Token Op,Expr right){
		this.left = left;
		this.Op = Op;
		this.right = right;
	}

public override  R accept<R>(IVisitor<R> visitor) {
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

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitGroupingStmt(this);
		 }


	 public Expr expression { get; }
}
public class Literal : Stmt {
	 public Literal(Object value){
		this.value = value;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitLiteralStmt(this);
		 }


	 public Object value { get; }
}
public class Unary : Stmt {
	 public Unary(Token op,Expr right){
		this.op = op;
		this.right = right;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitUnaryStmt(this);
		 }


	 public Token op { get; }
	 public Expr right { get; }
}
public class Variable : Stmt {
	 public Variable(Token name){
		this.name = name;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitVariableStmt(this);
		 }


	 public Token name { get; }
}
public class Expression : Stmt {
	 public Expression(Expr expression){
		this.expression = expression;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitExpressionStmt(this);
		 }


	 public Expr expression { get; }
}
public class Print : Stmt {
	 public Print(Expr expression){
		this.expression = expression;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitPrintStmt(this);
		 }


	 public Expr expression { get; }
}
public class Var : Stmt {
	 public Var(Token name,Expr initializer){
		this.name = name;
		this.initializer = initializer;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitVarStmt(this);
		 }


	 public Token name { get; }
	 public Expr initializer { get; }
}

public abstract R accept<R>(IVisitor<R> visitor);
}
