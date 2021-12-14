using CleanUpLogs.Logic.BusinessLogic;
using System.Collections.Generic;

namespace CleanUpLogs.Console
{
  public class CleanUpLogsConsole
  {
    #region Fields
    private string[] _args = null;
    private Dictionary<Flags, string> _flags = new Dictionary<Flags, string>();
    private CleanUpLogsLogic _cleanUpLogsLogic = new CleanUpLogsLogic();
    #endregion

    #region Properties
    public string[] Args { get => _args; set => _args = value; }
    public Dictionary<Flags, string> FlagCollection { get => _flags; set => _flags = value; }

    public CleanUpLogsLogic CleanUpLogsLogic { get => _cleanUpLogsLogic; set => _cleanUpLogsLogic = value; }
    #endregion

    #region Constructors
    public CleanUpLogsConsole(string[] args)
    {
      Args = args;
      StartLogCleaning();
    }

    public CleanUpLogsConsole() { }
    #endregion

    #region Methods
    private void StartLogCleaning()
    {
      if (_args == null) return;
      InitializeFlagCollection();
      if (!FlagCollection.ContainsKey(Flags.Path)) return;

      string path = FlagCollection[Flags.Path];
      if (!IsValidPath(path)) return;

      _cleanUpLogsLogic.ReadContentOfFile(path);
    }

    public bool IsValidPath(string path)
    {
      if (string.IsNullOrEmpty(path) || !path.Contains(".log"))
        return false;
      return true;
    }

    private void InitializeFlagCollection()
    {
      foreach (string arg in Args)
      {
        char[] sep = new char[] { '=' };
        string[] flag = arg.Split(sep);
        if (flag.Length == 2)
          AddFlagToDictionary(flag);
      }
    }

    private void AddFlagToDictionary(string[] flag)
    {
      Flags key = GetKeyFromString(flag[0]);
      if (key == Flags.Default) return;

      string value = flag[1];

      FlagCollection.Add(key, value);
    }

    private Flags GetKeyFromString(string key)
    {
      switch (key)
      {
        case "-f": return Flags.Path;
        default: return Flags.Default;
      }
    }
    #endregion
  }
}