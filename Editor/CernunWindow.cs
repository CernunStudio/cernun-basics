using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CernunWindow : EditorWindow
{
    private static float wWidth;
    private static float wHeight;
    private static float marginTop;
    private static float marginDown;
    private static float marginLeft;
    private static float marginRight;
    private static bool hasScrollBar;

    public static float posY;

    public static float vScroll;

    private static int selectedTab = 0;

    public static void GUIInitialize(float width, float height)
    {
        wWidth = width;
        wHeight = height;

        marginTop = 5f;
        marginDown = 10f;
        marginLeft = 10f;
        if (!hasScrollBar)
        {
            marginRight = 10f;
        }

        posY = 5f;
    }
    public static void GUIInitialize(float width, float height, float margin)
    {
        wWidth = width;
        wHeight = height;

        marginTop = margin;
        marginDown = margin;
        marginLeft = margin;
        if (!hasScrollBar)
        {
            marginRight = margin;
        }

        posY = margin;
    }
    public static void GUIInitialize(float width, float height, float marginTD, float marginLR)
    {
        wWidth = width;
        wHeight = height;

        marginTop = marginTD;
        marginDown = marginTD;
        marginLeft = marginLR;
        if (!hasScrollBar)
        {
            marginRight = marginLR;
        }

        posY = marginTD;
    }
    public static void GUIInitialize(float width, float height, float marginT, float marginD, float marginL, float marginR)
    {
        wWidth = width;
        wHeight = height;

        marginTop = marginT;
        marginDown = marginD;
        marginLeft = marginL;
        if (!hasScrollBar)
        {
            marginRight = marginR;
        }

        posY = marginT;
    }

    public static void ChangeLineColor(Color color)
    {
        Handles.color = color;
    }

    public static void DrawHorizontalSeparator(bool scrolled = true)
    {
        float larg = wWidth - marginRight;
        Handles.DrawLine(new Vector2(marginLeft, scrolled ? posY - vScroll : posY), new Vector2(larg, scrolled ? posY - vScroll : posY));
        posY += 5;
    }

    public static void ButtonDrawer(float larg, string name, Action function, bool scrolled = true)
    {
        float largeur = larg;
        if (larg > wWidth - marginLeft - marginRight)
        {
            largeur = wWidth - marginLeft - marginRight;
        }
        if (GUI.Button(new Rect((wWidth - largeur) / 2, scrolled ? posY - vScroll : posY, largeur, 20), name))
        {
            function();
        }
        posY += 25;
    }

    public static void DrawTextBox(string name, ref string sceneName, bool scrolled = true)
    {
        float larg = wWidth - marginLeft - marginRight;
        sceneName = EditorGUI.TextField(new Rect(marginLeft, scrolled ? posY - vScroll : posY, larg, 20), name, sceneName);
        posY += 25;
    }

    public static void DrawColumnSelector<T, U>(ref Dictionary<PathInfo, T> leftColumn, ref Dictionary<PathInfo, U> rightColumn, bool scrolled = true)
    {
        int controllerOffset = 0;
        int otherOffset = 0;

        PathInfo[] pathLeftCol = new PathInfo[leftColumn.Count];
        PathInfo[] pathRightCol = new PathInfo[rightColumn.Count];

        leftColumn.Keys.CopyTo(pathLeftCol, 0);
        rightColumn.Keys.CopyTo(pathRightCol, 0);

        float midWidth = wWidth / 2;

        foreach (PathInfo item in pathLeftCol)
        {
            leftColumn[item] = (T)(object)EditorGUI.ToggleLeft(new Rect(marginLeft, (scrolled ? posY - vScroll : posY) + controllerOffset, midWidth - marginLeft - marginRight, 20), item.PathName, Convert.ToBoolean(leftColumn[item]));
            controllerOffset += 25;
        }

        foreach (PathInfo item in pathRightCol)
        {
            rightColumn[item] = (U)(object)EditorGUI.IntField(new Rect(midWidth + marginLeft, (scrolled ? posY - vScroll : posY) + otherOffset, midWidth - marginLeft - marginRight, 20), item.PathName, Convert.ToInt32(rightColumn[item]));
            if (Convert.ToInt32(rightColumn[item]) <0)
            {
                rightColumn[item] = (U)(object)0;
            }
            otherOffset += 25;
        }

        Handles.DrawLine(new Vector2(midWidth, posY), new Vector2(midWidth, (scrolled ? posY - vScroll : posY) + (controllerOffset > otherOffset ? controllerOffset : otherOffset) - 5));

        posY += controllerOffset > otherOffset ? controllerOffset : otherOffset;
    }

    public static void DrawTab(Dictionary<string, Dictionary<PathInfo, bool>> tabs)
    {
        float larg = wWidth - marginLeft - marginRight;

        string[] tabName = new string[tabs.Count];
        tabs.Keys.CopyTo(tabName, 0);
        // Tentatives de Tab
        selectedTab = GUI.Toolbar(new Rect(marginLeft, posY - vScroll, larg, 20), selectedTab, tabName);

        posY += 25;
        PathInfo[] paths = new PathInfo[tabs[tabName[selectedTab]].Count];
        tabs[tabName[selectedTab]].Keys.CopyTo(paths, 0);
        foreach (PathInfo item in paths)
        {
            tabs[tabName[selectedTab]][item] = EditorGUI.ToggleLeft(new Rect(10, posY - vScroll, 280, 20), item.PathName, tabs[tabName[selectedTab]][item]);
            posY += 25;
        }

    }

    public static void DrawVerticalScrollBar()
    {
        if (posY > wHeight)
        {
            if (!hasScrollBar)
            {
                marginRight *= 2;
                hasScrollBar = true;
            }

            vScroll = GUI.VerticalScrollbar(new Rect(wWidth - marginRight / 2 - 5, 0, marginRight / 2, wHeight), vScroll, wHeight, 0f, posY);
        }
        else
        {
            if (hasScrollBar)
            {
                marginRight /= 2;
                hasScrollBar = false;
            }
        }
    }
}
