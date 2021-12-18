using CleanUpLogs.Console.BusinessLogic;
using NUnit.Framework;

namespace CleanUpLogs.Console.Tests.BusinessLogic
{
  class CleanUpLogsLogicTests : CleanUpLogsLogicTestsBase
  {
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

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log",
              new string[] { @"[kurs@localhost ~]$ sudo su -" })]
    public void AlterLines_DeleteMultipleSpecificChar_True(string path, string[] expected)
    {
      _cull.ReadContentOfFile(path);

      string[] lines = _cull.Lines;
      string[] result = _cull.AlterLines(lines);
      string expectedString = expected[0];
      string resultString = result[2];
      Assert.AreEqual(expectedString, resultString);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log",
              new string[] { "[kurs@localhost ~]$ sudo su -" })]
    public void AlterLines_DeleteLineBeginning_True(string path, string[] expected)
    {
      // From \u001b to ~\a

      _cull.ReadContentOfFile(path);

      string[] lines = _cull.Lines;
      string[] result = _cull.AlterLines(lines);
      string expectedString = expected[0];
      string resultString = result[2];
      StringAssert.AreEqualIgnoringCase(expectedString, resultString);
    }




    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log",
          new string[] { "[kurs@localhost ~]$ sudo su -", "[root@localhost ~]# # ls -la" })]
    public void AlterLines_DeleteMultipleLineBeginnings_True(string path, string[] expected)
    {
      // From \u001b to ~\a

      _cull.ReadContentOfFile(path);

      string[] lines = _cull.Lines;
      string[] result = _cull.AlterLines(lines);

      StringAssert.AreEqualIgnoringCase(expected[0], result[2]);
      StringAssert.AreEqualIgnoringCase(expected[1], result[4]);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei_IST.log", new string[] {  })]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei_IST.log", new string[] { "This is a test" })]
    public void WriteLines_WriteALineToFile_True(string path,string[] lines)
    {
      bool result = _cull.WriteContentToFiles(path, lines);

      Assert.IsTrue(result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei_IST.log", new string[] { "This is a test", "Cadra is a Schlawiner", "GrinZ3 ist toll" })]
    public void WriteLines_WriteMultipleLinesToFile_True(string path, string[] lines)
    {
      bool result = _cull.WriteContentToFiles(path, lines);

      Assert.IsTrue(result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei_IST.log", null)]
    [TestCase("", new string[] { "This is a test", "Cadra is a Schlawiner", "GrinZ3 ist toll" })]
    public void WriteLines_WriteMultipleLinesToFile_False(string path, string[] lines)
    {
      bool result = _cull.WriteContentToFiles(path, lines);

      Assert.IsFalse(result);
    }

    [TearDown]
    public void TearDown()
    {
      _cull = null;
    }

  }
}
