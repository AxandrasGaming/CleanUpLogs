using CleanUpLogs.Console.BusinessLogic;
using NUnit.Framework;

namespace CleanUpLogs.Logic.Tests
{
  public abstract class CleanUpLogsLogicTestsBase
  {
    protected ICleanUpLogsLogic _cull;

    [SetUp]
    public void Setup()
    {
      _cull = new CleanUpLogsLogic();
    }

    [TearDown]
    public void TearDown()
    {
      _cull = null;
    }
  }
}