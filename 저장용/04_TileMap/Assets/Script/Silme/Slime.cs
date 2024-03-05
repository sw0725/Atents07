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
    public float lifeTimeBonus = 2.0f;
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
    float pathWaitTime = 0.0f;          //다른슬라임에 의해 경로가 막힐때 기다리는 시간
                                        //길찾기 알고리즘은 무거우므로 자주 호출하면 안된다.
    Material material;
    PathLine pathLine;
    TileGridMap map;
    Action onDissolveEnd;
    Action onPhaseEnd;
    Transform pool;
    SpriteRenderer spriteRenderer;      //order in layer 수정용

    List<Vector2Int> path;
    Node currnt = null;
    Node Currnt 
    {
        get => currnt; 
        set 
        {
            if(currnt != value) 
            {
                if(currnt != null)                      //이전노드가 널이면 스킵
                {
                    currnt.type = Node.NodeType.Plain;
                }
                currnt = value;
                if (currnt != null)
                {
                    currnt.type = Node.NodeType.Slime;
                }
            }
        }
    }

    const float VisialeOutLine = 0.004f;
    const float VisiablePhase = 0.1f;
    const float MaxPathWaitTime = 1.0f;

    readonly int DissolveFadeId = Shader.PropertyToID("_DissolveFade");
    readonly int SplitId = Shader.PropertyToID("_Split");
    readonly int OutLineId = Shader.PropertyToID("_OutLineThickness");
    readonly int PhaseId = Shader.PropertyToID("_PhaseThickness");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        onDissolveEnd += ReturnToPool;
        onPhaseEnd += () => isMoveActivate = true;

        path = new List<Vector2Int>();
        pathLine = GetComponentInChildren<PathLine>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        ResetShader();                      //재사용시 중간에 멈춘채로 이어시작하는것 방지용
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

    public void Initialized(TileGridMap gridMap, Vector3 world)     //슬라임 초기화용 스폰직후 실행
    {                                               //슬라임 시작위치
        map = gridMap;
        transform.position = map.GridToWorld(map.WorldToGrid(world));        //자동적으로 셀의 가운데 계산을 위한
        Currnt = map.GetNode(world);
    }          

    public void Die() 
    {
        isMoveActivate = false;
        OnDie?.Invoke();
        OnDie = null;
        StartCoroutine(StartDissolve());
    }

    public void ReturnToPool() 
    {
        Currnt = null;
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
            material.SetFloat(SplitId, 1 - (time * nomalrise));     // == time/phaseDuration **플롯나눗셈은 적으면 적을수록 좋다.

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
        {           //길을 못찾아 경로가 없을때
            if (path != null && path.Count > 0 && pathWaitTime < MaxPathWaitTime) //패스에 남은 경로가 있다면, d오래 기다리지 않았다면
            {
                Vector2Int destGrid = path[0];                      //내가있는(가는중인)
                if (!map.IsSlime(destGrid) || map.GetNode(destGrid) == Currnt)
                {
                    Vector3 destPosition = map.GridToWorld(destGrid);           //벡터3로 목적지 월드 좌표를 받기
                    Vector3 direction = (destPosition - transform.position);    //를 방향백터로 전환

                    if (direction.sqrMagnitude < 0.001f)                        //백터의 길이를 확인 일정 이하 시
                    {
                        transform.position = destPosition;                      //오차보정 및
                        path.RemoveAt(0);                                       //다음 패스로
                    }
                    else
                    {
                        transform.Translate(Time.deltaTime * moveSpeed * direction.normalized);
                        Currnt = map.GetNode(transform.position);
                    }               //==order in layer
                    spriteRenderer.sortingOrder = -Mathf.FloorToInt(transform.position.y * 100);
                    pathWaitTime = 0.0f;
                }                                       //아래슬라임이 위에 그려짐
                else //다른 슬라임있다
                {
                    pathWaitTime += Time.deltaTime;
                }
            }
            else
            {
                pathWaitTime = 0.0f;
                OnDestinationArrive();
            }
        }
    }

    public void ShowPath(bool isShow = true) 
    {
        pathLine.gameObject.SetActive(isShow);
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
