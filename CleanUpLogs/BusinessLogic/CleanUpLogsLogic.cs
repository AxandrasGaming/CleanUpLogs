using CleanUpLogs.Console.Helper;
using System;
using System.Text.RegularExpressions;

namespace CleanUpLogs.Console.BusinessLogic
{
  public class CleanUpLogsLogic
  {
    private string[] _lines;

    public string[] Lines { get => _lines; set => _lines = value; }

    public CleanUpLogsLogic() { }

    public void ReadContentOfFile(string path)
    {
      Lines = FileHelper.ReadLines(path);
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
          string stringToDelete = string.Empty;
          string charactersToDelete = string.Empty;
          if (startPos - 1 >= 0 && deleteStringFlag.Contains("\b\u001b[K"))
          {
            charactersToDelete = line[startPos - 1].ToString();
            stringToDelete = charactersToDelete + deleteStringFlag;
          }
          else if (startPos - 1 >= 0 && deleteStringFlag.Contains("\b"))
          {
            charactersToDelete = line[startPos - 1].ToString();
            stringToDelete = charactersToDelete + deleteStringFlag;
          }
          else if (startPos - 1 >= 0 && deleteStringFlag.Contains("\u001b[K"))
          {
            charactersToDelete = line[startPos - 1].ToString();
            stringToDelete = charactersToDelete + deleteStringFlag;
          }
          if (startPos == 0 && deleteStringFlag.Contains("\u001b[K"))
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
            charactersToDelete = String.Format("{0}{1}{2}{3}",
                                                line[pos],
                                                line[pos + 1],
                                                line[pos + 2],
                                                line[pos + 3]);
            stringToDelete = deleteStringFlag + charactersToDelete;
          }

          line = DeleteStringFromLine(line, stringToDelete);
        }
        lines[posLines] = line;
      }
      return lines;
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
        || line.Contains($"\u001b[0m");
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
