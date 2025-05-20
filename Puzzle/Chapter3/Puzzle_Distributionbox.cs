using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Distributionbox : Puzzle
{
    /// <summary>
    /// ����ġ�� On���� Off���� üũ�ϱ����� enum
    /// </summary>
    public enum OnOff
    {
        Off,
        On
    }

    [SerializeField] private Puzzle_Check[] Switches;       // ������Ʈ�� enum�� �̿��Ͽ� ������ üũ�ϱ����� �迭
    [SerializeField] private Material[] Light;              // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private GameObject Checkanswer;        // ������ üũ�ϴ� ������Ʈ�� ��Ƶδ� GameObject
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer
    private bool PuzzleCheck;                               // ������ ������ üũ������ Ȯ���ϴ� bool
    private int[] ClearNumber = new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 0 }; // ������ ���� üũ�� ���� int �迭

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (PuzzleCheck) return;

        if (context.canceled)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                if(hit.transform.gameObject == Checkanswer)
                {
                    PuzzleCheck = true;
                    AudioManager.Instance.PlaySFX("SFX_PushLever");
                    ClearCheck();
                }
                else
                {
                    OnoffChange(hit.transform.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// ������Ʈ ��ġ�� On�� Off�� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="obj"></param>
    private void OnoffChange(GameObject obj)
    {
        Puzzle_Check puzzleCheck = obj.GetComponent<Puzzle_Check>();
        if (puzzleCheck == null) return;

        AudioManager.Instance.PlaySFX("SFX_PushSwitch");

        if (puzzleCheck.onoffswitch == OnOff.On)
        {
            obj.transform.DOLocalMoveX(0.0702f, 0f);
            puzzleCheck.onoffswitch = OnOff.Off;
        }
        else
        {
            obj.transform.DOLocalMoveX(-0.0700f, 0f);
            puzzleCheck.onoffswitch = OnOff.On;
        }
    }

    /// <summary>
    /// ������ üũ�ϱ� ���� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        for (int i = 0; i < Switches.Length; i++)
        {

            if (Switches[i].onoffswitch != (OnOff)ClearNumber[i])
            {
                StartCoroutine(Failure());
                AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");
                return;
            }
        }
        Checkanswer.transform.DOLocalRotate(new Vector3(10.279f, 270.116f, -363.121f), 0.25f).SetEase(Ease.Linear);
        Light[0].EnableKeyword("_EMISSION");
        AudioManager.Instance.PlaySFX("SFX_CorrectSound");
        PuzzleClear();
    }

    /// <summary>
    /// ���� ���� �� ���׸����� �Ӽ��� �����ϴ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator Failure()
    {
        Checkanswer.transform.DOLocalRotate(new Vector3(10.279f, 270.116f, -363.121f), 0.25f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.25f);
        Light[1].EnableKeyword("_EMISSION");
        Checkanswer.transform.DOLocalRotate(new Vector3(-81.046f, 284.103f, -375.635f), 0.25f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.25f);
        PuzzleCheck = false;
        yield return new WaitForSeconds(0.25f);
        Light[1].DisableKeyword("_EMISSION");
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
