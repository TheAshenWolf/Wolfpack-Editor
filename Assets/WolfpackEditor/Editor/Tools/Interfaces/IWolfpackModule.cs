namespace WolfpackEditor.Editor.Tools.Interfaces
{
  public interface IWolfpackModule
  {
    /// <summary>
    /// Call used when the module is enabled.
    /// </summary>
    public void Enable();
    
    /// <summary>
    /// Call used when the module is disabled.
    /// </summary>
    public void Disable();
    
    /// <summary>
    /// Call used to initialize the module during the editor start
    /// </summary>
    public void Initialize();

    /// <summary>
    /// Settings for this module; used in OnGUI
    /// </summary>
    public void Settings();

    /// <summary>
    /// Friendly name of the module
    /// </summary>
    /// <returns>Name</returns>
    public string FriendlyName();
  }
}