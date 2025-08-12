using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePdfToExcel;
internal class Lexer
{
    public Lexer(string source)
    {
        _source = source;
    }

    private readonly string _source;
    private int _pos;

    public Token Next()
    {
        if (IsEOF())
            return null;

        SkipWhitespace();
        if (IsEOF())
            return null;

        if (TryParse3Or4DigitNumber(out int number))
            return new Token(TokenKind.Number, number, number.ToString());

        return new Token(TokenKind.Text, -1, ReadTextUntilNext3Or4DigitNumber());
    }

    private bool IsEOF() => _pos >= _source.Length;

    private string ReadTextUntilNext3Or4DigitNumber()
    {
        int startPos = _pos;
        while (!IsEOF())
        {
            _pos++;

            if (IsWhitespace(_source[_pos]))
            {
                int endPos = _pos;

                SkipWhitespace();

                int _savedPos = _pos;
                if (TryParse3Or4DigitNumber(out _))
                {
                    _pos = _savedPos;
                    return _source.Substring(startPos, endPos - startPos);
                }
                _pos = _savedPos;
            }
        }

        throw new Exception("rcm");
    }

    private bool TryParse3Or4DigitNumber(out int number)
    {
        number = -1;
        if (IsEOF())
            return false;

        int savedPos = _pos;
        if (IsNumber(_source[_pos]))
            number = _source[_pos] - '0';
        else
            return false;

        _pos++;
        if (IsNumber(_source[_pos]))
        {
            number *= 10;
            number += _source[_pos] - '0';
        }
        else
        {
            _pos = savedPos;
            return false;
        }

        _pos++;
        if (IsNumber(_source[_pos]))
        {
            number *= 10;
            number += _source[_pos] - '0';
        }
        else
        {
            _pos = savedPos;
            return false;
        }

        _pos++;
        if (IsNumber(_source[_pos]))
        {
            number *= 10;
            number += _source[_pos] - '0';

            _pos++;
        }

        return true;
    }

    private static bool IsNumber(char ch)
    {
        switch (ch)
        {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return true;
        }

        return false;
    }

    private void SkipWhitespace()
    {
        for (; !IsEOF(); _pos++)
        {
            if (!IsWhitespace(_source[_pos]))
                break;
        }
    }

    private static bool IsWhitespace(char ch)
    {
        switch (ch)
        {
            case ' ':
            case '\t':
            case '\r':
            case '\n':
                return true;
        }

        return false;
    }
}
