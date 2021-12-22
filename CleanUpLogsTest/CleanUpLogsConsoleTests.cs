using NUnit.Framework;

namespace CleanUpLogs.Console.Tests
{
  public class CleanUpLogsConsoleTests
  {
    private CleanUpLogsConsole _culc;
    #region ParamterTests
    [TestCase(new string[] { "Hallo", "Dieter", "-o" }, Flags.None)]
    [TestCase(new string[] { "Hallo", "Dieter", "-o=k" }, Flags.None)]
    [TestCase(new string[] { "Hallo", "Dieter", "-s" }, Flags.SourcePath)]
    [TestCase(new string[] { "Hallo", "Dieter", "-S" }, Flags.SourcePath)]
    [TestCase(new string[] { "Hallo", "Dieter", "-d" }, Flags.DestinationPath)]
    [TestCase(new string[] { "Hallo", "Dieter", "-D" }, Flags.DestinationPath)]
    public void CleanUpLogsConsole_WithParameterWithSpecificFlag_False(string[] args, Flags flag)
    {
      _culc = new CleanUpLogsConsole(args);
      Assert.IsFalse(_culc.FlagCollection.ContainsKey(flag));
    }

    [TestCase(new string[] { "Hallo", "Dieter", "-s=a " }, Flags.SourcePath)]
    [TestCase(new string[] { "Hallo", "Dieter", "-S=cb" }, Flags.SourcePath)]
    [TestCase(new string[] { "Hallo", "Dieter", "-d=de " }, Flags.DestinationPath)]
    [TestCase(new string[] { "Hallo", "Dieter", "-D=f " }, Flags.DestinationPath)]
    public void CleanUpLogsConsole_WithParameterWithSpecificFlag_True(string[] args, Flags flag)
    {
      _culc = new CleanUpLogsConsole(args);
      Assert.IsTrue(_culc.FlagCollection.ContainsKey(flag));
    }

    [Test]
    public void CleanUpLogsConsole_WithParameterPathCorrectFormat_Test()
    {
      _culc = new CleanUpLogsConsole(new string[] { "-s=C:\\path", "Dieter", "-o=k" });

      Assert.AreEqual("C:\\path", _culc.FlagCollection.ContainsKey(Flags.SourcePath) ? _culc.FlagCollection[Flags.SourcePath] : string.Empty);
    }

    [TestCase(new string[] { @"-s=E:\path", "Dieter", @"-d=E:\path\output" }, @"E:\path", Flags.SourcePath)]
    [TestCase(new string[] { @"-s=E:\path", "Dieter", @"-d=E:\path\output" }, @"E:\path\output", Flags.DestinationPath)]
    public void CleanUpLogsConsole_WithMultiplePathParameters_Test(string[] paramArray, string expected, Flags flag)
    {
      _culc = new CleanUpLogsConsole(paramArray);

      StringAssert.AreEqualIgnoringCase(expected, _culc.FlagCollection.ContainsKey(flag) ? _culc.FlagCollection[flag] : string.Empty);
    }
    #endregion 

  }
}