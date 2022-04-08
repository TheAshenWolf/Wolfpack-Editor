using System.Linq;
using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools.Interfaces;

namespace WolfpackEditor.Editor.Hierarchy
{
  public class HierarchyToggleModule : IWolfpackModule
  {
    public void Enable()
    {
      EditorApplication.hierarchyWindowItemOnGUI += AddToggle;
    }

    public void Disable()
    {
      EditorApplication.hierarchyWindowItemOnGUI -= AddToggle;
    }

    public void Initialize()
    {
      Enable();
    }

    public void Settings()
    {
      throw new System.NotImplementedException();
    }

    public string FriendlyName() => "Toggle Module";

    private static void AddToggle(int instanceID, Rect rect)
    {
      GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

      if (gameObject == null)
      {
        return;
      }

      Rect toggleRect = new Rect(
        rect.xMax - 4,
        rect.yMin,
        15,
        rect.height);
      bool tmp = GUI.Toggle(toggleRect, gameObject.activeSelf, "");

      if (tmp != gameObject.activeSelf)
      {
        // If you're toggling an object which is part of a selection, toggle the selection
        // This check might be slow
        if (Selection.gameObjects.Contains(gameObject))
        {
          ToggleFocusedObjects(tmp);
        }
        // Otherwise toggle just the object
        else
        {
          gameObject.SetActive(tmp);
        }
      }
    }

    private static void ToggleFocusedObjects(bool value)
    {
      foreach (GameObject gameObject in Selection.gameObjects)
      {
        gameObject.SetActive(value);
      }
    }
  }
}