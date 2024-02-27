using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : RecycleObject
{
    public float phaseDuration = 0.5f;
    public float dissolveDuration = 1.0f;
    public float moveSpeed = 2.0f;
    public Action OnDie;
    public Transform Pool 
    {
        set 
        {
            if (pool == null) 
            {
                pool = value;
            }
        }
    }

    Vector2Int GridPosition => map.WorldToGrid(transform.position);
    bool isMoveActivate = false;

    Material material;
    PathLine pathLine;
    TileGridMap map;
    Action onDissolveEnd;
    Action onPhaseEnd;
    Transform pool;

    List<Vector2Int> path;

    const float VisialeOutLine = 0.004f;
    const float VisiablePhase = 0.1f;

    readonly int DissolveFadeId = Shader.PropertyToID("_DissolveFade");
    readonly int SplitId = Shader.PropertyToID("_Split");
    readonly int OutLineId = Shader.PropertyToID("_OutLineThickness");
    readonly int PhaseId = Shader.PropertyToID("_PhaseThickness");

    private void Awake()
    {
        Renderer slime = GetComponent<Renderer>();
        material = slime.material;

        onDissolveEnd += ReturnToPool;
        onPhaseEnd += () => isMoveActivate = true;

        path = new List<Vector2Int>();
        pathLine = GetComponentInChildren<PathLine>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        ResetShader();                      //����� �߰��� ����ä�� �̾�����ϴ°� ������
        StartCoroutine(StartPhase());
        isMoveActivate = false;
    }

    protected override void OnDisable()
    {
        path.Clear();
        pathLine.ClearPath();
        base.OnDisable();
    }

    private void Update()
    {
        MoveUpdate();
    }

    public void Initialized(TileGridMap gridMap, Vector3 world)     //������ �ʱ�ȭ�� �������� ����
    {                                               //������ ������ġ
        map = gridMap;
        transform.position = map.GridToWorld(map.WorldToGrid(world));
    }           //�ڵ������� ���� ��� ����� ����

    public void Die() 
    {
        isMoveActivate = false;
        OnDie?.Invoke();
        OnDie = null;
        StartCoroutine(StartDissolve());
    }

    void ReturnToPool() 
    {
        transform.SetParent(pool);
        gameObject.SetActive(false);
    }

    void ResetShader()
    {
        ShowOutLine(false);
        material.SetFloat(PhaseId, 0);
        material.SetFloat(SplitId, 1);
        material.SetFloat(DissolveFadeId, 1);
    }
    public void ShowOutLine(bool isShow = true)
    {
        if (isShow)
        {
            material.SetFloat(OutLineId, VisialeOutLine);
        }
        else
        {
            material.SetFloat(OutLineId, 0);
        }
    }
    IEnumerator StartPhase()
    {
        float nomalrise = 1.0f / phaseDuration;

        float time = 0.0f;

        material.SetFloat(PhaseId, VisiablePhase);
        while (time < phaseDuration)
        {
            time += Time.deltaTime;
            material.SetFloat(SplitId, 1 - (time * nomalrise));     // == time/phaseDuration **�÷Գ������� ������ �������� ����.

            yield return null;
        }
        material.SetFloat(PhaseId, 0);
        material.SetFloat(SplitId, 0);

        onPhaseEnd?.Invoke();
    }

    IEnumerator StartDissolve()
    {
        float nomalrise = 1.0f / dissolveDuration;

        float time = 0.0f;

        while (time < dissolveDuration)
        {
            time += Time.deltaTime;
            material.SetFloat(DissolveFadeId, 1 - (time * nomalrise));

            yield return null;
        }
        material.SetFloat(DissolveFadeId, 0);

        onDissolveEnd?.Invoke();
    }

    public void SetDestination(Vector2Int destination) 
    {
        path = AStar.PathFind(map, GridPosition, destination);
        pathLine.DrawPath(map, path);
    }

    void OnDestinationArrive() 
    {
        SetDestination(map.GetRandomMoveableposition());
    }

    void MoveUpdate() 
    {
        if (isMoveActivate) 
        {
            if (path.Count > 0) //�н��� ���� ��ΰ� �ִٸ�
            {
                Vector2Int destGrid = path[0];
                Vector3 destPosition = map.GridToWorld(destGrid);           //����3�� ������ ���� ��ǥ�� �ޱ�
                Vector3 direction = (destPosition - transform.position);    //�� ������ͷ� ��ȯ

                if (direction.sqrMagnitude < 0.001f)                        //������ ���̸� Ȯ�� ���� ���� ��
                {
                    transform.position = destPosition;                      //�������� ��
                    path.RemoveAt(0);                                       //���� �н���
                }
                else
                {
                    transform.Translate(Time.deltaTime * moveSpeed * direction.normalized);
                }
            }
            else
            {
                OnDestinationArrive();
            }
        }
    }

#if UNITY_EDITOR
    public void TestShader(int index) 
    {
        switch (index) 
        {
            case 1:
                ResetShader();
                break;
            case 2:
                ShowOutLine(true);
                break;
            case 3:
                ShowOutLine(false);
                break;
            case 4:
                StartCoroutine(StartPhase());
                break;
            case 5:
                StartCoroutine(StartDissolve());
                break;
        }
    }

    public void TestDie() 
    {
        Die();
    }
#endif
}
