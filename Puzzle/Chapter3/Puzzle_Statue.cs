using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Puzzle_Statue : Puzzle
{
    [SerializeField] private Material[] Light;          // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private LayerMask layer;           // Ư�� ���̾� üũ�� ���� Layer
    private const string PuzzleNumber = "8244";         // ������ ����üũ�� ���� ������ ��Ƶ� const string
    private string NumberCheck = string.Empty;          // ������ ���� üũ�� ���� string
    private bool isChecking = false;                    // ������ üũ�ϴ� ������ Ȯ���ϴ� bool
    private GameObject hitObject;                       // ��ġ�� ������Ʈ�� ��Ƶδ� GameObject

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            if (hitObject != null) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer) && !isChecking)
            {
                hitObject = hit.transform.gameObject;
                Buttoninput();
            }
        }
    }

    /// <summary>
    /// ��ġ�� ������Ʈ�� ���� �ְ� �Էµ� ������ Ȯ���Ͽ� ����üũ�� ȣ���ϴ� �޼ҵ�
    /// </summary>
    private void Buttoninput()
    {
        NumberCheck += hitObject.name;
        hitObject.transform.DOLocalMoveZ(0.394f, 0.15f).SetEase(Ease.Linear).OnComplete(ButtonUp); ;
        int inputStep = NumberCheck.Length - 1;
        Light[inputStep].EnableKeyword("_EMISSION");
        AudioManager.Instance.PlaySFX("SFX_PushButton1");


        if (NumberCheck.Length == 4)
        {
            if (ClearCheck())
            {
                PuzzleClear();
            }
            else
            {
                StartCoroutine(Puzzlefailure());
            }

            isChecking = true;
        }
    }

    /// <summary>
    /// ��ġ�� ������Ʈ�� �������� �ٽ� ���󺹱� ��Ű�� �޼���
    /// </summary>
    private void ButtonUp()
    {
        hitObject.transform.DOLocalMoveZ(0.412f, 0.15f).SetEase(Ease.Linear);
        hitObject = null;
    }

    /// <summary>
    /// ������ Ʋ������ Ʋ������� �ð��� ȿ���� �ִ� �޼���
    /// </summary>
    /// <returns></returns>
    private IEnumerator Puzzlefailure()
    {
        AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");

        for (int i = 0; i < 3; i++)
        {
            foreach (var material in Light)
            {
                material.DisableKeyword("_EMISSION");
            }

            yield return new WaitForSeconds(0.2f);

            foreach (var material in Light)
            {
                material.EnableKeyword("_EMISSION");
            }

            yield return new WaitForSeconds(0.2f);
        }

        foreach (var material in Light)
        {
            material.DisableKeyword("_EMISSION");
        }

        isChecking = false;
        NumberCheck = string.Empty;
    }

    /// <summary>
    /// ������ üũ�ϴ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    private bool ClearCheck()
    {
        if (PuzzleNumber.Equals(NumberCheck))
        {
            return true;
        }

        return false;
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
