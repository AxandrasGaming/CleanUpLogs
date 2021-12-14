using CleanUpLogs.Logic.Helper;
using System;
using System.Collections.Generic;
using System.Text;

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
        if (IsValidToDelete(line))
          lines[posLines] = DeleteStringFromLine(line, $@"\b\u001b[K");
      }
      return lines;
    }

    private string DeleteStringFromLine(string line, string delString)
    {
      int startPos = line.IndexOf(delString);
      return line.Remove(startPos - 1, delString.Length + 1);
    }

    private bool IsValidToDelete(string line)
    {
      return line.Contains('\b') && line.Contains('\u001b') && line.Contains('[') && line.Contains('K');
    }

    public string DeleteCharFromLine(string line, char delChar)
    {
      int lineLength = line.Length;
      int deletedChars = 0;
      for (int posCharacter = 0; posCharacter < lineLength; posCharacter++)
      {
        int pos = posCharacter - deletedChars;
        char d = line[pos];

        if (d == delChar)
        {
          deletedChars++;
          line = line.Remove(pos, 1);
        }
      }
      return line;
    }
  }
}
