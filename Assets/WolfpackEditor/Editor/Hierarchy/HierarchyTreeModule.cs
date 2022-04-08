using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools.Interfaces;

namespace WolfpackEditor.Editor.Hierarchy
{
  public class HierarchyTreeModule : IWolfpackModule
  {
    private Texture _plus;
    private Texture _minus;
    public void Enable()
    {
      EditorApplication.hierarchyWindowItemOnGUI += AddTree;
    }

    public void Disable()
    {
    }

    public void Initialize()
    {
      _plus = Resources.Load("plus") as Texture;
      _minus = Resources.Load("minus") as Texture;
        
      Enable();
    }

    public void Settings()
    {
      throw new System.NotImplementedException();
    }

    public string FriendlyName() => "Tree Module";

    private void AddTree(int instanceId, Rect rect)
    {
      // ├ │ └
      if ((int)rect.xMin == 46) return;

      int lines = (int)(rect.xMin - 46) / 14;

      rect.xMin -= 9;
      rect.yMin -= EditorGUIUtility.singleLineHeight / 2;
      rect.height = EditorGUIUtility.singleLineHeight;
      for (int i = 0; i < lines; i++)
      {
        Random.InitState((int)rect.xMin);
        Color color = new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f);
        
        // Draw lines across
        if (i == 0)
        {
          Rect childRect = new Rect(rect);
          childRect.yMin += EditorGUIUtility.singleLineHeight / 2 + 7;
          childRect.xMin -= 14;
          childRect.width = 16;
          childRect.height = 2;
          
          if (i != lines - 1)
          {
            EditorGUI.DrawRect(childRect, color);
          }
          

          //Draw square as the toggle
          GameObject target = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
          if (target && target.transform.childCount > 0)
          {
            // I am a parent of someone
            Random.InitState((int)rect.xMin + 14);
            Color squareColor = new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f,
              Random.Range(0, 255) / 255f);


            Rect squareRect = new Rect(childRect);
            squareRect.xMin += 10;
            squareRect.yMin -= 4;
            squareRect.width = 10;
            squareRect.height = 10;
            EditorGUI.DrawRect(squareRect, squareColor);
            DrawIconOnBackground(squareRect, _plus, squareColor);
          }
        }
        
        if (i != lines - 1)
        {
          rect.xMin -= 14;
          rect.width = 2;
          
          EditorGUI.DrawRect(rect, color);
        }
        
        Transform self = (EditorUtility.InstanceIDToObject(instanceId) as GameObject)?.transform;
        if (i == 0 && self && self.transform.parent != null && self.transform.parent.GetChild(0) == self.transform)
        {
          // I am the first child of my parent
          Rect squareRect = new Rect(rect);
          squareRect.xMin -= 4;
          squareRect.yMin -= 4;
          squareRect.width = 10;
          squareRect.height = 10;
          EditorGUI.DrawRect(squareRect, color);

          DrawIconOnBackground(squareRect, _minus, color);
        }


      }
    }

    private void DrawIconOnBackground(Rect rect, Texture icon, Color backgroundColor)
    {
      GUI.color = backgroundColor.r * 0.299f + backgroundColor.g * 0.587f + backgroundColor.b * 0.114f > .2f ? Color.black : Color.white;
      GUI.DrawTexture(rect, icon);
      GUI.color = Color.white;
    }
  }
}