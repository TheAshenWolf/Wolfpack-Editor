using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools.Interfaces;

namespace WolfpackEditor.Editor.Hierarchy
{
  public class HierarchyStripesModule : IWolfpackModule
  {
    public void Enable()
    {
      EditorApplication.hierarchyWindowItemOnGUI += AddStripes;
    }

    public void Disable()
    {
      EditorApplication.hierarchyWindowItemOnGUI += AddStripes;
    }

    public void Initialize()
    {
      Enable();
    }

    public void Settings()
    {
      throw new System.NotImplementedException();
    }

    public string FriendlyName() => "Stripes Module";

    private static void AddStripes(int instanceID, Rect rect)
    {
      Rect fullStripe = new Rect(rect);
      fullStripe.xMin = 32;
      fullStripe.xMax += 32;
    
      if (rect.yMin != 0 && (int)(rect.yMin / 16) % 2 == 0)
      { 
        EditorGUI.DrawRect(fullStripe, new Color(1, 1, 1, 0.03f));
      }
    }
  }
}