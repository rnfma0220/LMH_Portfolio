using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class Puzzle_Hexagon : Puzzle
{
    [SerializeField] private List<Transform> Node;          // ��带 �����ϱ� ���� Transform�� List
    [SerializeField] private List<GameObject> Objects;      // ������ ������Ʈ�� ��Ƶ� GameObject�� List
    [SerializeField] private Puzzle_Check[] puzzlecheck;    // ������ ������ üũ�ϱ����� ���� �迭
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer

    private Transform EmptyNode;                            // ����ִ� ����� Transform

    private Dictionary<Transform, List<Transform>> Node_Connection = new Dictionary<Transform, List<Transform>>(); // �̵������� ��带 �����ϴ� Dictionary

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
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
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
    /// ��ġ�� ������Ʈ�� �̵��������� üũ �� �����ϴٸ� �̵�
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
    /// ������ üũ�ϴ� �޼ҵ�
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
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
