# Chapter 4 challenges


### Challenge 1
**Lexical grammars of python and haskell are not regular. What does that mean, and why not?**

  a regular language is a complicated thing, but a convenient description is that it can be captured by a regular expression
  python and haskell both have meaningful whitespace and indentation, so they cannot be captured by a regex. 

### Challenge 2
 **Aside from separating tokens, distinguishing print foo from printfoo, spaces are not important in most languages.**
 **however, in CoffeScript, Ruby and the C preprocessor, a space does have effects. what are theyn and when do they happen?**


I have not used CoffeeScript or Ruby, but I have used C. since he specified preprocessor, I assume its for things like #define or #pragma 
or other preprocessor directives. looking at the list of preprocessor directives, #define can be used for functions
```c
    #define foo(a, b) a + b

    //replaces foo(a, b) with a + b
    //seems to keep the space
```
this is a time where whitespace matters. 

### Challenge 3

**Our Scanner like most, discards comments and whitespace since those are not needed by the parser. when shouldnt you do that?**

    If whitespace is relevant to your programming language, or if you want to do xaml comments, or keep track of todos. 

### Challenge 4
**Add support for C-Style /\* ... \*/ comments. make srue to handle newlines. consider nesting (lolno)**

```cs
 case '/':
    //handle block comments
    if(Match('*')){
    bool endComment = (Peek() == '*' && peekNext() == '/');
    while(!endComment&& !IsAtEnd){
        endComment = (Peek() == '*' && peekNext() == '/');
        if(Peek() == '\n'){
            line++;
        }
        advance();
    }
break;     
```