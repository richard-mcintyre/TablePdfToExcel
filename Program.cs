using System;
using System.IO;
using System.Text;

namespace TablePdfToExcel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText("TextToParse.txt");

            Lexer lexer = new Lexer(text);

            Token token = lexer.Next();

            StringBuilder sb = new StringBuilder();

            TokenKind? lastKind = null;
            while (token is not null)
            {
                if (token.Kind == TokenKind.Text)
                {
                    sb.Append(token.Text);
                    sb.AppendLine();
                }
                else if (token.Kind == TokenKind.Number)
                {
                    if (lastKind == TokenKind.Number)
                        sb.AppendLine();

                    sb.Append(token.Text);
                    sb.Append(",");
                }

                Console.WriteLine($"{token.Kind}: {token.Text}");

                lastKind = token.Kind;
                token = lexer.Next();
            }

            Console.WriteLine(sb.ToString());

            File.WriteAllText("pdf.csv", sb.ToString());
        }
    }
}
