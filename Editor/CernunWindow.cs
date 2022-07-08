using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CernunWindow : EditorWindow
{
    public static float posY;

    public static void ChangeLineColor(Color color)
    {
        Handles.color = color;
    }
    public static void DrawHorizontalSeparator(float larg)
    {
        Handles.DrawLine(new Vector2(10, posY), new Vector2(larg, posY));
        posY += 5;
    }
    public static void ButtonDrawer(float posX, float larg, string name, Action function)
    {
        if (GUI.Button(new Rect(posX, posY, larg, 20), name))
        {
            function();
        }
        posY += 25;
    }
    public static void DrawTextBox(Rect position, string name, ref string sceneName)
    {
        sceneName = EditorGUI.TextField(position, name, sceneName);
        posY += 25;
    }
    public static void DrawColumnSelector<T, U>(ref Dictionary<PathInfo, T> leftColumn, ref Dictionary<PathInfo, U> rightColumn)
    {
        int controllerOffset = 0;
        int otherOffset = 0;

        PathInfo[] pathLeftCol = new PathInfo[leftColumn.Count];
        PathInfo[] pathRightCol = new PathInfo[rightColumn.Count];

        leftColumn.Keys.CopyTo(pathLeftCol, 0);
        rightColumn.Keys.CopyTo(pathRightCol, 0);

        foreach (PathInfo item in pathLeftCol)
        {
            leftColumn[item] = (T)(object)EditorGUI.ToggleLeft(new Rect(10, posY + controllerOffset, 280, 20), item.PathName, Convert.ToBoolean(leftColumn[item]));
            controllerOffset += 25;
        }

        foreach (PathInfo item in pathRightCol)
        {
            rightColumn[item] = (U)(object)EditorGUI.IntField(new Rect(310, posY + otherOffset, 280, 20), item.PathName, Convert.ToInt32(rightColumn[item]));
            otherOffset += 25;
        }

        Handles.DrawLine(new Vector2(300, posY), new Vector2(300, posY + (controllerOffset > otherOffset ? controllerOffset : otherOffset) - 5));

        posY += controllerOffset > otherOffset ? controllerOffset : otherOffset;
    }
}
