namespace LoxSharp;

 

public abstract class Stmt {
public interface IVisitor<R>{
		R visitExpressionStmt(Expression stmt);
		R visitPrintStmt(Print stmt);
		R visitVarStmt(Var stmt);
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
