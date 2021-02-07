using Libraries.Tokens;
using System.Collections.Generic;
using System.Linq;
using Libraries.ExpressionSyntax;

namespace Libraries.Tokeninfo
{
    class TokenDetails: SyntaxNode
    {
        public TokenDetails ( Tokentypes tokenType, int position, string part, object value )
        {
            this.tokenType = tokenType;
            this.position = position;
            this.text = part;
            this.value = value;
        }

        public override Tokentypes tokenType { get; }
        public int position { get; }
        public string text { get; }
        public object value { get; }

        public override IEnumerable<SyntaxNode> GetChildren( )
        {
            return Enumerable.Empty<SyntaxNode>( );
        } 
    }
}