using CleanUpLogs.Console.Helper;

namespace CleanUpLogs.Console.Tests.Helper
{
  class StubFileExtensionManager : IExtensionManager
  {
    public bool IsValidPath(string path)
    {
      if (string.IsNullOrEmpty(path) || !path.Contains(".log"))
        return false;
      return true;
    }
  }
}
