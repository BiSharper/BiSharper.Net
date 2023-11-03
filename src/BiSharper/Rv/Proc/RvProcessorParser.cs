using System.Text;
using BiSharper.Common.Lex;
using BiSharper.Common.Parse;

namespace BiSharper.Rv.Proc;

public partial class RvProcessorContext: IProcessed
{
    public void Process(ref Lexer lexer)
    {
        var memory = new MemoryStream(new byte[1024], true);
        var writer = new StreamWriter(memory);
        var i = -1;

        var quoted = false;
        var newLine = true;
        while (!lexer.EOF)
        {
            if(ConsumeStrippedNot(lexer, '\r') is not { } current) break;

            if (quoted)
            {
                writer.Write(lexer.ConsumeUntil('"'));
                quoted = false;
                goto Continue;
            }
            
            if (ValidIdChar(current, true))
            {
                ProcessIdentifier(lexer, current, false, writer);
                goto Continue;
            }

            switch (current)
            {
                case '#':
                {
                    var nextAfterHash = ConsumeStripped(lexer);
                    if (newLine && nextAfterHash != '#')
                    {
                        newLine = false;
                        var directive = ConsumeIdentifier(lexer, 1024, ConsumeSpace(lexer));
                        if (!AssertDirectiveSpace(lexer)) throw new Exception($"Expected space after {directive} directive!");

                        switch (directive)
                        {
                            case "include":
                                ProcessIncludeDirective(lexer, writer);
                                break;
                            case "define":
                                ProcessDefineDirective(lexer);
                                break;
                            case "ifdef":
                                ProcessIfDirective(lexer, false, writer);
                                break;
                            case "ifndef":
                                ProcessIfDirective(lexer, true, writer);
                                break;
                            case "undef":
                                ProcessUndefineDirective(lexer);
                                break;
                            default:
                                throw new Exception("Unknown directive!");
                        }
                        break;
                    }
                    ProcessIdentifier(lexer, null, true, writer);
                    break;
                }
                case '/':
                    if ( ConsumeSpace(lexer) is not { } nextAfterSlash)
                    {
                        writer.Write('/');
                        break;
                    }

                    switch (nextAfterSlash)
                    {
                        case '/':
                            TraverseLineComment(lexer);
                            goto Continue;
                        case '*': 
                            TraverseBlockComment(lexer);
                            break;
                        default: 
                            writer.Write('/');
                            break;
                    }
                    break;
                case '\n':
                    newLine = true;
                    continue;
                case '"':
                    quoted = !quoted;
                    writer.Write(current);
                    break;
                
                default: writer.Write(current); break;
            }
            
            Continue:
            if (newLine) newLine = false;
        }
        writer.Flush();
        lexer = new Lexer(memory, lexer.Encoding, lexer.CacheSize);
    }

    //todo
    private void ProcessDefineDirective(Lexer lexer)
    {
    }
    
    //todo
    private void ProcessUndefineDirective(Lexer lexer)
    {
    }

    //todo
    private void ProcessIfDirective(Lexer lexer, bool negated, StreamWriter writer)
    {
    }
    
    //todo
    private void ProcessIncludeDirective(Lexer lexer, StreamWriter writer)
    {
    }
    
    //todo
    private void ProcessIdentifier(Lexer lexer, char? useFirst, bool mustExist, StreamWriter writer)
    {
        var identifier = ConsumeIdentifier(lexer, 1024, useFirst);
    }
    
    private static bool AssertDirectiveSpace(Lexer lexer)
    {
        if(ConsumeStrippedNot(lexer, '\r') is not { } next) return false;
        lexer.ConsumeCountNot(' ', out var spaceCount);
        return spaceCount > 0;
    }
    
    private static char? ConsumeSpace(Lexer lexer)
    {
        while (lexer.Current < 33 && lexer.Current != '\n' && !lexer.EOF)
        {
            lexer.StepForward();
        }

        return lexer.Current;
    }

    private static string ConsumeIdentifier(
        Lexer lexer,
        uint maxSize,
        char? useFirst
    )
    {
        var isFirst = !useFirst.HasValue;
        var bytes = IterateByCondition(lexer, maxSize, useFirst, current =>
        {
            bool isValid = ValidIdChar(current, isFirst);
            isFirst = false;
            return isValid;
        });
        return bytes.ToString();
    }

    private static Span<char> IterateByCondition(
        Lexer lexer,
        uint maxSize,
        char? useFirst,
        Func<char, bool> checkCondition
    )
    {
        if (lexer.EOF) throw new Exception("Premature EOF.");
        Span<char> buffer = new char[maxSize];
        if (maxSize == 0) return buffer;
        var i = 0;
        var current = useFirst ?? (ConsumeStripped(lexer) ?? throw new Exception("Premature EOF."));
        while (checkCondition(current!))
        {
            
            buffer[i] = current;
            

            if (++i == maxSize || lexer.EOF)
            {
                return buffer;
            }

            current = ConsumeStripped(lexer) ?? throw new Exception("Premature EOF.");
        }
        
        lexer.StepBackward();
        return buffer;
    }

    private static bool ValidIdChar(char c, bool isFirst) =>  isFirst && c >= '0' || c <= '9' ||
        c >= 'a' ||
        c <= 'z' ||
        c >= 'A' ||
        c <= 'Z' ||
        c == '_';

    private static char? ConsumeStripped(Lexer lexer)
    {
        var current = lexer.ConsumeNot('\r');
        while (current == '\\')
        {
            if (lexer.ConsumeNot('\r') != '\n')
            {
                lexer.StepBackward();
                return '\r';
            }

            current = lexer.ConsumeNot('\r');
        }

        return current;
    }
    
    private static char? ConsumeStrippedNot(Lexer lexer, char target)
    {
        var last = ConsumeStripped(lexer);
        while (last == target && !lexer.EOF)
        {
            last = ConsumeStripped(lexer);
        }

        return last;
    }
    
    private static void TraverseBlockComment(Lexer lexer)
    {
        char? last = null;
        if (lexer.Consume() is not { } current)
        {
            return;
        }

        while (!lexer.EOF && (last != '*' || current != '/'))
        {
            last = current;

            current = lexer.Consume() ?? throw new IOException("Unexpected EOF");
        }
    }
    
    private static void TraverseLineComment(Lexer lexer)
    {
        if (lexer.Consume() is not { } current)
        {
            return;
        }

        while (!lexer.EOF && current != '\n')
        {
            current = lexer.Consume() ?? throw new IOException("Unexpected EOF");
        }
    }
}