[System.Serializable] 
public class WordScore
{
    public int shownCount = 0;   // 出題された回数
    public int correctCount = 0; // 正解した回数
    public int wrongCount = 0;   // 間違えた回数

    // 正答率を計算するプロパティ（0〜100%）
    public float Accuracy
    {
        get
        {
            if (shownCount == 0) return 0f;
            return (float)correctCount / shownCount * 100f;
        }
    }
}