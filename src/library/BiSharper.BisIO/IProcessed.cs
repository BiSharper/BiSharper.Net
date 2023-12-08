using BiSharper.BisIO.IO;
using BiSharper.BisIO.IO.Read;

namespace BiSharper.BisIO;

public interface IProcessed
{
    void Process(ref TraversableAnalyser traversableAnalyser);
}
