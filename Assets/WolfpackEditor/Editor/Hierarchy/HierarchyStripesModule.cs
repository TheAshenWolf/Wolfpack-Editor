using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools.Interfaces;

namespace WolfpackEditor.Editor.Hierarchy
{
  public class HierarchyStripesModule : AWolfpackModule
  {
    public override void Enable()
    {
      base.Enable();
      EditorApplication.hierarchyWindowItemOnGUI += AddStripes;
    }

    public override void Disable()
    {
      base.Disable();
      EditorApplication.hierarchyWindowItemOnGUI += AddStripes;
    }

    public override void Initialize()
    {
      Enable();
    }

    public override void Settings()
    {
      base.Settings();
    }

    public override string FriendlyName() => "Stripes Module";

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