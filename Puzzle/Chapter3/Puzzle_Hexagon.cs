using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class Puzzle_Hexagon : Puzzle
{
    [SerializeField] private List<Transform> Node;          // 노드를 저장하기 위핸 Transform의 List
    [SerializeField] private List<GameObject> Objects;      // 움직일 오브젝트를 담아둔 GameObject의 List
    [SerializeField] private Puzzle_Check[] puzzlecheck;    // 퍼즐의 정답을 체크하기위해 만든 배열
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer

    private Transform EmptyNode;                            // 비어있는 노드의 Transform

    private Dictionary<Transform, List<Transform>> Node_Connection = new Dictionary<Transform, List<Transform>>(); // 이동가능한 노드를 연결하는 Dictionary

    private void Start()
    {
        foreach (Transform obj in Node)
        {
            Node_Connection[obj] = new List<Transform>();
        }

        EmptyNode = Node[0];

        Node_Connection[Node[0]] = new List<Transform> { Node[3], Node[4], Node[5], Node[6] };
        Node_Connection[Node[1]] = new List<Transform> { Node[4], Node[5] };
        Node_Connection[Node[2]] = new List<Transform> { Node[3], Node[6] };
        Node_Connection[Node[3]] = new List<Transform> { Node[0], Node[2], Node[4] };
        Node_Connection[Node[4]] = new List<Transform> { Node[0], Node[1], Node[3] };
        Node_Connection[Node[5]] = new List<Transform> { Node[0], Node[1], Node[6] };
        Node_Connection[Node[6]] = new List<Transform> { Node[0], Node[2], Node[5] };
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
                MoveObjectCheck(hit.transform.gameObject);
            }
        }
    }

    /// <summary>
    /// 터치한 오브젝트가 이동가능한지 체크 후 가능하다면 이동
    /// </summary>
    /// <param name="obj"></param>
    private void MoveObjectCheck(GameObject obj)
    {
        Transform currentNode = obj.transform.parent; 

        if (Node_Connection.ContainsKey(currentNode) && Node_Connection[currentNode].Contains(EmptyNode))
        {
            obj.transform.parent = EmptyNode;
            AudioManager.Instance.PlaySFX("SFX_MoveHexagon");
            obj.transform.DOMove(EmptyNode.position, 0.3f).SetEase(Ease.Linear).OnComplete(ClearCheck);
            EmptyNode = currentNode;

        }
    }

    /// <summary>
    /// 정답을 체크하는 메소드
    /// </summary>
    private void ClearCheck()
    {
        foreach (Puzzle_Check Hexagon in puzzlecheck)
        {
            if (!Hexagon.IsPuzzleNameCheck())
            {
                return;
            }
        }
        PuzzleClear();
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
