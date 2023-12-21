using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.Writer;

public delegate void ParamWriterDelegate(ParamContext root, Stream output);