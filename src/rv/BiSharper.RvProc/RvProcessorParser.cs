using System.Diagnostics.CodeAnalysis;
using BiSharper.Common.Language;

namespace BiSharper.RvProc;

public static class RvProcessorParser
{
    public static void Process(ref Lexer lexer, RvProcessorContext context)
    {
        Span<char> preprocessed = stackalloc char[lexer.Length];
        var i = -1;

        var quoted = false;
        var newLine = true;
        while (!lexer.IsEOF() && ++i <= lexer.Length)
        {
            if(lexer.ConsumeStrippedNot('\r') is not { } current) break;

            if (quoted)
            {
                lexer.ConsumeUntil('"');
                quoted = false;
                goto Continue;
            }
            
            if (current.ValidIdChar(true))
            {
                lexer.ProcessIdentifier(current, false, context, ref preprocessed);
                goto Continue;
            }

            switch (current)
            {
                case '#':
                {
                    var nextAfterHash = lexer.ConsumeStripped();
                    if (newLine && nextAfterHash != '#')
                    {
                        newLine = false;
                        var directive = lexer.ConsumeIdentifier(1024, lexer.ConsumeSpace());
                        if (!lexer.AssertDirectiveSpace()) throw new Exception($"Expected space after {directive} directive!");

                        switch (directive)
                        {
                            case "include":
                                lexer.ProcessIncludeDirective(context, ref preprocessed);
                                break;
                            case "define":
                                lexer.ProcessDefineDirective(context);
                                break;
                            case "ifdef":
                                lexer.ProcessIfDirective(false, context, ref preprocessed);
                                break;
                            case "ifndef":
                                lexer.ProcessIfDirective(true, context, ref preprocessed);
                                break;
                            case "undef":
                                lexer.ProcessUndefineDirective(context);
                                break;
                            default:
                                throw new Exception("Unknown directive!");
                        }
                        break;
                    }
                    lexer.ProcessIdentifier(null, true, context, ref preprocessed);
                    break;
                }
                case '/':
                    if ( lexer.ConsumeSpace() is not { } nextAfterSlash)
                    {
                        preprocessed[i] = '/'; 
                        break;
                    }

                    switch (nextAfterSlash)
                    {
                        case '/':
                            lexer.TraverseLineComment();
                            goto Continue;
                        case '*': 
                            lexer.TraverseBlockComment();
                            break;
                        default: 
                            preprocessed[i] = '/'; 
                            break;
                    }
                    break;
                case '\n':
                    newLine = true;
                    continue;
                case '"':
                    quoted = !quoted;
                    preprocessed[i] = current; 
                    break;
                
                default: preprocessed[i] = current; break;
            }
            
            Continue:
            if (newLine) newLine = false;
        }

        lexer = new Lexer(preprocessed.ToArray());
    }

    //todo
    private static void ProcessDefineDirective(this Lexer lexer, RvProcessorContext context)
    {
    }
    
    //todo
    private static void ProcessUndefineDirective(this Lexer lexer, RvProcessorContext context)
    {
    }

    //todo
    private static void ProcessIfDirective(this Lexer lexer, bool negated, RvProcessorContext context, ref Span<char> preprocessed)
    {
    }
    
    //todo
    private static void ProcessIncludeDirective(this Lexer lexer, RvProcessorContext context, ref Span<char> preprocessed)
    {
    }
    
    //todo
    private static void ProcessIdentifier(this Lexer lexer, char? useFirst, bool mustExist, RvProcessorContext context, ref Span<char> preprocessed)
    {
        var identifier = lexer.ConsumeIdentifier(1024, useFirst);
    }
    
    private static bool AssertDirectiveSpace(this Lexer lexer)
    {
        if(lexer.ConsumeStrippedNot('\r') is not { } next) return false;
        lexer.ConsumeCountNot(' ', out var spaceCount);
        return spaceCount > 0;
    }
    
    private static char? ConsumeSpace(this Lexer lexer)
    {
        while (lexer.Current < 33 && lexer.Current != '\n' && !lexer.IsEOF())
        {
            lexer.StepForward();
        }

        return lexer.Current;
    }

    private static string ConsumeIdentifier(
        this Lexer lexer,
        uint maxSize,
        char? useFirst
    )
    {
        var isFirst = !useFirst.HasValue;
        var bytes = lexer.IterateByCondition(maxSize, useFirst, current =>
        {
            bool isValid = current.ValidIdChar(isFirst);
            isFirst = false;
            return isValid;
        });
        return bytes.ToString();
    }

    private static Span<char> IterateByCondition(
        this Lexer lexer,
        uint maxSize,
        char? useFirst,
        Func<char, bool> checkCondition
    )
    {
        if (lexer.IsEOF()) throw new Exception("Premature EOF.");
        Span<char> buffer = new char[maxSize];
        if (maxSize == 0) return buffer;
        var i = 0;
        var current = useFirst ?? (lexer.ConsumeStripped() ?? throw new Exception("Premature EOF."));
        while (checkCondition(current!))
        {
            
            buffer[i] = current;
            

            if (++i == maxSize || lexer.IsEOF())
            {
                return buffer;
            }

            current = lexer.ConsumeStripped() ?? throw new Exception("Premature EOF.");
        }
        
        lexer.StepBack();
        return buffer;
    }

    private static bool ValidIdChar(this char c, bool isFirst) =>  isFirst && c >= '0' || c <= '9' ||
        c >= 'a' ||
        c <= 'z' ||
        c >= 'A' ||
        c <= 'Z' ||
        c == '_';

    private static char? ConsumeStripped(this Lexer lexer)
    {
        var current = lexer.ConsumeNot('\r');
        while (current == '\\')
        {
            if (lexer.ConsumeNot('\r') != '\n')
            {
                lexer.StepBack();
                return '\r';
            }

            current = lexer.ConsumeNot('\r');
        }

        return current;
    }
    
    private static char? ConsumeStrippedNot(this Lexer lexer, char target)
    {
        var last = ConsumeStripped(lexer);
        while (last == target && !lexer.IsEOF())
        {
            last = ConsumeStripped(lexer);
        }

        return last;
    }
    
    private static void TraverseBlockComment(this Lexer lexer)
    {
        char? last = null;
        if (lexer.Consume() is not { } current)
        {
            return;
        }

        while (!lexer.IsEOF() && (last != '*' || current != '/'))
        {
            last = current;

            current = lexer.Consume() ?? throw new IOException("Unexpected EOF");
        }
    }
    
    private static void TraverseLineComment(this Lexer lexer)
    {
        if (lexer.Consume() is not { } current)
        {
            return;
        }

        while (!lexer.IsEOF() && current != '\n')
        {
            current = lexer.Consume() ?? throw new IOException("Unexpected EOF");
        }
    }
}