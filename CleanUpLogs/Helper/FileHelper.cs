using System;

namespace CleanUpLogs.Console.Helper
{
  public class FileHelper
  {
    public static string[] ReadLines(string path)
    {
      if (string.IsNullOrEmpty(path))
        return new string[] { };
      try
      {
        return System.IO.File.ReadAllLines(path);
      }
      catch (Exception e)
      {
        System.Console.WriteLine(e.Message);
      }
      return new string[] { };
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
