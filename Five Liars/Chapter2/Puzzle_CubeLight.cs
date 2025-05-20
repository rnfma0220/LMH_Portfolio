using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_CubeLight : Puzzle
{
    [SerializeField] private Material[] Cube_Material;          // 마테리얼의 속성을 설정하기위해 담아둔 Material 배열
    [SerializeField] private LayerMask layer;                   // 특정 레이어 체크를 위한 Layer
    private bool IsHint = false;                                // 힌트가 동작중인지 체크하는 bool
    private float LightTime = 0.6f;                             // 코루틴에서 Wait시간으로 사용하기위한 float

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
                if (hit.transform.name.Equals("Triger_Button") || !IsHint)
                {
                    AudioManager.Instance.PlaySFX("SFX_PushButton1");
                    StartCoroutine(HintStart(hit.transform.gameObject));
                }
            }
        }
    }

    /// <summary>
    /// 힌트를 타이밍에 맞춰서 재생하는 코루틴
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
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
