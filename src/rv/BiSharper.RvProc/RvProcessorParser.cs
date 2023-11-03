using BiSharper.Common.Language;

namespace BiSharper.RvProc;

public static class RvProcessorParser
{
    public static void Process(ref Lexer lexer, RvProcessorContext context)
    {
        Span<char> preprocessed = stackalloc char[lexer.Length];
        var i = -1;

        var quoted = false;
        while (!lexer.IsEOF() && ++i <= lexer.Length)
        {
            if(lexer.ConsumeStrippedNot('\r') is not { } current) break;
            if (current.ValidIdChar(true))
            {
                lexer.ProcessIdentifier(current, false, context, ref preprocessed);
                continue;
            }

            switch (current)
            {
                case '"': quoted = !quoted; continue;
                case '#':
                {
                    if (!lexer.Take('#'))
                    {
                        lexer.ProcessDirective(context, ref preprocessed);
                        continue;
                    }
                    lexer.ProcessIdentifier(null, true, context, ref preprocessed);
                    continue;
                }
                default: preprocessed[i] = current; continue;
            }
        }

        lexer = new Lexer(preprocessed.ToArray());
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