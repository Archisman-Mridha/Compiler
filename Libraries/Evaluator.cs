using Libraries.Tokens;
using System;
using Libraries.ExpressionSyntax;

namespace Libraries.Evaluator
{
    class Evaluator
    {
        private readonly ExpressionSyntax.ExpressionSyntax root;
    
        public Evaluator ( ExpressionSyntax.ExpressionSyntax root )
        {
            this.root = root;
        }
    
        public int Evaluate ( )
        {
            return EvaluateExpression( root );
        }
    
        private int EvaluateExpression ( ExpressionSyntax.ExpressionSyntax node )
        {
            if ( node is NumberExpressionSyntax n )
                return ( int )n.numberToken.value;
    
            if ( node is BinaryExpressionSyntax b )
            {
                var left = EvaluateExpression( b.left );
                var right = EvaluateExpression( b.right );
    
                switch ( b.operatorToken.tokenType )
                {
                    case Tokentypes.Plus:
                        return right + left;
    
                    case Tokentypes.Minus:
                        return left - right;
                    
                    case Tokentypes.Multiply:
                        return left * right;
                    
                    case Tokentypes.Slash:
                        return left / right;
    
                    default:
                        throw new System.Exception($"Unexpected Binary Operator: { b.operatorToken.tokenType }");
                }
            }

            if ( node is ParanthesizedExpressionSyntax p )
                return EvaluateExpression( p.expression );
    
            else
                throw new Exception($"Unexpected node: { node.tokenType }");
        }
    }
}