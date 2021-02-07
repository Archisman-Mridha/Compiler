using System.Collections.Generic;
using Libraries.Tokeninfo;
using Libraries.Tokens;

namespace Libraries.ExpressionSyntax
{
    abstract class SyntaxNode
    {
        abstract public Tokentypes tokenType { get; }
        abstract public IEnumerable<SyntaxNode> GetChildren( );
    }

    abstract class ExpressionSyntax: SyntaxNode
    {}

    sealed class NumberExpressionSyntax: ExpressionSyntax
    {
        public NumberExpressionSyntax ( TokenDetails numberToken )
        {
            this.numberToken = numberToken;
        }

        public override Tokentypes tokenType
        {
            get
            {
                return Tokentypes.NumberExpression;
            }
        }

        public TokenDetails numberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren( )
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

    sealed class ParanthesizedExpressionSyntax: ExpressionSyntax
    {
        public ParanthesizedExpressionSyntax ( TokenDetails openParanthesis, ExpressionSyntax expression, TokenDetails closeParanthesis )
        {
            this.openParanthesis = openParanthesis;
            this.expression = expression;
            this.closeParanthesis = closeParanthesis;
        }

        public override Tokentypes tokenType => Tokentypes.ParanthesizedExpression;
        public TokenDetails openParanthesis { get; }
        public TokenDetails closeParanthesis { get; }
        public ExpressionSyntax expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren ( )
        {
            yield return openParanthesis;
            yield return expression;
            yield return closeParanthesis;
        }
    }
}