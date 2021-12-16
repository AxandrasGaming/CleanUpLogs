using CleanUpLogs.Console.Helper;
using NUnit.Framework;
using System.IO;

namespace CleanUpLogs.Console.Tests
{
  public class FileHelperTests
  {
    private string _tempFileName;

    [SetUp]
    public void Setup()
    {
      _tempFileName = Path.GetTempFileName();
    }

    [TestCase("Hallo", "Hallo")]
    public void FileHandler_ReadLinesFromFile_True(string text, string expected)
    {
      File.AppendAllText(_tempFileName, text);

      var result = FileHelper.ReadLines($@"{_tempFileName}");

      Assert.AreEqual(expected, result[0]);
    }

    [TestCase("Halo", "Hallo")]
    public void FileHandler_ReadLinesFromFile_False(string text, string expected)
    {
      File.AppendAllText(_tempFileName, text);

      var result = FileHelper.ReadLines($@"{_tempFileName}");

      Assert.AreNotEqual(expected, result[0]);
    }

    [TestCase(new string[] { "Hallo", "das ist ein Test", "Auf Wiedersehen" }, true)]
    [TestCase(new string[] { }, true)]
    [TestCase(null, false)]
    public void FileHandler_WriteLinesIntoFiles(string[] text, bool expected)
    {
      bool result = FileHelper.WriteLines($@"{_tempFileName}", text);

      Assert.AreEqual(expected, result);
    }

    [TearDown]
    public void CleanUp()
    {
      if (Directory.Exists(_tempFileName))
        File.Delete(_tempFileName);
      _tempFileName = string.Empty;
    }

  }
}