using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools;
using WolfpackEditor.Editor.Tools.Interfaces;

namespace WolfpackEditor.Editor.Hierarchy
{
  public class HierarchyHighlightModule : AWolfpackModule
  {
    private static Color _singleColor;
    private static string _pattern = "> ";
    public override void Enable()
    {
      base.Enable();
      Load();
      EditorApplication.hierarchyWindowItemOnGUI += RenderHighlight;
    }

    public override void Disable()
    {
      base.Disable();
      Save();
      EditorApplication.hierarchyWindowItemOnGUI -= RenderHighlight;
    }

    public override void Initialize()
    {
      Enable();
    }
    
    protected override string SavesPrefix() => "Wolfpack_Hierarchy_RenderHighlight_";
    public override void Save()
    {
      WolfpackUtils.SetColor(SavesPrefix() + "_singleColor", _singleColor);
      PlayerPrefs.SetString(SavesPrefix() + "_pattern", _pattern);
    }

    public override void Load()
    {
      _singleColor = WolfpackUtils.GetColor(SavesPrefix() + "_singleColor");
      _pattern = PlayerPrefs.GetString(SavesPrefix() + "_pattern");
    }

    public override void Settings()
    {
      base.Settings();
      GUILayout.BeginVertical(WolfpackStyles.areaStyle);
      {
        WolfpackStyles.Subheader("Pattern Matching");
        
        EditorGUILayout.HelpBox("In order to match a pattern, the name of the GameObject must start with the pattern.", MessageType.Info);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Pattern");
        _pattern = EditorGUILayout.TextField(_pattern);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Color");
        _singleColor = EditorGUILayout.ColorField(new GUIContent(), _singleColor, true, true, false);
        EditorGUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
    }

    public override string FriendlyName() => "Highlight Module";

    private static void RenderHighlight(int instanceID, Rect rect)
    {
      Rect fullStripe = new Rect(rect);
      fullStripe.xMin = 32;
      fullStripe.xMax += 32;

      if (EditorUtility.InstanceIDToObject(instanceID) is GameObject gameObject && gameObject.name.StartsWith(_pattern))
      { 
        EditorGUI.DrawRect(fullStripe, _singleColor);
      }
    }
  }
}