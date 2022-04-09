using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools;
using WolfpackEditor.Editor.Tools.Enums;
using WolfpackEditor.Editor.Tools.Interfaces;

namespace WolfpackEditor.Editor.WolfpackWindows
{
  public class WolfpackSettings : EditorWindow
  {
    private List<GUIContent> _toolbarIconsList = new List<GUIContent>();
    private GUIContent[] _toolbarIcons;
    
    private int _toolbarTab;
    
    // Textures
    private Texture _wolfpackLogo;
    private Texture _hierarchyIcon;

    // Rects
    private Rect _toolbarRect;
    private Rect _contentRect;
    private Rect _visibleContentRect;
    
    // Current values
    private Vector2 _scrollPosition;
    private Dictionary<Type, bool> _foldouts = new Dictionary<Type, bool>();

    void OnEnable()
    {
      LoadIcons();

      titleContent.image = _wolfpackLogo;
      titleContent.text = "Wolfpack Settings";
      maxSize = new Vector2(600, 900);
      minSize = new Vector2(600, 900);

      
      _contentRect = new Rect(50, 0, 550, 900);
      _visibleContentRect = new Rect(_contentRect);
      _visibleContentRect.xMax -= 16;
      _toolbarRect = new Rect(0, 0, 50, 900);
      
    }

    private void OnGUI()
    {
      EditorGUILayout.BeginHorizontal();
      _toolbarTab = GUILayout.SelectionGrid(_toolbarTab, _toolbarIcons, 1, new GUILayoutOption[]
      {
        GUILayout.Width(50),
        GUILayout.Height(900)
      });
      
      _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);

      switch (_toolbarTab)
      {
        case 0: // Hierarchy
          WolfpackStyles.Header("Hierarchy");
          DrawHierarchySettings();
          break;
      }
      
      GUILayout.EndScrollView();
      EditorGUILayout.EndHorizontal();
    }
    private void DrawHierarchySettings()
    {
      foreach (AWolfpackModule module in WolfpackModuleManager.modules[WolfpackModules.Hierarchy])
      {
        Type type = module.GetType();

        if (!_foldouts.ContainsKey(type))
        {
          _foldouts.Add(type, false);
        }

        _foldouts[type] = EditorGUILayout.BeginFoldoutHeaderGroup(_foldouts[type], module.FriendlyName());
        if (_foldouts[type])
        {
          module.Settings();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

      }
    }
    
    private void LoadIcons()
    {
      _toolbarIconsList = new List<GUIContent>();
      _wolfpackLogo = Resources.Load<Texture>("wolfpack-logo");
      _hierarchyIcon = Resources.Load<Texture>("hierarchy");
      
      _toolbarIconsList.Add(new GUIContent(_hierarchyIcon, "Hierarchy"));
      _toolbarIconsList.Add(new GUIContent(_wolfpackLogo, "Project"));
      _toolbarIconsList.Add(new GUIContent(_wolfpackLogo, "Console"));
      _toolbarIconsList.Add(new GUIContent(_wolfpackLogo, "Inspector"));
      _toolbarIconsList.Add(new GUIContent(_wolfpackLogo, "Scene"));

      _toolbarIcons = _toolbarIconsList.ToArray();
    }
  }
}