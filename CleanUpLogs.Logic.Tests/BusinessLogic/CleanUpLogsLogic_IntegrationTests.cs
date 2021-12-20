using NUnit.Framework;
using System.IO;

namespace CleanUpLogs.Logic.Tests
{
  public class CleanUpLogsLogic_IntegrationTests : CleanUpLogsLogicTestsBase
  {
    // A Test behaves as an ordinary method
    
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", @"..\netcoreapp3.1\Resources\testdatei_SOLL.log", @"..\netcoreapp3.1\Resources\testdatei_IST.log")]
    [Ignore("Slow test just for Integration!")]
    public void Integration_WholeCRUDProcess_True(string pathSource, string pathExpected, string pathResult)
    {
      _cull.ReadContentOfFile(pathSource);
      _cull.AlterLines(_cull.Lines);
      bool result = _cull.WriteContentToFiles(pathResult,_cull.Lines);
      if(result)
      {
        Stream streamExpected = new FileStream(pathExpected, FileMode.Open);
        Stream streamResult = new FileStream(pathResult, FileMode.Open);
        FileAssert.AreEqual(streamExpected, streamResult);
      }
    }
  }
}