using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_CoffeeMachine : Puzzle
{
    [SerializeField] private GameObject[] Coffee;       // ����� ���� ������Ʈ���� ��Ƶδ� GameObject �迭
    [SerializeField] private Transform button;          // ����üũ�� ����� ������Ʈ�� ��Ƶδ� Transform
    [SerializeField] private LayerMask layer;           // Ư�� ���̾� üũ�� ���� Layer

    private int Alpha = 0;                              // ������ üũ�ϱ� ���� ���� int 
    private int Beta = 0;                               // ������ üũ�ϱ� ���� ���� int
    private int Gamma = 0;                              // ������ üũ�ϱ� ���� ���� int

    private bool Coffee_1, Coffee_2, Coffee_3, Coffee_4, IsTouch;   // �̹� ���� ������Ʈ���� ��ġ������ üũ�ϴ� bool

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
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
            if (IsTouch) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                GameObject hitObject = hit.transform.gameObject;

                switch (hitObject.name) // �浹�� ������Ʈ�� name�� Ȯ���Ͽ� üũ
                {
                    case "Choco":
                        Alpha += 3;
                        Beta += 1;
                        StartCoroutine(Button_Co(hitObject));
                        break;
                    case "Honey":
                        Alpha -= 1;
                        Beta += 2;
                        Gamma += 2;
                        StartCoroutine(Button_Co(hitObject));
                        break;
                    case "CoffeeBean":
                        Alpha += 2;
                        Gamma -= 1;
                        StartCoroutine(Button_Co(hitObject));
                        break;
                    case "Milk":
                        Beta -= 1;
                        StartCoroutine(Button_Co(hitObject));
                        break;
                    case "Water":
                        Gamma += 2;
                        StartCoroutine(Button_Co(hitObject));
                        break;
                    case "CoffeeButton":
                        IsTouch = true;
                        button.transform.DOLocalMoveZ(2.017f, 0.25f).SetEase(Ease.Linear).OnComplete(Check);
                        AudioManager.Instance.PlaySFX("SFX_PushButton2");
                        break;
                    default:
                        break;

                }
            }
        }
    }

    /// <summary>
    /// ��ġ�� ������Ʈ�� �������� �ð�ȿ���� ���� �ڷ�ƾ
    /// </summary>
    /// <param name="obj">��ġ�� ������Ʈ�� ����</param>
    /// <returns></returns>
    private IEnumerator Button_Co(GameObject obj)
    {
        obj.transform.DOLocalMoveY(0.32f, 0.15f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.15f);
        AudioManager.Instance.PlaySFX("SFX_PushButton1");
        obj.transform.DOLocalMoveY(0.327f, 0.15f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.15f);
    }

    /// <summary>
    /// Alpha, Beta, Gamma �� ���� ���� ���� üũ
    /// </summary>
    private void Check()
    {
        if (Alpha == 2 && Beta == 0 && Gamma == 1 && !Coffee_1)
        {
            Coffee_1 = true;
            Coffee[0].GetComponent<Rigidbody>().useGravity = true;
            AudioManager.Instance.PlaySFX("SFX_FallCoffeeToten");
        }

        else if (Alpha == 2 && Beta == -2 && Gamma == -1 && !Coffee_2)
        {
            Coffee_2 = true;
            Coffee[1].GetComponent<Rigidbody>().useGravity = true;
            AudioManager.Instance.PlaySFX("SFX_FallCoffeeToten");
        }

        else if (Alpha == 1 && Beta == 1 && Gamma == 1 && !Coffee_3)
        {
            Coffee_3 = true;
            Coffee[2].GetComponent<Rigidbody>().useGravity = true;
            AudioManager.Instance.PlaySFX("SFX_FallCoffeeToten");
        }

        else if (Alpha == 4 && Beta == 2 && Gamma == 1 && !Coffee_4)
        {
            Coffee_4 = true;
            Coffee[3].GetComponent<Rigidbody>().useGravity = true;
            AudioManager.Instance.PlaySFX("SFX_FallCoffeeToten");
        }

        button.DOLocalMoveZ(2.01f, 0.25f).SetEase(Ease.Linear);
        Alpha = 0; Beta = 0; Gamma = 0;
        IsTouch = false;
    }

}
