using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class DamegeText : RecycleObject
{
    public AnimationCurve movement;
    public AnimationCurve fade;
    public float maxHight = 1.5f;
    public float duration = 1.0f;

    float elapsedTime = 0.0f;
    float baceHight;

    TextMeshPro damegeText;

    private void Awake()
    {
        damegeText = GetComponent<TextMeshPro>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        damegeText.color = Color.white;
        elapsedTime = 0.0f;
        baceHight = transform.position.y;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float timeRatio = elapsedTime / duration;

        float curveMove = movement.Evaluate(timeRatio);
        float currentHight = baceHight + maxHight * curveMove;
        transform.position =new Vector3(transform.position.x, currentHight, transform.position.z);

        float curveAlpha = fade.Evaluate(timeRatio);
        damegeText.color = new Color(1, 1, 1, curveAlpha);

        transform.localScale = new Vector3(curveAlpha, curveAlpha, curveAlpha);
        if (elapsedTime > duration)
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetDamege(int damege) 
    {
        damegeText.text = damege.ToString();
    }
}
