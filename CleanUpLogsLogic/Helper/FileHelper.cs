using System;

namespace CleanUpLogs.Logic.Helper
{
  public class FileHelper
  {
    public static string[] ReadLines(string path)
    {
      if (string.IsNullOrEmpty(path))
        return null;

      return System.IO.File.ReadAllLines(path);
    }

    public static bool WriteLines(string path, string[] text)
    {
      if (string.IsNullOrEmpty(path)) return false;
      if (text == null) return false;
      try
      {
        System.IO.File.WriteAllLines(path, text);
      }
      catch (Exception)
      {
        return false;
      }

      return true;
    }
  }
}
