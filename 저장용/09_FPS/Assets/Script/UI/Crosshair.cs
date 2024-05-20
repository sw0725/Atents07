using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public AnimationCurve curve;
    public float maxExpend = 100.0f;

    const float defaultEapend = 10.0f;
    const float recorveryWaitTime = 0.1f;
    const float recorveryDuration = 0.5f;
    const float divPreCompute = 1/recorveryDuration;

    float current = 0.0f;

    RectTransform[] crossRects;

    readonly Vector2[] direction = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};

    private void Awake()
    {
        crossRects = new RectTransform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) 
        {
            crossRects[i] = transform.GetChild(i) as RectTransform;
        }
    }

    public void Expend(float amount) 
    {
        current = Mathf.Min(current+amount, maxExpend);
        for(int i = 0;i < crossRects.Length;i++) 
        {
            crossRects[i].anchoredPosition = (current + defaultEapend) * direction[i];
        }

        StopAllCoroutines();
        StartCoroutine(DelayRecovery(recorveryWaitTime));
    }

    IEnumerator DelayRecovery(float wait) 
    {
        yield return new WaitForSeconds(wait);

        float startExpend = current;
        float curveProcess = 0.0f;

        while (curveProcess < 1) 
        {
            curveProcess += Time.deltaTime * divPreCompute;
            current = curve.Evaluate(curveProcess) * startExpend;

            for (int i = 0; i < crossRects.Length; i++)
            {
                crossRects[i].anchoredPosition = (current + defaultEapend) * direction[i];
            }
            yield return null;
        }
        for (int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i].anchoredPosition = defaultEapend * direction[i];
        }

        current = 0;
    }
}
