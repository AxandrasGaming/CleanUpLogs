namespace CleanUpLogs.Console.Helper
{
 public class ExtensionManagerFactory
  {
    private IExtensionManager _extensionManager = null;
    public IExtensionManager Create()
    {
      if (_extensionManager != null) return _extensionManager;
      return new FileExtensionManager();
    }

    public void SetManager(IExtensionManager extensionManager)
    {
      _extensionManager = extensionManager;
    }
  }
}
