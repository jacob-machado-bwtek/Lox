# Chapter 5 challenges

### Challenge 1
**Earlier, I said that the |, *, and + forms we added to our grammar metasyntax were just syntactic sugar. Take this grammar:

expr → expr ( "(" ( expr ( "," expr )* )? ")" | "." IDENTIFIER )+
     | IDENTIFIER
     | NUMBER
Produce a grammar that matches the same language but does not use any of that notational sugar.

Bonus: What kind of expression does this bit of grammar encode?**

``` 
expr → expr_tail expr_list
     | IDENTIFIER
     | NUMBER

expr_list → expr_tail expr_list
          | expr_tail

expr_tail → function_call
          | member_access

function_call → "(" expr_seq ")"
              | "(" ")"

expr_seq → expr more_exprs

more_exprs → "," expr more_exprs
           | "," expr

member_access → "." IDENTIFIER
```

### Challenge 2
The Visitor pattern lets you emulate the functional style in an object-oriented language. Devise a complementary pattern for a functional language. It should let you bundle all of the operations on one type together and let you define new types easily.

