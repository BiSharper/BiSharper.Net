﻿using BiSharper.Common.Language;

namespace BiSharper.RvProc;

public class RvProcessorContext : IPreProcessor
{
    public void Process(ref Lexer lexer) => RvProcessorParser.PreProcess(ref lexer, this);
}