using NUnit.Framework;

namespace CleanUpLogs.Console.Tests
{
  public class CleanUpLogsConsoleTests
  {
    private CleanUpLogsConsole _culc;
    #region ParamterTests
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

      Assert.IsTrue(_culc.FlagCollection.ContainsKey(Flags.None));
    }

    [Test]
    public void CleanUpLogsConsole_WithParameterPathCorrectFormat_Test()
    {
      _culc = new CleanUpLogsConsole(new string[] { "-f=C:\\path", "Dieter", "-o=k" });

      Assert.AreEqual("C:\\path", _culc.FlagCollection.ContainsKey(Flags.SourcePath) ? _culc.FlagCollection[Flags.SourcePath] : string.Empty);
    }

    [TestCase(new string[] { @"-f=E:\path", "Dieter", @"-d=E:\path\output" }, @"E:\path", Flags.SourcePath)]
    [TestCase(new string[] { @"-f=E:\path", "Dieter", @"-d=E:\path\output" }, @"E:\path\output", Flags.DestinationPath)]
    public void CleanUpLogsConsole_WithMultiplePathParameters_Test(string[] paramArray, string expected, Flags flag)
    {
      _culc = new CleanUpLogsConsole(paramArray);

      StringAssert.AreEqualIgnoringCase(expected, _culc.FlagCollection.ContainsKey(flag) ? _culc.FlagCollection[flag] : string.Empty);
    }

    #endregion 

  }
}