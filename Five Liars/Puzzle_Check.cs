using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Check : MonoBehaviour
{
    [SerializeField] private string PuzzleName;         // �ڽİ�ü�� String�� Ȯ���ϱ� ���� String
    public Puzzle_Distributionbox.OnOff onoffswitch;    // Puzzle_Distributionbox�� On/Off Enum�� üũ�ϱ����� ����

    /// <summary>
    /// �ڽİ�ü�� String���� Ȯ���Ͽ� ������ üũ�ϴ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    public bool IsPuzzleNameCheck()
    {
        if (transform.childCount == 0) return false;

        if (!transform.GetChild(0).name.Equals(PuzzleName)) return false;

        return true;
    }

}
