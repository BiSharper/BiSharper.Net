using System.Text;
using BiSharper.Common.Lex;
using BiSharper.Common.Parse;
using BiSharper.Rv.Param.Models;
using BiSharper.Rv.Proc;

namespace BiSharper.Rv.Param;

public partial struct ParamRoot : IParsed<RvProcessorContext>
{
    public void Parse(Lexer lexer)
    {
        var file = new ParamRoot();
        var contexts = new Stack<IParamContextHolder>();
        var currentLine = 1;
        contexts.Push(file);
        for (;;)
        {
            var current = SkipWhitespace(lexer);
            if (current is null)
            {
                if (contexts.Count == 1)
                {
                    break;
                }

                throw new EndOfStreamException($"[{currentLine}] Stream ended before closing context with \"}};\"");
            }

            if (TryReadDirective(lexer, ref currentLine, current))
            {
                continue;
            }

            if (current is '}')
            {

            }
        }
    }

    public static bool TryReadDirective(Lexer lexer, ref int currentLine, char? currentChar)
    {
        if (currentChar == '#')
        {
            lexer.StepForward();
            var directive = GetWord(lexer);
            switch (directive)
            {
                case "line": 
                    ReadLineDirective(lexer, ref currentLine);
                    return true;
                default:
                    throw new Exception($"[{currentLine}] Unknown directive found in param file: \"{directive}\"");
            }
        }

        return false;
    }

    private static void ReadLineDirective(Lexer lexer, ref int currentLine)
    {
        throw new NotImplementedException();
    }

    public static string GetWord(Lexer lexer, ref int lineCount, char[] terminators, out bool quoted)
    {
        var builder = new StringBuilder();
        SkipWhitespace(lexer);


        if (lexer.Current == '"')
        {
            quoted = true;
            lexer.StepForward();
            var previousWasQuote = false;
            while (lexer.Current is { } current)
            {
                switch (current)
                {
                    case '"':
                    {
                        previousWasQuote = true;
                        lexer.StepForward();
                        continue;
                    }
                    case '\r' or '\n': 
                        throw new Exception($"[{lineCount}] Unexpected new line was found in string");
                    default:
                    {
                        if (previousWasQuote)
                        {
                            previousWasQuote = false;
                            if (current != '"')
                            {
                                SkipWhitespace(lexer);
                                if (lexer.Current != '\\')
                                {
                                    lexer.StepBackward();
                                    break;
                                }
                                    
                                lexer.StepForward();
                                if (lexer.Current != 'n') throw new Exception($"[{lineCount}] Unsupported escape sequence \"\\{lexer.Current}\" after \"\"{builder}\"");

                                lexer.StepForward();
                                SkipWhitespace(lexer);
                                if (lexer.Current != '"') throw new Exception($"[{lineCount}] '\"' expected after \"\"{builder}\"");

                                current = '\n';
                            }
                        }
                        builder.Append(current);
                        lexer.StepForward();
                        continue;
                    }
                }

                break;
            }
            
            return builder.ToString();
        }
        quoted = false;
        while (lexer.Current is { } current && !terminators.Contains(current))
        {
            switch (current)
            {
                case '\n' or '\r':
                {
                    if (current == '\n') lineCount++;
                    lexer.StepForward();
                    TryReadDirective(lexer, ref lineCount, lexer.Current);
                    break;
                }
                default:
                {
                    builder.Append(current);
                    lexer.StepForward();
                    break;
                }
            }
        }

        var result = builder.ToString();
        while (result.Length > 0 && IsSpace(result[^1]))
        {
            result = result[..^1];
        }
        return result;
    }
    
    public static string GetWord(Lexer lexer)
    {
        var builder = new StringBuilder();
        SkipWhitespace(lexer);
        while (IsAlphaNumeric(lexer.Current))
        {
            builder.Append(lexer.Current);
        }

        return builder.ToString();
    }

    public static char? SkipWhitespace(Lexer lexer)
    {
        while (IsSpace(lexer.Current))
        {
            lexer.StepForward();
        }

        return lexer.Current;
    }
    public static bool IsAlphaNumeric(char? c) => c is >= '0' and <= '9' or (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '_';

    public static bool IsSpace(char? target) => target is ' ' or '\t' or '\n' or '\v' or '\f' or '\r';
}