using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WolfpackEditor.Editor.Tools;
using WolfpackEditor.Editor.Tools.Enums;
using WolfpackEditor.Editor.Tools.Interfaces;
using Random = UnityEngine.Random;

namespace WolfpackEditor.Editor.Hierarchy
{
  public class HierarchyTreeModule : AWolfpackModule
  {
    private Texture _plus;
    private Texture _minus;

    private TreeColoringOption _treeColoringOption;

    // Option values
    private int _colorSeed;
    private List<Color> _arrayColors;
    private Color _singleColor;
    private Color _altColor1;
    private Color _altColor2;

    public override void Enable()
    {
      base.Enable();
      Load();
      EditorApplication.hierarchyWindowItemOnGUI += AddTree;
    }

    public override void Disable()
    {
      base.Disable();
      Save();
      EditorApplication.hierarchyWindowItemOnGUI -= AddTree;
    }

    public override void Initialize()
    {
      _plus = Resources.Load("plus") as Texture;
      _minus = Resources.Load("minus") as Texture;

      _arrayColors = new List<Color>();
      _arrayColors.Add(new Color());

      Enable();
    }

    public override void Settings()
    {
      base.Settings();

      GUILayout.BeginVertical(WolfpackStyles.areaStyle);
      WolfpackStyles.Subheader("Color Settings");
      _treeColoringOption = (TreeColoringOption)EditorGUILayout.EnumPopup(_treeColoringOption);
      EditorGUILayout.Space(EditorGUIUtility.singleLineHeight / 2);

      switch (_treeColoringOption)
      {
        case TreeColoringOption.FromSeed:
          EditorGUILayout.BeginHorizontal();
          GUILayout.Label("Seed");
          _colorSeed = EditorGUILayout.IntField(_colorSeed);
          EditorGUILayout.EndHorizontal();
          break;
        case TreeColoringOption.FromArray:
          EditorGUILayout.BeginHorizontal();
          GUILayout.Label("Colors");
          EditorGUILayout.BeginVertical();


          EditorGUILayout.BeginHorizontal();
          if (GUILayout.Button("Add a color"))
          {
            _arrayColors.Add(Color.black);
          }

          if (GUILayout.Button("Remove last color"))
          {
            if (_arrayColors.Count > 1)
            {
              _arrayColors.RemoveAt(_arrayColors.Count - 1);
            }
          }

          EditorGUILayout.EndHorizontal();


          for (int i = 0; i < _arrayColors.Count(); i++)
          {
            _arrayColors[i] = EditorGUILayout.ColorField(new GUIContent(), _arrayColors[i], true, false, false);
          }

          EditorGUILayout.EndVertical();
          EditorGUILayout.EndHorizontal();
          break;
        case TreeColoringOption.AllOneColor:
          EditorGUILayout.BeginHorizontal();
          GUILayout.Label("Color");
          _singleColor = EditorGUILayout.ColorField(new GUIContent(), _singleColor, true, false, false);
          EditorGUILayout.EndHorizontal();
          break;
        case TreeColoringOption.AlternatingColors:
          EditorGUILayout.BeginHorizontal();
          GUILayout.Label("Color 1");
          _altColor1 = EditorGUILayout.ColorField(new GUIContent(), _altColor1, true, false, false);
          EditorGUILayout.EndHorizontal();

          EditorGUILayout.BeginHorizontal();
          GUILayout.Label("Color 2");
          _altColor2 = EditorGUILayout.ColorField(new GUIContent(), _altColor2, true, false, false);
          EditorGUILayout.EndHorizontal();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      GUILayout.EndVertical();
    }

    public override string FriendlyName() => "Tree Module";

    private void AddTree(int instanceId, Rect rect)
    {
      if ((int)rect.xMin == 46) return;

      int lines = (int)(rect.xMin - 46) / 14;

      rect.xMin -= 9;
      rect.yMin -= EditorGUIUtility.singleLineHeight / 2;
      rect.height = EditorGUIUtility.singleLineHeight;
      for (int i = 0; i < lines; i++)
      {
        Color color = GetColor((int)rect.xMin / 14);
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
            Color squareColor = GetColor((int)rect.xMin / 14 + 1);


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
      if (icon == null) return;
      GUI.color = backgroundColor.r * 0.299f + backgroundColor.g * 0.587f + backgroundColor.b * 0.114f > .2f
        ? Color.black
        : Color.white;
      GUI.DrawTexture(rect, icon);
      GUI.color = Color.white;
    }

    public override void Save()
    {
      EditorPrefs.SetInt(SavesPrefix() + "_treeColoringOption", (int)_treeColoringOption);
      
      // Seed
      EditorPrefs.SetInt(SavesPrefix() + "_colorSeed", _colorSeed);

      // Array
      EditorPrefs.SetInt(SavesPrefix() + "_arrayColors.Count", _arrayColors.Count);
      for (int index = 0; index < _arrayColors.Count; index++)
      {
        Color color = _arrayColors[index];
        WolfpackUtils.SetColor(SavesPrefix() + "_arrayColors_" + index, color);
      }

      // Single
      WolfpackUtils.SetColor(SavesPrefix() + "_singleColor", _singleColor);

      // Alt
      WolfpackUtils.SetColor(SavesPrefix() + "_altColor1", _altColor1);
      WolfpackUtils.SetColor(SavesPrefix() + "_altColor2", _altColor2);
    }

    public override void Load()
    {
      _treeColoringOption = (TreeColoringOption)EditorPrefs.GetInt(SavesPrefix() + "_treeColoringOption", 0);

      _colorSeed = EditorPrefs.GetInt(SavesPrefix() + "_colorSeed", Random.Range(0, int.MaxValue));

      _arrayColors = new List<Color>();
      int arrayColorsCount = EditorPrefs.GetInt(SavesPrefix() + "_arrayColors.Count", 0);

      if (arrayColorsCount == 0)
      {
        _arrayColors.Add(Color.black);
      }
      else
      {
        for (int i = 0; i < arrayColorsCount; i++)
        {
          _arrayColors.Add(WolfpackUtils.GetColor(SavesPrefix() + "_arrayColors_" + i));
        }
      }
      
      _singleColor = WolfpackUtils.GetColor(SavesPrefix() + "_singleColor");

      _altColor1 = WolfpackUtils.GetColor(SavesPrefix() + "_altColor1");
      _altColor2 = WolfpackUtils.GetColor(SavesPrefix() + "_altColor2");
    }

    protected override string SavesPrefix() => "Wolfpack_Hierarchy_TreeModule_";

    private Color GetColor(int number)
    {
      number -= 3; // 3 is the lowest valid received value; remap to 0..n
      switch (_treeColoringOption)
      {
        case TreeColoringOption.FromSeed:
          Random.InitState(number + _colorSeed);
          return new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f,
            Random.Range(0, 255) / 255f);
        case TreeColoringOption.FromArray:

          int index = number % (_arrayColors.Count) - 1;
          
          return _arrayColors[index < 0 ? _arrayColors.Count - 1 : index];
        case TreeColoringOption.AllOneColor:
          return _singleColor;
        case TreeColoringOption.AlternatingColors:
          return number % 2 == 1 ? _altColor1 : _altColor2;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}