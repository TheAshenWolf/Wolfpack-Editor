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
    public static readonly Dictionary<WolfpackModules, IWolfpackModule[]> modules = new Dictionary<WolfpackModules, IWolfpackModule[]>()
    {
      {
        WolfpackModules.Hierarchy, new IWolfpackModule[]
        {
          new HierarchyStripesModule(),
          new HierarchyToggleModule(),
          new HierarchyTreeModule()
        }
      }
    };

    static WolfpackModuleManager()
    {
      foreach (KeyValuePair<WolfpackModules, IWolfpackModule[]> entry in modules)
      {
        IWolfpackModule[] submodules = entry.Value;
        foreach (IWolfpackModule module in submodules)
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