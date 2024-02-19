using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RoadTile))]             //�ε�Ÿ�Ͽ� Ŀ���ҿ�����ǥ��
public class RoadTileEditor : Editor
{
    RoadTile roadTile;

    private void OnEnable()
    {
        roadTile = target as RoadTile;        //�ν�����â�� �� ������ �ε�Ÿ�� Ÿ���ΰ�
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (roadTile != null) 
        {
            if (roadTile.sprites != null) //Sprites����
            {
                EditorGUILayout.LabelField("Sprites Image Preview");
                Texture2D texture;
                for (int i = 0; i < roadTile.sprites.Length; i++) 
                {
                    texture = AssetPreview.GetAssetPreview(roadTile.sprites[i]);
                    if (texture != null) 
                    {
                        GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                    }
                }
            }
        }
    }
}
#endif