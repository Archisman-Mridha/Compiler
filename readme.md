# Compiler Designing

## Lexer

The lexer of the compiler is responsible for recognizing each syntax token.

We define a class for describing the Lexer. The class has 2 data properties : a word that we pass onto him, and an integer representing the position in the word. Initially the position is 0, starting from the beginning of the word.

```cs

    class Lexer
    {
        private readonly string text;
        private int position;
    }
```

In the Lexer class we have constructor, where we initialize the properties.

```cs

    public Lexer ( string text )
    {
        this.text = text;
        this.position = 0;
    }
```

We include some additional properties, like getting the current character, based on the current position property of the class.
We need the if-else block, to detect the end-of-file, and if it is, then return the '\0' character.

```cs

    private char Current
    {
        get
        {
            if ( position >= text.Length )
                return '\0';

            return text[ position ];
        }
    }
```

To increment the position, we define a function IncrementPosition :

```cs

    private void IncrementPosition ( )
    {
        position++;
    }
```

For representing different types of tokens, an enum named TokenTypes is created.

```cs

    enum Tokentypes
    {
        Number, WhiteSpace, Plus, Minus, Slash, OpenParanthesis, CloseParanthesis, Unknown, EndOfFile, NumberExpression, BinaryExpression
    }
```

To store the details of each token, we create a class TokenDetails. This class may give details about
an individual token like "+" or "/" or "(" or details about a series of tokens, in case of a number or
an alphabetical word.

```cs

    class TokenDetails
    {
        public TokenDetails ( Tokentypes tokenType, int position, string part, object value )
        {
            this.tokenType = tokenType;
            this.position = position;
            this.part = part;
            this.value = value;
        }

        public Tokentypes tokenType { get; }
        public int position { get; }
        public string text { get; }
        public object value { get; }
    }
```

Now we build the token parser function.

```cs

    private TokenDetails ParseToken ( )
    {
        if ( char.IsDigit( Current ) )
        {
            var start = position;

            while ( char.IsDigit( Current ) )
                IncrementPosition ( );

            var length = position - start;
            var subString = text.Substring ( start, length );
            int.TryParse( part, out var value );
            return TokenDetails ( TokenTypes.Number, start, part, value );
        }

        else if ( char.IsWhiteSpace( Current ) )
        {
            var start = position;

            while ( char.IsWhiteSpace( Current ) )
                IncrementPosition( );

            var length = position - start;
            var subString = text.SubString ( start, length );

            return TokenDetails ( TokenTypes.WhiteSpace, start, part, null );
        }

        switch ( Current )
        {
            case '+':
                return TokenDetails ( TokenTypes.Plus, position++, "+", null );

            case '-':
                return TokenDetails ( TokenTypes.Minus, position++, "-", null );

            case  '/':
                return TokenDetails ( TokenTypes.Slash, position++, "/", null );

            case '(':
                return TokenDetails ( TokenTypes.OpenParanthesis, position++, "(", null );

            case ')':
                return TokenDetails ( TokenTypes.CloseParanthesis, position++, ")", null );

            default:
                return TokenDetails ( TokenTypes.UnknownToken, position++, text.SubString( position-1, position ), null );
        }

        if ( position >= text.Length )
            return new SyntaxToken( Tokentypes.EndOfFile, position, "\0", null );
    }
```

## Parser

Parser is responsible for parsing the expressions, after the lexer parses different tokens. We create
a class Parser, which has 2 properties : a list storing tokendetails object for each syntax token in
an expression and an integer varibale position storing the position in the expression.

```cs

    class Parser
    {
        private readonly TokenDetails[] characters;
        private int position;
    }
```

In the constructor function, we take the expression as parameter, then parse that using the lexer, and
store the tokendetails object for each syntax token in the characters list. Also we initialize the
position to 0.

```cs

    public Parser ( string text )
    {
        var tokenlist = new List<TokenDetails>( );

        var lexer = new Lexer( text );
        TokenDetails tokenDetails;

        do
        {
            tokenDetails = lexer.parseToken( );

            if ( tokenDetails.tokenType != TokenTypes.WhiteSpace && tokenDetails.tokenType != TokenTypes.UnknownToken )
                tokenList.Add( tokenDetails );

        } while ( tokenDetails.tokenType != TokenTypes.EndOfFile );

        this.characters = tokenlist.ToArray( );
        this.position = 0;
    }
```

Now we add some additional properties to the Parser class

```cs

    private TokenDetails Current
    {
       get
       {
           return characters[ position ];
       }
    }

    private TokenDetails nextToken ( )
    {
        var current = Current;
        position++;
        return current;
    }

    private SyntaxToken matchToken ( TokenTypes tokenType )
    {
        if ( current.tokenType == tokenType )
            nextToken( );

        return new TokenDetails ( tokenType, Current.position, null, null );
    }
```

