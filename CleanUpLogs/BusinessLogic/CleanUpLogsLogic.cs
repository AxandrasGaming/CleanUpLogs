using CleanUpLogs.Console.Helper;
using System;
using System.Text.RegularExpressions;

namespace CleanUpLogs.Console.BusinessLogic
{
  public class CleanUpLogsLogic
  {
    public CleanUpLogsLogic() { }

    public string[] ReadContentOfFile(string path)
    {
      return FileHelper.ReadLines(path);
    }

    public string[] AlterLines(string[] lines)
    {
      for (int posLines = 0; posLines < lines.Length; posLines++)
      {
        string line = lines[posLines];

        line = DeleteStringFromBeginning(line);

        while (IsValidToDelete(line))
        {
          string deleteStringFlag = StringContainsDeletionKeyStroke(line);
          int startPos = line.IndexOf(deleteStringFlag);
          if (!IsValidStartPositon(startPos))
            break;
          string stringToDelete = DetermineStringToDelete(line, deleteStringFlag, startPos);

          line = DeleteStringFromLine(line, stringToDelete);
        }
        lines[posLines] = line;
      }
      return lines;
    }

    private static string DetermineStringToDelete(string line, string deleteStringFlag, int startPos)
    {
      string stringToDelete = string.Empty;
      if (IsValidStartPositon(startPos - 1) && deleteStringFlag.Contains("\b\u001b[K"))
      {
        stringToDelete = GetStringToDelete(line, deleteStringFlag, startPos);
      }
      else if (IsValidStartPositon(startPos - 1) && deleteStringFlag.Contains("\b"))
      {
        stringToDelete = GetStringToDelete(line, deleteStringFlag, startPos);
      }
      else if (IsValidStartPositon(startPos - 1) && deleteStringFlag.Contains("\u001b[K"))
      {
        stringToDelete = GetStringToDelete(line, deleteStringFlag, startPos);
      }
      if (IsValidStartPositon(startPos) && deleteStringFlag.Contains("\u001b[K"))
      {
        stringToDelete = deleteStringFlag;
      }
      if (deleteStringFlag.Contains("[0m"))
      {
        stringToDelete = deleteStringFlag;
      }
      if (deleteStringFlag.Contains("[01"))
      {
        int pos = startPos + 4;
        stringToDelete = deleteStringFlag + String.Format("{0}{1}{2}{3}",
                                            line[pos],
                                            line[pos + 1],
                                            line[pos + 2],
                                            line[pos + 3]);
      }
      if (deleteStringFlag.Contains("\u001b[C"))
      {
        stringToDelete = deleteStringFlag;
      }

      return stringToDelete;
    }

    private static string GetStringToDelete(string line, string deleteStringFlag, int startPos)
    {
      return line[startPos - 1].ToString() + deleteStringFlag;
    }

    private static bool IsValidStartPositon(int startPos)
    {
      return startPos >= 0;
    }

    private string StringContainsDeletionKeyStroke(string line)
    {
      if (line.Contains("\b\u001b[K")) return "\b\u001b[K";
      if (line.Contains($"\\b\\u001b[K")) return $"\\b\\u001b[K";
      if (line.Contains($@"\b\u001b[K")) return $@"\b\u001b[K";
      if (line.Contains("\b")) return "\b";
      if (line.Contains("\u001b[K")) return "\u001b[K";
      if (line.Contains("\u001b[01")) return "\u001b[01";
      if (line.Contains("\u001b[0m")) return "\u001b[0m";
      if (line.Contains("\u001b[C")) return "\u001b[C";

      string pattern = "\\u001b\\[K(.*)";
      Regex reg = new Regex(pattern);
      if (reg.IsMatch(line)) return "\u001b[K";
      return string.Empty;
    }

    private string DeleteStringFromBeginning(string line)
    {
      string[] split;
      string pattern = "\\u001b]0;(.*)~?\\a";
      Regex reg = new Regex(pattern);
      if (reg.IsMatch(line))
      {
        split = reg.Split(line);
        return split[2];
      }
      return line;
    }

    private string DeleteStringFromLine(string line, string delString)
    {
      if (string.IsNullOrEmpty(line)) return line;
      return line.Replace(delString, "");
    }

    private bool IsValidToDelete(string line)
    {
      string pattern = "\\u001b\\[K(.*)";
      Regex reg = new Regex(pattern);
      return reg.IsMatch(line)
        || line.Contains("\b")
        || line.Contains($"\\b\\u001b[K")
        || line.Contains($"\\u001b[K")
        || line.Contains($@"\b\u001b[K")
        || line.Contains($"\u001b[01")
        || line.Contains($"\u001b[0m")
        || line.Contains("\u001b[C");
    }

    public bool WriteContentToFiles(string path, string[] lines)
    {
      if (string.IsNullOrEmpty(path)) return false;
      if (lines == null) return false;
      try
      {
        FileHelper.WriteLines(path, lines);
      }
      catch (Exception)
      {
        return false;
      }

      return true;
    }
  }
}
