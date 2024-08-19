﻿namespace LoxSharp;

 

public abstract class Stmt {
public interface IVisitor<R>{
		R visitBlockStmt(Block stmt);
		R visitExpressionStmt(Expression stmt);
		R visitIfStmt(If stmt);
		R visitPrintStmt(Print stmt);
		R visitVarStmt(Var stmt);
}
public class Block : Stmt {
	 public Block(List<Stmt> statements){
		this.statements = statements;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitBlockStmt(this);
		 }


	 public List<Stmt> statements { get; }
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
public class If : Stmt {
	 public If(Expr condition,Stmt thenBranch,Stmt elseBranch){
		this.condition = condition;
		this.thenBranch = thenBranch;
		this.elseBranch = elseBranch;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitIfStmt(this);
		 }


	 public Expr condition { get; }
	 public Stmt thenBranch { get; }
	 public Stmt elseBranch { get; }
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
