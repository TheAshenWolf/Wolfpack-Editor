using UnityEditor;
using UnityEngine;

namespace WolfpackEditor.Editor.Tools
{
  public static class WolfpackStyles
  {
    private static readonly GUIStyle _headerStyle = new GUIStyle()
    {
      richText = true,
      fontSize = 26,
      fontStyle = FontStyle.Bold,
      padding = { top = 10, bottom = 0, left = 10, right = 10 },
      normal = {textColor = Color.white}
    };
    
    private static readonly GUIStyle _subheaderStyle = new GUIStyle()
    {
      richText = true,
      fontSize = 14,
      fontStyle = FontStyle.Bold,
      padding = { top = 10, bottom = 0, left = 10, right = 10 },
      normal = {textColor = Color.white}
    };

    public static readonly GUIStyle areaStyle = new GUIStyle(GUI.skin.box)
    {
      padding = { top = 10, bottom = 10, left = 10, right = 10 }
    };

    public static void Header(string text)
    {
      GUILayout.Label(text, _headerStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    
    public static void Subheader(string text)
    {
      GUILayout.Label(text, _subheaderStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
  }
}