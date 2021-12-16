using CleanUpLogs.Console.Helper;
using NUnit.Framework;

namespace CleanUpLogs.Console.Tests.Helper
{
  class FileExtensionManagerTests
  {
    private IExtensionManager _extensionManager;
    private ExtensionManagerFactory _extensionManagerFactory;

    [SetUp]
    public void Setup()
    {
      _extensionManagerFactory = new ExtensionManagerFactory();
      _extensionManagerFactory.SetManager(new StubFileExtensionManager());
      _extensionManager = _extensionManagerFactory.Create();
    }

    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.txt", false)]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.LOG", false)]
    [TestCase(@"..\netcoreapp3.1\Resources\testdatei.log", true)]
    public void IsValidPath_PathWithFileExtension(string path, bool expected)
    {
      bool result = _extensionManager.IsValidPath(path);

      Assert.AreEqual(expected, result);
    }
    
    [Test]
    public void IsValidPath_SupportedFileExtension_True()
    {
      _extensionManager = _extensionManagerFactory.Create();
      bool result = _extensionManager.IsValidPath("");

      Assert.IsFalse(result);
    }

    [TearDown]
    public void CleanUp()
    {
      _extensionManager = null;
    }
  }
}
