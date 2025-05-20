using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Check : MonoBehaviour
{
    [SerializeField] private string PuzzleName;         // 자식객체의 String를 확인하기 위한 String
    public Puzzle_Distributionbox.OnOff onoffswitch;    // Puzzle_Distributionbox의 On/Off Enum을 체크하기위한 변수

    /// <summary>
    /// 자식객체의 String값을 확인하여 정답을 체크하는 메소드
    /// </summary>
    /// <returns></returns>
    public bool IsPuzzleNameCheck()
    {
        if (transform.childCount == 0) return false;

        if (!transform.GetChild(0).name.Equals(PuzzleName)) return false;

        return true;
    }

}
