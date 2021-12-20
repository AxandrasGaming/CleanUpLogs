﻿using CleanUpLogs.Console.BusinessLogic;
using CleanUpLogs.Console.Helper;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CleanUpLogs.Console
{
  public class CleanUpLogsConsole
  {
    #region Fields
    private string[] _args = null;
    private Dictionary<Flags, string> _flags = new Dictionary<Flags, string>();
    private CleanUpLogsLogic _cleanUpLogsLogic = new CleanUpLogsLogic();
    private IExtensionManager _fileExtensionManager;
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
      ExtensionManagerFactory emf = new ExtensionManagerFactory();
      _fileExtensionManager = emf.Create();
      StartLogCleaning();
    }

    public CleanUpLogsConsole() { }
    #endregion

    #region Methods
    private void StartLogCleaning()
    {
      if (_args == null) return;
      InitializeFlagCollection();
      if (FlagCollectionHasValidFlags()) return;

      string path = FlagCollection[Flags.SourcePath];
      if (!_fileExtensionManager.IsValidPath(path)) return;

      _cleanUpLogsLogic.ReadContentOfFile(path);
    }

    private bool FlagCollectionHasValidFlags()
    {
      return FlagCollection.Keys.Count <= 0
        || FlagCollection.ContainsKey(Flags.None)
        || FlagCollection.ContainsKey(Flags.Default);
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
      FlagCollection.TryAdd(key, value);
    }

    private Flags GetKeyFromString(string key)
    {
      string pattern = @"[-]{1}[fF]{1}";
      Regex reg = new Regex(pattern);
      if (reg.IsMatch(key))
        switch (key)
        {
          case "-f":
          case "-F": return Flags.SourcePath;
          default: return Flags.Default;
        }
      return Flags.None;
    }
    #endregion
  }
}