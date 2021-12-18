using CleanUpLogs.Console.BusinessLogic;
using NUnit.Framework;

namespace CleanUpLogs.Console.Tests.BusinessLogic
{
  public abstract class CleanUpLogsLogicTestsBase
  {
    protected CleanUpLogsLogic _cull;

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