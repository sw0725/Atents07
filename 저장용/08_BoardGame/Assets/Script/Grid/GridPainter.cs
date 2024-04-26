using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridPainter : MonoBehaviour
{
    public GameObject Line;
    public GameObject Letter;

    const int gridLineCount = 11;

    private void Awake()
    {
        DrawGridLines();
        DrawGridLetters();
    }

    void DrawGridLines() 
    {
        for(int i =0; i < gridLineCount; i++) 
        {
            GameObject line = Instantiate(Line, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(i, 0, 1));
            lineRenderer.SetPosition(1, new Vector3(i, 0, 1 - gridLineCount));
        }
        for (int i = 0; i < gridLineCount; i++)
        {
            GameObject line = Instantiate(Line, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(-1, 0, -i));
            lineRenderer.SetPosition(1, new Vector3(gridLineCount - 1, 0, - i));
        }
    }

    void DrawGridLetters() 
    {
        for (int i = 1; i < gridLineCount; i++)
        {
            GameObject letter = Instantiate(Letter, transform);
            letter.transform.position = new Vector3(i - 0.5f, 0, 0.5f);
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            char alphabet = (char)('A' + i -1);
            text.text = alphabet.ToString();
        }

        for (int i = 1; i < gridLineCount; i++)
        {
            GameObject letter = Instantiate(Letter, transform);
            letter.transform.position = new Vector3(-0.5f, 0, 0.5f - i);
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            text.text = i.ToString();
            if (i > 9) 
            {
                text.fontSize = 8;
            }
        }
    }
}
