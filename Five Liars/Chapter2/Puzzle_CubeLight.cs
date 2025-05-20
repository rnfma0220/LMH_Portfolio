using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_CubeLight : Puzzle
{
    [SerializeField] private Material[] Cube_Material;          // ���׸����� �Ӽ��� �����ϱ����� ��Ƶ� Material �迭
    [SerializeField] private LayerMask layer;                   // Ư�� ���̾� üũ�� ���� Layer
    private bool IsHint = false;                                // ��Ʈ�� ���������� üũ�ϴ� bool
    private float LightTime = 0.6f;                             // �ڷ�ƾ���� Wait�ð����� ����ϱ����� float

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
                if (hit.transform.name.Equals("Triger_Button") || !IsHint)
                {
                    AudioManager.Instance.PlaySFX("SFX_PushButton1");
                    StartCoroutine(HintStart(hit.transform.gameObject));
                }
            }
        }
    }

    /// <summary>
    /// ��Ʈ�� Ÿ�ֿ̹� ���缭 ����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="hitobject"></param>
    /// <returns></returns>
    private IEnumerator HintStart(GameObject hitobject)
    {
        hitobject.transform.DOLocalMoveZ(-0.688f, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.75f);
        Cube_Material[2].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[2].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[3].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[3].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[7].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[7].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[0].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[0].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[4].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[4].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[6].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[6].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[1].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[1].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[5].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[5].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[8].SetFloat("_Cutoff", 0.5f);
        yield return new WaitForSeconds(LightTime);
        Cube_Material[8].SetFloat("_Cutoff", 0f);
        yield return new WaitForSeconds(LightTime);
        hitobject.transform.DOLocalMoveZ(0.434f, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        IsHint = false;
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
