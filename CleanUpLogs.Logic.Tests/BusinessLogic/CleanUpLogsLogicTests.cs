using NUnit.Framework;
using System.Linq;

namespace CleanUpLogs.Logic.Tests
{
  class CleanUpLogsLogicTests : CleanUpLogsLogicTestsBase
  {
    #region ReadTests
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log")]
    public void ReadContentOfFile_True(string path)
    {
      string[] lines = _cull.ReadContentOfFile(path);

      Assert.AreEqual
      (
"##############################################################################################",
        lines[0]
      );
    }
    #endregion

    #region AlterationTests
    #region DeletionTests

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", @"[kurs@localhost ~]$ sudo su -")]
    public void AlterLines_Delete1bANDKMultipleSpecificChar_True(string path, string expected)
    {
      // \\b\\u001b[K

      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == (expected));
      StringAssert.AreEqualIgnoringCase(expected, result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "[kurs@localhost ~]$ sudo su -")]
    public void AlterLines_Delete1BAndAAtLineBeginningWithTilde_True(string path, string expected)
    {
      // From \u001b to \a + ~

      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == (expected));
      StringAssert.AreEqualIgnoringCase(expected, result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "[root@server1 etc]# ls -la")]
    public void AlterLines_Delete1BAndAAtLineBeginningWithoutTilde_True(string path, string expected)
    {
      // From \u001b to \a - ~

      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == (expected));
      StringAssert.AreEqualIgnoringCase(expected, result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "[kurs@localhost ~]$ sudo su -")]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "[root@localhost ~]# # ls -la")]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "[root@server1 etc]# ls -la")]
    public void AlterLines_Delete1BANDAWithAndWithoutTildeMultipleLineAtBeginnings_True(string path, string expected)
    {
      // From \u001b to ~ +/- \a

      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == expected);
      StringAssert.AreEqualIgnoringCase(expected, result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "[root@localhost ~]# ")]
    public void AlterLines_DeleteBackSpaceAtBeginning_True(string path, string expected)
    {
      // \\u001b\\[K
      

      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == expected);
      StringAssert.AreEqualIgnoringCase(expected, result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", "dr-xr-x---.  5 root root  184  4. Okt 11:29 .")]
    public void AlterLines_Delete01AND0mBeginning_True(string path, string expected)
    {
      // [01 + [0m
      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == (expected));
      StringAssert.AreEqualIgnoringCase(expected, result);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log")]
    public void AlterLines_DeleteBackSpaceCharacter_True(string path)
    {
      // \b

      string expected = "[root@server1 etc]# u\u001b[1P";
      string[] lines = _cull.ReadContentOfFile(path);
      lines = _cull.AlterLines(lines);
      string result = lines.FirstOrDefault(_ => _ == (expected));
      StringAssert.AreEqualIgnoringCase(expected, result);
    }



    #endregion
    #endregion

    #region WriteTests

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei_IST.log", new string[] { })]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei_IST.log", new string[] { "This is a test" })]
    public void WriteLines_WriteALineToFile_True(string path, string[] lines)
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
    #endregion
  }
}
