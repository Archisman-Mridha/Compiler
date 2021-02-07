using System;
using Libraries.Evaluator;
using Libraries.Tokeninfo;
using Libraries.Parser;
using System.Linq;
using Libraries.ExpressionSyntax;

namespace Compiler
{
    class Program
    {
        static void Main ( )
        {
            while( true )
            {
                Console.Write("> ");

                var line = Console.ReadLine( );
                
                if ( string.IsNullOrWhiteSpace(line) )
                    return;

                var parser = new Parser( line );
                var parsedObject = parser.Parse( );

                prettyPrint( parsedObject.root );

                if ( parsedObject.errors.Any( ) )
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach ( var error in parser.Errors )
                        Console.WriteLine( error );

                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    var evaluatedResult = new Evaluator( parsedObject.root );
                    var result = evaluatedResult.Evaluate( );

                    Console.WriteLine( result );
                }
            }
        }

        static void prettyPrint ( SyntaxNode node, string indent = "", bool isLastChild = true, bool firstChild = true )
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
    }
}