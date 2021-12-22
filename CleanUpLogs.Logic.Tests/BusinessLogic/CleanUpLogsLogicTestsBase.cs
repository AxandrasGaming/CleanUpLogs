using CleanUpLogs.Console.BusinessLogic;
using CleanUpLogs.Console.Helper;
using NUnit.Framework;

namespace CleanUpLogs.Logic.Tests
{
  public abstract class CleanUpLogsLogicTestsBase
  {
    protected CleanUpLogsLogic _cull;

    [SetUp]
    public void Setup()
    {
      ExtensionManagerFactory emf = new ExtensionManagerFactory();
      _cull = new CleanUpLogsLogic(emf.Create());
    }

    [TearDown]
    public void TearDown()
    {
      _cull = null;
    }
  }
}