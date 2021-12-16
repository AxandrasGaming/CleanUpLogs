using System;
using System.Collections.Generic;
using System.Text;

namespace CleanUpLogs.Console.Helper
{
  public class FileExtensionManager : IExtensionManager
  {
    public bool IsValidPath(string path)
    {
      if (string.IsNullOrEmpty(path) || !path.Contains(".log"))
        return false;
      return true;
    }
  }
}
