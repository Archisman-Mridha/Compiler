using Libraries.Tokeninfo;
using System.Collections.Generic;
using System.Linq;
using Libraries.ExpressionSyntax;

sealed class SyntaxTree
{
    public SyntaxTree ( IEnumerable<string> errors, ExpressionSyntax root, TokenDetails endOfFileToken )
    {
        this.root = root;
        this.errors = errors.ToArray( );
        this.endOfFileToken = endOfFileToken;
    }

    public ExpressionSyntax root { get; }
    public TokenDetails endOfFileToken { get; }
    public IReadOnlyList<string> errors { get; }
}