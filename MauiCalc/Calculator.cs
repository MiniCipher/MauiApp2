using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MauiCalc;

namespace MauiCalc
{
    public class Calculator
    {
        public static Sheet sheet { get; }
        static Calculator()
        {
            sheet = new Sheet();
        }
        public static double Evaluate(string expression)
        {
            var lexer = new MyGrammarLexer(new AntlrInputStream(expression));
            var tokens = new CommonTokenStream(lexer);
            var parser = new MyGrammarParser(tokens);
            var tree = parser.expression();
            var visitor = new MyGrammarVisitor();
            double s = visitor.Visit(tree);
            return s;
        }

    }
}
