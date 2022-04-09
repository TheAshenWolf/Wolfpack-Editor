using UnityEditor;
using UnityEngine;

namespace WolfpackEditor.Editor.Tools
{
  public static class WolfpackUtils
  {
    /// <summary>
    /// Saves a color into editor prefs
    /// </summary>
    /// <param name="name">key</param>
    /// <param name="color">value</param>
    public static void SetColor(string name, Color color)
    {
      EditorPrefs.SetFloat(name + "_r", color.r);
      EditorPrefs.SetFloat(name + "_g", color.g);
      EditorPrefs.SetFloat(name + "_b", color.b);
    }

    /// <summary>
    /// Loads a color from editor prefs, returns black if not found
    /// </summary>
    /// <param name="name">key</param>
    /// <returns>color</returns>
    public static Color GetColor(string name)
    {
      Color color = new Color();
      
      if (EditorPrefs.HasKey(name + "_r"))
      {
        color.r = EditorPrefs.GetFloat(name + "_r");
        color.g = EditorPrefs.GetFloat(name + "_g");
        color.b = EditorPrefs.GetFloat(name + "_b");
        color.a = 1;
      }

      return color;
    }
  }
}