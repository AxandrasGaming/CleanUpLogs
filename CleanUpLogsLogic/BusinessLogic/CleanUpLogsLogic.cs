using CleanUpLogs.Logic.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanUpLogs.Logic.BusinessLogic
{
  public class CleanUpLogsLogic
  {
    private string[] _lines;

    public string[] Lines { get => _lines; set => _lines = value; }

    public CleanUpLogsLogic()
    {

    }
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
          string deleteStringFlag = "\b\u001b[K";
          int startPos = line.IndexOf(deleteStringFlag);
          if (startPos < 0)
            break;
          char charToDelete = line[startPos - 1];
          string stringToDelete = charToDelete + deleteStringFlag;
          line = DeleteStringFromLine(line, stringToDelete);
        }
        lines[posLines] = line;
      }
      return lines;
    }

    private string DeleteStringFromBeginning(string line)
    {
      Regex reg = new Regex("\\u001b]0;(.*)~\\a");
      if (!reg.IsMatch(line))
        return line;
      string[] split = reg.Split(line);
      return split[2];
    }

    private string DeleteStringFromLine(string line, string delString)
    {
      return line.Replace(delString, "");
    }

    private bool IsValidToDelete(string line)
    {
      return line.Contains($"\b\u001b[K") || line.Contains($@"\b\u001b[K");
    }
  }
}
