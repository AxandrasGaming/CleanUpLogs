using NUnit.Framework;

namespace CleanUpLogs.Console.Test
{
  public class CleanUpLogsConsoleTests
  {
    private CleanUpLogsConsole _culc;

    [TestCase(new string[] { "Hallo", "Dieter" }, new string[] { "Hallo", "Dieter" })]
    [TestCase(new string[] { }, new string[] { })]
    [TestCase(null, null)]
    public void Get_CleanUpLogsConsole_WithParameter_Test(string[] args, string[] expected)
    {
      _culc = new CleanUpLogsConsole(args);

      Assert.AreEqual(expected, _culc.Args);
    }

    [Test]
    public void Get_CleanUpLogsConsole_WithParameterOnSpecificPosition_Test()
    {
      _culc = new CleanUpLogsConsole(new string[] { "Hallo", "Dieter", "-o" });

      Assert.AreEqual("-o", _culc.Args[2]);
    }

    [Test]
    public void Get_CleanUpLogsConsole_WithParameterWithSpecificFlag_Test()
    {
      _culc = new CleanUpLogsConsole(new string[] { "Hallo", "Dieter", "-o=k" });

      Assert.IsFalse(_culc.FlagCollection.ContainsKey(Flags.Default));
    }

    [Test]
    public void CleanUpLogsConsole_WithParameterPathInCorrectFormat_Test()
    {
      _culc = new CleanUpLogsConsole(new string[] { "-f=C:\\path", "Dieter", "-o=k" });

      Assert.AreEqual("C:\\path", _culc.FlagCollection[0]);
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.txt", false)]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.LOG", false)]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", true)]
    public void IsValidPath_PathIncorrectFileExtension_False(string path, bool expected)
    {
      _culc = new CleanUpLogsConsole();
      bool result = _culc.IsValidPath(path);

      Assert.AreEqual(expected, result);
    }
  }
}