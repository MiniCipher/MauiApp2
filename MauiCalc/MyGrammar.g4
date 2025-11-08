grammar MyGrammar;

compileUnit: expression EOF;

expression
    : '-' expression                                    # UnaryMinus
    | '+' expression                                    # UnaryPlus
    | expression '^' expression                         # Exponent
    | expression '*' expression                         # Multiply
    | expression '/' expression                         # Divide
    | expression '+' expression                         # Add
    | expression '-' expression                         # Subtract
    | expression '=' expression                         # Equal
    | expression '<' expression                         # LessThan
    | expression '>' expression                         # GreaterThan
    | expression '<=' expression                        # LessThanOrEqual
    | expression '>=' expression                        # GreaterThanOrEqual
    | expression '<>' expression                        # NotEqual
    | '(' expression ')'                                # Parens
    | NUMBER                                            # Number
    | IDENTIFIER #IdentifierExpr
    ;

NUMBER: [0-9]+;
IDENTIFIER : [a-zA-Z]+[1-9][0-9]*;

WS: [ \t\r\n]+ -> skip;