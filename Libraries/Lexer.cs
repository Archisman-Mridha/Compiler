using Libraries.Tokens;
using Libraries.Tokeninfo;
using System.Collections.Generic;

namespace Libraries.Lexer
{
    class Lexer
    {
        private readonly string text;
        private int position;

        public List<string> errors = new List<string>( );

        public Lexer ( string text )
        {
            this.text = text;
            this.position = 0;
        }

        private char Current
        {
            get
            {
                if ( position >= text.Length )
                    return '\0';

                return text[ position ];
            }
        }

        private void IncrementPosition ( )
        {
            position++;
        }

        public TokenDetails ParseToken( )
        {
            if ( position >= text.Length )
                return new TokenDetails( Tokentypes.EndOfFile, position, "\0", null );

            if ( char.IsDigit( Current ) )
            {
                var start = position;

                while ( char.IsDigit( Current ) )
                    IncrementPosition( );

                var length = position - start;
                var part = text.Substring( start, length );

                if ( !int.TryParse( part, out var value ) )
                    errors.Add($"The number { text } isn't valid int32");

                return new TokenDetails( Tokentypes.Number, start, part, value );
            }

            else if ( char.IsWhiteSpace( Current ) )
            {
                var start = position;

                while ( char.IsWhiteSpace( Current ) )
                    IncrementPosition( );

                var length = position - start;
                var part = text.Substring( start, length );

                return new TokenDetails( Tokentypes.WhiteSpace, start, part, null );
            }

            switch( Current )
            {
                case '+':
                    return new TokenDetails( Tokentypes.Plus, position++, "+", null );

                case '-':
                    return new TokenDetails( Tokentypes.Minus, position++, "-", null );

                case '*':
                    return new TokenDetails( Tokentypes.Multiply, position++, "*", null );

                case '/':
                    return new TokenDetails( Tokentypes.Slash, position++, "/", null );

                case '(':
                    return new TokenDetails( Tokentypes.OpenParanthesis, position++, "(", null );

                case ')':
                    return new TokenDetails( Tokentypes.CloseParanthesis, position++, ")", null );

                default:
                    errors.Add($"ERROR: Bad Character Input: { Current }");
                    return new TokenDetails( Tokentypes.Unknown, position++, text.Substring( position-1, 1 ), null );
            }
        }
    }
}