Let's forget about the Parse class for a moment and think about how we will represent and evaluate
expressions, like binary expressions. We use an abstract class SyntaxNode first, where we have 2
properties, tokenType and GetChildren( ). Then we create another abstract class ExpressionSyntax, which
inherits from the SyntaxNode class. This class represents, that we are dealing with expressions, like
binary and number expressions.

Consider  a binary expression like '1 + 2 + 32'. Our compiler evaluates
the binary expression in this manner :

        +
       / \
      +   32
     / \
    1   2

Here we treat 1, 2 and 32 as number expressions.

The tokenType property, returns the syntax-token type. Like '32' is a NumberExpression and '1+2+3'
is a BinaryExpression. The GetChildren( ) property is an iterator, which is used mainly in case of
BinaryExpressionSyntax. BinaryExpressionSyntax consists of three types of data in a binary operation :
the left expresion, middle operator and the right expression. So GetChildren( ) returns all of these
3 things, and for multiple returns we have used the yield keyword in it.

```cs

    abstract class SyntaxNode
    {
        abstract public Tokentypes tokenType { get; }
        abstract public IEnumerable<SyntaxNode> GetChildren( );
    }

    abstract class ExpressionSyntax
    {}
```

Now we define the NumberExpressionSyntax and BinaryExpressionSyntax classes, which represent binary
and number expressions. We make these 2 classes sealed, so that no other class can further inherit
from these 2 classes.

```cs

    class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax ( TokenDetails numberToken )
        {
            this.numberToken = numberToken;
        }

        public TokenDetails numberToken { get; }

        public override TokenType tokenType
        {
            get
            {
                return TokenTypes.NumberExpression;
            }
        }

        public override IEnumerable<SyntaxNode> GetChildren ( )
        {
            yield return numberToken;
        }
    }

    sealed class BinaryExpressionSyntax: ExpressionSyntax
    {
        public BinaryExpressionSyntax ( ExpressionSyntax left, TokenDetails operatorToken, ExpressionSyntax right )
        {
            this.right = right;
            this.operatorToken = operatorToken;
            this.left = left;
        }

        public override Tokentypes tokenType
        {
            get
            {
                return Tokentypes.BinaryExpression;
            }
        }

        public ExpressionSyntax left { get; }
        public TokenDetails operatorToken { get; }
        public ExpressionSyntax right { get; }

        public override IEnumerable<SyntaxNode> GetChildren( )
        {
            yield return left;
            yield return operatorToken;
            yield return right;
        }
    }
```

## Pretty Printing the Expression Tree

We need to pretty-print the binary tree of expressions, that was showed in the example - '1 + 2 + 3'.
For this, we create a function prettyPrint( ) which takes the root node of the tree ( middle operator )
a operator and then using recursion, prints the children of the root node.

```cs

    static void prettyPrint ( SyntaxNode node, string indent= "", bool isLastChild = true, bool isFirstChild = true )
    {
        var marker = isLastChild ? "└──" : "├──";
        var lastChild = node.GetChildren( ).LastOrDefault( );

        Console.Write( indent + (firstChild ? "" : marker) + node.tokenType );

        if ( node is TokenDetails t && t.value != null )
        {
            Console.Write(" ");
            Console.Write( t.value );
        }

        Console.WriteLine( );

        indent += isLastChild ? "    ": "│   ";

        foreach( var child in node.GetChildren( ) )
            prettyPrint( child, indent, child == lastChild, false );
    }
```

## Error Handling

Wait ! we are missing something. Error handling. Let's get started with it. In our Lexer class, we introduce a new data filed, errors, which is a list of strings.

```cs

    int public List<string> errors = new List<string>( );
```

In the ParseToken( ) function, where we recognize all the tokens, there can be 2 error situations.

* Error if we can't parse a number token.

```cs

    if ( !int.TryParse( part, out var value ) )
        errors.Add($"The number { text } isn't valid int32");
```

* Error if we can't recognize a token.

```cs

    errors.Add($"ERROR: Bad Character Input: { Current }");
```

Similarly, inm our Parser where we evaluate our expressions, we introduce a list of strings as the error message storing field. In the constructor function of Parser class, where we recognize every token, using the Lexer class, we catch the errors ocuring in token recognization and add them to the error list field of the Parser class.

```cs

    private List<string> errors = new List<string>( );

    Parser ( )
    {
        // at the end of token parsing
        errors.AddRange( lexer.errors );
    }
```

Next in the matchToken( ) funtion, we catch the errors, if we don't find expected tokens.