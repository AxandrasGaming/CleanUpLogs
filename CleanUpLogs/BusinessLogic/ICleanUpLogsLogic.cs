namespace CleanUpLogs.Console.BusinessLogic
{
  public interface ICleanUpLogsLogic
  {
    string[] ReadContentOfFile(string path);
    string[] AlterLines(string[] lines);
    bool WriteContentToFile(string path, string[] lines);
  }
}