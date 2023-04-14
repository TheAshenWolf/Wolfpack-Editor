using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.SEIDInfo;
using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Hierarchy;
using WolfpackEditor.Editor.Tools.Enums;
using WolfpackEditor.Editor.Tools.Interfaces;
using WolfpackEditor.Editor.WolfpackWindows;

namespace WolfpackEditor.Editor
{
  [InitializeOnLoad]
  public class WolfpackModuleManager
  {
    public static readonly Dictionary<WolfpackModules, AWolfpackModule[]> modules = new Dictionary<WolfpackModules, AWolfpackModule[]>()
    {
      {
        // The order here determines
        // - The order in the menu
        // - The order in which the modules are drawn (-> background alterations should be first)
        WolfpackModules.Hierarchy, new AWolfpackModule[]
        {
          new HierarchyStripesModule(),
          new HierarchyHighlightModule(),
          new HierarchyToggleModule(),
          new HierarchyTreeModule(),
        }
      }
    };

    static WolfpackModuleManager()
    {
      foreach (KeyValuePair<WolfpackModules, AWolfpackModule[]> entry in modules)
      {
        AWolfpackModule[] submodules = entry.Value;
        foreach (AWolfpackModule module in submodules)
        {
          module.Initialize();
        }
      }
      
    }
    
    [MenuItem("Wolfpack/Settings")]
    static void OpenWolfpackSettingsWindow()
    {
      EditorWindow.GetWindow<WolfpackSettings>().Show();
    }
  }
}