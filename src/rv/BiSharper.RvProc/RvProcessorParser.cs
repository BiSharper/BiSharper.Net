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
                        lexer.StepBack();
                        
                        lexer.ProcessDirective(context, ref preprocessed);
                        break;
                    }
                    lexer.ProcessIdentifier(null, true, context, ref preprocessed);
                    break;
                }
                case '/':
                    var nextAfterSlash = lexer.ConsumeSpace();
                    if (nextAfterSlash is not { } next)
                    {
                        preprocessed[i] = '/'; 
                        break;
                    }

                    switch (next)
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

    private static void TraverseBlockComment(this Lexer lexer)
    {
        throw new NotImplementedException();
    }
    
    private static void TraverseLineComment(this Lexer lexer)
    {
        throw new NotImplementedException();
    }

    private static void ProcessDirective(this Lexer lexer, RvProcessorContext context, ref Span<char> preprocessed)
    {
        throw new NotImplementedException();
    }
    
    private static void ProcessIdentifier(this Lexer lexer, char? useFirst, bool mustExist, RvProcessorContext context, ref Span<char> preprocessed)
    {
        var identifier = lexer.ConsumeIdentifier(1024, useFirst);

        throw new NotImplementedException();
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
}