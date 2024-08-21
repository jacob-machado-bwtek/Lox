namespace LoxSharp;

 

public abstract class Stmt {
public interface IVisitor<R>{
		R visitBlockStmt(Block stmt);
		R visitClassStmt(Class stmt);
		R visitExpressionStmt(Expression stmt);
		R visitFunctionStmt(Function stmt);
		R visitIfStmt(If stmt);
		R visitPrintStmt(Print stmt);
		R visitReturnStmt(Return stmt);
		R visitVarStmt(Var stmt);
		R visitWhileStmt(While stmt);
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
public class Class : Stmt {
	 public Class(Token name,List<Stmt.Function> methods){
		this.name = name;
		this.methods = methods;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitClassStmt(this);
		 }


	 public Token name { get; }
	 public List<Stmt.Function> methods { get; }
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
public class Function : Stmt {
	 public Function(Token name,List<Token> parameters,List<Stmt> body){
		this.name = name;
		this.parameters = parameters;
		this.body = body;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitFunctionStmt(this);
		 }


	 public Token name { get; }
	 public List<Token> parameters { get; }
	 public List<Stmt> body { get; }
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
public class Return : Stmt {
	 public Return(Token keyword,Expr value){
		this.keyword = keyword;
		this.value = value;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitReturnStmt(this);
		 }


	 public Token keyword { get; }
	 public Expr value { get; }
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
public class While : Stmt {
	 public While(Expr condition,Stmt body){
		this.condition = condition;
		this.body = body;
	}

public override  R accept<R>(IVisitor<R> visitor) {
			 return visitor.visitWhileStmt(this);
		 }


	 public Expr condition { get; }
	 public Stmt body { get; }
}

public abstract R accept<R>(IVisitor<R> visitor);
}
