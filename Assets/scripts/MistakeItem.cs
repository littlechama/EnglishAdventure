using UnityEngine;
using TMPro;
using System;

public class MistakeItem : MonoBehaviour
{
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI infoText;
    // データをセットするメソッド
    public void SetData(string word, string meaning, int wrongCount, float accuracy, int rank)
    {
        wordText.text = word;
        // 色を変えたりサイズを変えたりして見やすく整形
        infoText.text = $"{meaning}\n<size=80%><color=red>ミス: {wrongCount}回</color> (正答率: {accuracy:F0}%)</size> [{rank}]";
    }
}
