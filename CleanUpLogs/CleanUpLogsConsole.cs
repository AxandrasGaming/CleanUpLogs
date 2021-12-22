using CleanUpLogs.Console.BusinessLogic;
using CleanUpLogs.Console.Helper;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CleanUpLogs.Console
{
  public class CleanUpLogsConsole
  {
    #region Fields
    private Dictionary<Flags, string> _flags = new Dictionary<Flags, string>();
    private ICleanUpLogsLogic _cleanUpLogsLogic = new CleanUpLogsLogic();
    private IExtensionManager _fileExtensionManager;
    #endregion

    #region Properties
    public Dictionary<Flags, string> FlagCollection { get => _flags; set => _flags = value; }
    #endregion

    #region Constructors
    public CleanUpLogsConsole(string[] args)
    {
      ExtensionManagerFactory emf = new ExtensionManagerFactory();
      _fileExtensionManager = emf.Create();
      StartLogCleaning(args);
    }

    public CleanUpLogsConsole() { }
    #endregion

    #region Methods
    private void StartLogCleaning(string[] args)
    {
      if (args == null) return;
      InitializeFlagCollection(args);
      if (!FlagCollectionHasValidFlags()) return;

      if (!FlagCollection.TryGetValue(Flags.SourcePath, out string sourcePath)) return;
      if (!_fileExtensionManager.IsValidPath(sourcePath)) return;

      string[] readLines = _cleanUpLogsLogic.ReadContentOfFile(sourcePath);
      if (readLines == null) return;

      string[] alteredLines = _cleanUpLogsLogic.AlterLines(readLines);

      if (!FlagCollection.TryGetValue(Flags.DestinationPath, out string destinationPath)) return;
      _cleanUpLogsLogic.WriteContentToFile(destinationPath, alteredLines);
      System.Console.WriteLine("Fertig mit der Bearbeitung.");
    }

    private bool FlagCollectionHasValidFlags()
    {
      return FlagCollection.Keys.Count > 0
        && FlagCollection.ContainsKey(Flags.SourcePath);
    }

    private void InitializeFlagCollection(string[] args)
    {
      foreach (string arg in args)
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
      if (key == Flags.None) return;

      string value = flag[1];
      FlagCollection.TryAdd(key, value);
    }

    private Flags GetKeyFromString(string key)
    {
      string pattern = @"[-]{1}[sSdD]{1}";
      Regex reg = new Regex(pattern);
      if (reg.IsMatch(key))
        switch (key)
        {
          case "-d":
          case "-D": return Flags.DestinationPath;
          case "-s":
          case "-S": return Flags.SourcePath;
          default: return Flags.None;
        }
      return Flags.None;
    }
    #endregion
  }
}