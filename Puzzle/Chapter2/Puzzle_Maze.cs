using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Maze : Puzzle
{
    [SerializeField] private List<Transform> Node1;     // 노드를 담아두기위한 List
    [SerializeField] private List<Transform> Node2;     // 노드를 담아두기위한 List
    [SerializeField] private List<Transform> Node3;     // 노드를 담아두기위한 List

    [SerializeField] private GameObject Player1;        // 플레이어의 오브젝트를 담아두는 GameObject
    [SerializeField] private GameObject Player2;        // 플레이어의 오브젝트를 담아두는 GameObject
    [SerializeField] private GameObject Player3;        // 플레이어의 오브젝트를 담아두는 GameObject

    [SerializeField] private LayerMask layer;           // 특정 레이어 체크를 위한 Layer

    private Transform Player1_transform;                // 노드의 포지션을 저장해두기 위한 Transform
    private Transform Player2_transform;                // 노드의 포지션을 저장해두기 위한 Transform
    private Transform Player3_transform;                // 노드의 포지션을 저장해두기 위한 Transform

    private Dictionary<Transform, List<Transform>> Node1_Connection = new Dictionary<Transform, List<Transform>>(); // 노드의 연결점을 만들기 위한 Dictionary
    private Dictionary<Transform, List<Transform>> Node2_Connection = new Dictionary<Transform, List<Transform>>(); // 노드의 연결점을 만들기 위한 Dictionary
    private Dictionary<Transform, List<Transform>> Node3_Connection = new Dictionary<Transform, List<Transform>>(); // 노드의 연결점을 만들기 위한 Dictionary

    private float PlayerSpeed = 0.2f; // 플레이어 오브젝트의 이동 속도

    private void Start()
    {
        Player1_transform = Node1[0]; // 초기 노드 포지션 설정
        Player2_transform = Node2[1]; // 초기 노드 포지션 설정
        Player3_transform = Node3[0]; // 초기 노드 포지션 설정

        Node1Set();
        Node2Set();
        Node3Set();
    }

    /// <summary>
    /// 노드의 이동가능 Transform을 셋팅하는 메소드
    /// </summary>
    private void Node1Set()
    {
        foreach (Transform obj in Node1)
        {
            Node1_Connection[obj] = new List<Transform>();
        }

        Node1_Connection[Node1[0]] = new List<Transform> { Node1[1] };
        Node1_Connection[Node1[1]] = new List<Transform> { Node1[0], Node1[2], Node1[4] };
        Node1_Connection[Node1[2]] = new List<Transform> { Node1[1], Node1[3] };
        Node1_Connection[Node1[3]] = new List<Transform> { Node1[2] };
        Node1_Connection[Node1[4]] = new List<Transform> { Node1[1], Node1[5], Node1[6] };
        Node1_Connection[Node1[5]] = new List<Transform> { Node1[4] };
        Node1_Connection[Node1[6]] = new List<Transform> { Node1[4], Node1[7], Node1[16] };
        Node1_Connection[Node1[7]] = new List<Transform> { Node1[6], Node1[8] };
        Node1_Connection[Node1[8]] = new List<Transform> { Node1[7], Node1[9] };
        Node1_Connection[Node1[9]] = new List<Transform> { Node1[8], Node1[10] };
        Node1_Connection[Node1[10]] = new List<Transform> { Node1[9], Node1[11] };
        Node1_Connection[Node1[11]] = new List<Transform> { Node1[10], Node1[12] };
        Node1_Connection[Node1[12]] = new List<Transform> { Node1[11], Node1[13], Node1[19] };
        Node1_Connection[Node1[13]] = new List<Transform> { Node1[12], Node1[14] };
        Node1_Connection[Node1[14]] = new List<Transform> { Node1[13], Node1[15] };
        Node1_Connection[Node1[15]] = new List<Transform> { Node1[14] };
        Node1_Connection[Node1[16]] = new List<Transform> { Node1[6], Node1[17], Node1[18] };
        Node1_Connection[Node1[17]] = new List<Transform> { Node1[16] };
        Node1_Connection[Node1[18]] = new List<Transform> { Node1[16], Node1[19] };
        Node1_Connection[Node1[19]] = new List<Transform> { Node1[12], Node1[18] };
    }

    /// <summary>
    /// 노드의 이동가능 Transform을 셋팅하는 메소드
    /// </summary>
    private void Node2Set()
    {
        foreach (Transform obj in Node2)
        {
            Node2_Connection[obj] = new List<Transform>();
        }

        Node2_Connection[Node2[0]] = new List<Transform> { Node2[1], Node2[2] };
        Node2_Connection[Node2[1]] = new List<Transform> { Node2[0] };
        Node2_Connection[Node2[2]] = new List<Transform> { Node2[0], Node2[3], Node2[5] };
        Node2_Connection[Node2[3]] = new List<Transform> { Node2[2], Node2[4] };
        Node2_Connection[Node2[4]] = new List<Transform> { Node2[3] };
        Node2_Connection[Node2[5]] = new List<Transform> { Node2[2], Node2[6] };
        Node2_Connection[Node2[6]] = new List<Transform> { Node2[5], Node2[7] };
        Node2_Connection[Node2[7]] = new List<Transform> { Node2[6], Node2[8] };
        Node2_Connection[Node2[8]] = new List<Transform> { Node2[7], Node2[9] };
        Node2_Connection[Node2[9]] = new List<Transform> { Node2[8], Node2[10] };
        Node2_Connection[Node2[10]] = new List<Transform> { Node2[9], Node2[11] };
        Node2_Connection[Node2[11]] = new List<Transform> { Node2[10], Node2[12] };
        Node2_Connection[Node2[12]] = new List<Transform> { Node2[11], Node2[13], Node2[17] };
        Node2_Connection[Node2[13]] = new List<Transform> { Node2[12], Node2[14], Node2[15], Node2[16] };
        Node2_Connection[Node2[14]] = new List<Transform> { Node2[13] };
        Node2_Connection[Node2[15]] = new List<Transform> { Node2[13] };
        Node2_Connection[Node2[16]] = new List<Transform> { Node2[13], Node2[17] };
        Node2_Connection[Node2[17]] = new List<Transform> { Node2[12], Node2[16], Node2[18], Node2[19] };
        Node2_Connection[Node2[18]] = new List<Transform> { Node2[17] };
        Node2_Connection[Node2[19]] = new List<Transform> { Node2[17] };
    }

    /// <summary>
    /// 노드의 이동가능 Transform을 셋팅하는 메소드
    /// </summary>
    private void Node3Set()
    {
        foreach (Transform obj in Node3)
        {
            Node3_Connection[obj] = new List<Transform>();
        }

        Node3_Connection[Node3[0]] = new List<Transform> { Node3[1] };
        Node3_Connection[Node3[1]] = new List<Transform> { Node3[0], Node3[2], Node3[3] };
        Node3_Connection[Node3[2]] = new List<Transform> { Node3[1] };
        Node3_Connection[Node3[3]] = new List<Transform> { Node3[1], Node3[4] };
        Node3_Connection[Node3[4]] = new List<Transform> { Node3[3], Node3[5] };
        Node3_Connection[Node3[5]] = new List<Transform> { Node3[4], Node3[6] };
        Node3_Connection[Node3[6]] = new List<Transform> { Node3[5], Node3[7], Node3[9] };
        Node3_Connection[Node3[7]] = new List<Transform> { Node3[6], Node3[8] };
        Node3_Connection[Node3[8]] = new List<Transform> { Node3[7] };
        Node3_Connection[Node3[9]] = new List<Transform> { Node3[6], Node3[10] };
        Node3_Connection[Node3[10]] = new List<Transform> { Node3[9], Node3[11] };
        Node3_Connection[Node3[11]] = new List<Transform> { Node3[10], Node3[12] };
        Node3_Connection[Node3[12]] = new List<Transform> { Node3[11], Node3[13] };
        Node3_Connection[Node3[13]] = new List<Transform> { Node3[12], Node3[14] };
        Node3_Connection[Node3[14]] = new List<Transform> { Node3[13], Node3[15] };
        Node3_Connection[Node3[15]] = new List<Transform> { Node3[14], Node3[16], Node3[17] };
        Node3_Connection[Node3[16]] = new List<Transform> { Node3[15] };
        Node3_Connection[Node3[17]] = new List<Transform> { Node3[15], Node3[18] };
        Node3_Connection[Node3[18]] = new List<Transform> { Node3[17], Node3[19] };
        Node3_Connection[Node3[19]] = new List<Transform> { Node3[18], Node3[20] };
        Node3_Connection[Node3[20]] = new List<Transform> { Node3[19] };
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                switch (hit.transform.name)
                {
                    case "Up":
                        AudioManager.Instance.PlaySFX("SFX_MoveMaze");
                        Player1_MoveCheck(Vector3.up);
                        Player2_MoveCheck(Vector3.up);
                        Player3_MoveCheck(Vector3.up);
                        ClearCheck();
                        break;

                    case "Down":
                        AudioManager.Instance.PlaySFX("SFX_MoveMaze");
                        Player1_MoveCheck(Vector3.down);
                        Player2_MoveCheck(Vector3.down);
                        Player3_MoveCheck(Vector3.down);
                        ClearCheck();
                        break;

                    case "Left":
                        AudioManager.Instance.PlaySFX("SFX_MoveMaze");
                        Player1_MoveCheck(Vector3.left);
                        Player2_MoveCheck(Vector3.left);
                        Player3_MoveCheck(Vector3.left);
                        ClearCheck();
                        break;

                    case "Right":
                        AudioManager.Instance.PlaySFX("SFX_MoveMaze");
                        Player1_MoveCheck(Vector3.right);
                        Player2_MoveCheck(Vector3.right);
                        Player3_MoveCheck(Vector3.right);
                        ClearCheck();
                        break;

                    case "Reset":
                        AudioManager.Instance.PlaySFX("SFX_PushButton2");
                        PuzzleReSet();
                        break;
                }

            }
        }
    }

    /// <summary>
    /// 해당 위치로 움직일 수 있는지 체크하여 움직이는 메소드
    /// </summary>
    /// <param name="direction"></param>
    private void Player1_MoveCheck(Vector3 direction)
    {
        Vector3 newPosition = Player1_transform.position + direction * 0.25f;

        if (Node1_Connection.ContainsKey(Player1_transform))
        {
            foreach (Transform connectedObject in Node1_Connection[Player1_transform])
            {
                if (Vector3.Distance(connectedObject.position, newPosition) < 0.1f)
                {
                    Player1_transform = connectedObject;
                    Player1.transform.DOLocalMove(Player1_transform.localPosition, PlayerSpeed).SetEase(Ease.Linear);
                    break;
                }   
            }
        }
    }

    /// <summary>
    /// 해당 위치로 움직일 수 있는지 체크하여 움직이는 메소드
    /// </summary>
    /// <param name="direction"></param>
    private void Player2_MoveCheck(Vector3 direction)
    {
        Vector3 newPosition = Player2_transform.position + direction * 0.25f;

        if (Node2_Connection.ContainsKey(Player2_transform))
        {
            foreach (Transform connectedObject in Node2_Connection[Player2_transform])
            {
                if (Vector3.Distance(connectedObject.position, newPosition) < 0.1f)
                {
                    Player2_transform = connectedObject;
                    Player2.transform.DOLocalMove(Player2_transform.localPosition, PlayerSpeed).SetEase(Ease.Linear);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 해당 위치로 움직일 수 있는지 체크하여 움직이는 메소드
    /// </summary>
    /// <param name="direction"></param>
    private void Player3_MoveCheck(Vector3 direction)
    {
        Vector3 newPosition = Player3_transform.position + direction * 0.25f;

        if (Node3_Connection.ContainsKey(Player3_transform))
        {
            foreach (Transform connectedObject in Node3_Connection[Player3_transform])
            {
                if (Vector3.Distance(connectedObject.position, newPosition) < 0.1f)
                {
                    Player3_transform = connectedObject;
                    Player3.transform.DOLocalMove(Player3_transform.localPosition, PlayerSpeed).SetEase(Ease.Linear);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 리셋 버튼을 누를경우 초기위치로 리셋하기 위한 메소드
    /// </summary>
    private void PuzzleReSet()
    {
        Player1_transform = Node1[0];
        Player2_transform = Node2[1];
        Player3_transform = Node3[0];
        Player1.transform.localPosition = Player1_transform.localPosition;
        Player2.transform.localPosition = Player2_transform.localPosition;
        Player3.transform.localPosition = Player3_transform.localPosition;
    }

    /// <summary>
    /// 퍼즐의 정답을 체크하기위한 메소드
    /// </summary>
    private void ClearCheck()
    {
        if(Player1_transform == Node1[15] && Player2_transform == Node2[19] && Player3_transform == Node3[20])
        {
            AudioManager.Instance.PlaySFX("SFX_UseKey");
            PuzzleClear();
        }
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
