using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RoadTile))]             //로드타일용 커스텀에디터표시
public class RoadTileEditor : Editor
{
    RoadTile roadTile;

    private void OnEnable()
    {
        roadTile = target as RoadTile;        //인스펙터창에 뜬 에셋이 로드타일 타입인가
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (roadTile != null) 
        {
            if (roadTile.sprites != null) //Sprites존재
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