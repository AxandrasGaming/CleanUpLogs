using CleanUpLogs.Logic.BusinessLogic;
using NUnit.Framework;

namespace CleanUpLogs.Logic.Tests.BusinessLogic
{
  class CleanUpLogsLogicTests
  {
    private CleanUpLogsLogic _cull;

    [SetUp]
    public void Setup()
    {
      _cull = new CleanUpLogsLogic();
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log")]
    public void Get_ReadContentOfFile_True(string path)
    {
      _cull.ReadContentOfFile(path);

      string[] lines = _cull.Lines;

      Assert.AreEqual
      (
"##############################################################################################",
        lines[0]
      );
    }

    [TestCase(new string[] { "Grinze ist toll", "Dem Inker fällt was auf" },
              new string[] { "Grnze st toll", "Dem Inker fällt was auf" })]
    public void AlterLines_DeleteChar_True(string[] lines, string[] expected)
    {
      int i = 0;
      foreach (string line in lines)
      {
        char delChar = 'i';
        string result = _cull.DeleteCharFromLine(line, delChar);
        StringAssert.AreEqualIgnoringCase(expected[i++], result);
      }
    }


    [TestCase(new string[] { @"# ls -lisah[K[K[K[Ka" },
              new string[] { @"# ls -la" })]
    public void AlterLines_DeleteSpecificChar_True(string[] lines, string[] expected)
    {
      string[] result = _cull.AlterLines(lines);
      string expectedString = expected[0];
      string resultString = result[0];
      StringAssert.AreEqualIgnoringCase(expectedString, resultString);
    }

    [TearDown]
    public void TearDown()
    {
      _cull = null;
    }

  }
}
