parser grammar Common;

@header{
using System;
}


@parser::members
{
public bool skipToEndOfObject()
{
    Stack<Object> CurlyStack = new Stack<Object>();
    Object o = new Object();
    int t = _input.La(1);

    while (t != -1 && !(CurlyStack.Count == 0 && t == CLOSING_CURLY))
    {
        if (t == PRE_ELSE)
        {
            Stack<Object> ifdefStack = new Stack<Object>();
            Consume();
            t = _input.La(1);

            while (t != -1 && !(ifdefStack.Count == 0 && (t == PRE_ENDIF)))
            {
                if (t == PRE_IF)
                {
                    ifdefStack.Push(o);
                }
                else if (t == PRE_ENDIF)
                {
                    ifdefStack.Pop();
                }
                Consume();
                t = _input.La(1);
            }
        }

        if (t == OPENING_CURLY)
        {
            CurlyStack.Push(o);
        }
        else if (t == CLOSING_CURLY)
        {
            CurlyStack.Pop();
        }
        Consume();
        t = _input.La(1);
    }
    if (t != -1)
    {
        Consume();
    }
    return true;
}

// this should go into FunctionGrammar but ANTLR fails
// to join the parser::members-section on inclusion

public bool preProcSkipToEnd()
{
    Stack<Object> CurlyStack = new Stack<Object>();
    Object o = new Object();
    int t = _input.La(1);

    while (t != -1 && !(CurlyStack.Count == 0 && t == PRE_ENDIF))
    {
        if (t == PRE_IF)
        {
            CurlyStack.Push(o);
        }
        else if (t == PRE_ENDIF)
        {
            CurlyStack.Pop();
        }
        Consume();
        t = _input.La(1);
    }
    if (t != -1)
    {
        Consume();
    }

    return true;
}
}

unary_operator : '&' | '*' | '+'| '-' | '~' | '!';
relational_operator: ('<'|'>'|'<='|'>=');

constant
    :   HEX_LITERAL
    |   OCTAL_LITERAL
    |   DECIMAL_LITERAL
    |    STRING
    |   CHAR
    |   FLOATING_POINT_LITERAL
    ;

// keywords & operators

function_decl_specifiers: ('inline' | 'virtual' | 'explicit' | 'friend' | 'static');
ptr_operator: ('*' | '&');

access_specifier: ('public' | 'private' | 'protected');

operator: (('new' | 'delete' ) ('[' ']')?)
  | '+' | '-' | '*' | '/' | '%' |'^' | '&' | '|' | '~'
  | '!' | '=' | '<' | '>' | '+=' | '-=' | '*='
  | '/=' | '%=' | '^=' | '&=' | '|=' | '>>'
  |'<<'| '>>=' | '<<=' | '==' | '!='
  | '<=' | '>=' | '&&' | '||' | '++' | '--'
  | ',' | '->*' | '->' | '(' ')' | '[' ']'
  ;

assignment_operator: '=' | '*=' | '/=' | '%=' | '+=' | '-=' | '<<=' | '>>=' | '&=' | '^=' | '|='; 
equality_operator: ('=='| '!=');

template_decl_start : TEMPLATE '<' template_param_list '>';


// template water
template_param_list : (('<' template_param_list '>') |
                       ('(' template_param_list ')') | 
                       no_angle_brackets_or_brackets)+
;

// water

no_brackets: ~('(' | ')');
no_brackets_curlies_or_squares: ~('(' | ')' | '{' | '}' | '[' | ']');
no_brackets_or_semicolon: ~('(' | ')' | ';');
no_angle_brackets_or_brackets : ~('<' | '>' | '(' | ')');
no_curlies: ~('{' | '}');
no_squares: ~('[' | ']');
no_squares_or_semicolon: ~('[' | ']' | ';');
no_comma_or_semicolon: ~(',' | ';');

assign_water: ~('(' | ')' | '{' | '}' | '[' | ']' | ';' | ',');
assign_water_l2: ~('(' | ')' | '{' | '}' | '[' | ']');

water: .;
