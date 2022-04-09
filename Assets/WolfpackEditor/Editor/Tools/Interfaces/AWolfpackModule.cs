using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WolfpackEditor.Editor.Tools.Interfaces
{
  public abstract class AWolfpackModule
  {
    private bool _isEnabled;

    /// <summary>
    /// Call used when the module is enabled.
    /// </summary>
    public virtual void Enable()
    {
      _isEnabled = true;
    }

    /// <summary>
    /// Call used when the module is disabled.
    /// </summary>
    public virtual void Disable()
    {
      _isEnabled = false;
    }
    
    private bool IsEnabled
    {
      get => _isEnabled;
      set
      {
        if (value) Disable();
        else Enable();
      }
    }
    
    /// <summary>
    /// Call used to initialize the module during the editor start
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Settings for this module; used in OnGUI
    /// </summary>
    public virtual void Settings()
    {
      GUILayout.BeginVertical(WolfpackStyles.areaStyle);
        IsEnabled = EditorGUILayout.ToggleLeft("Enabled", IsEnabled);
      GUILayout.EndVertical();
    }

    /// <summary>
    /// Friendly name of the module
    /// </summary>
    /// <returns>Name</returns>
    public abstract string FriendlyName();
  }
}