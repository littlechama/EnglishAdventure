using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class WordDatabase : MonoBehaviour
{
    // 【追加 1】どこからでもアクセスできる「自分自身」の分身
    public static WordDatabase instance;

    [SerializeField] private string csvFileName = "tSL_WLS";
    public List<ToeicWord> wordList = new List<ToeicWord>();

    // 【変更】Start ではなく Awake にする
    // (他のスクリプトの Start が動く前に読み込みを終わらせるため)
    void Awake()
    {
        // 【追加 2】シングルトンの設定
        if (instance == null)
        {
            instance = this;
            LoadCSV(); // ここで読み込み実行
        }
        else
        {
            Destroy(gameObject); // 2つ以上あったら消す
        }
    }

    // CSVを読み込むメイン処理
    public void LoadCSV()
    {
        // Resourcesフォルダからテキストとして読み込む
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError("CSVファイルが見つかりません！Resourcesフォルダを確認してください。");
            return;
        }

        // 行ごとに分割する
        StringReader reader = new StringReader(csvFile.text);
        string line;
        int lineCount = 0;

        // リストをクリア
        wordList.Clear();

        while ((line = reader.ReadLine()) != null)
        {
            lineCount++;

            // 1行目はヘッダー（項目名）なので無視する
            if (lineCount == 1) continue;
            // 空行は無視
            if (string.IsNullOrWhiteSpace(line)) continue;

            // CSVの行をパース（分解）する
            string[] values = SplitCsvLine(line);

            // カラム数が足りているかチェック（4項目あるはず）
            if (values.Length >= 4)
            {
                ToeicWord newWord = new ToeicWord();

                // CSVの並び順に合わせて代入
                // 0: word, 1: meaning_short, 2: meaning_raw, 3: tsl rank
                newWord.word = values[0];
                newWord.pos = values[1];
                newWord.meaning = values[2];

                // ランクは数字なのでintに変換
                if (int.TryParse(values[3], out int rankResult))
                {
                    newWord.rank = rankResult;
                }
                else
                {
                    newWord.rank = 9999; // エラー時は適当な値を
                }

                wordList.Add(newWord);
            }
        }

        Debug.Log($"読み込み完了！合計 {wordList.Count} 単語をロードしました。");
    }

    // 【重要】カンマ区切りの分解処理
    // 単純な .Split(',') だと、意味の中に「,」があった時にバグるため、
    // 引用符("")で囲まれたカンマは無視するロジックです。
    private string[] SplitCsvLine(string line)
    {
        // 正規表現を使ってCSVを正しく分割するパターン
        // (引用符内のカンマは区切り文字とみなさない)
        string pattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        string[] rawValues = Regex.Split(line, pattern);

        for (int i = 0; i < rawValues.Length; i++)
        {
            // 前後の引用符(")を削除し、2重引用符("")を1つに戻す
            rawValues[i] = rawValues[i].TrimStart('"').TrimEnd('"').Replace("\"\"", "\"");
        }
        return rawValues;
    }
    public ToeicWord GetData(string searchWord)
    {
        return wordList.FirstOrDefault(w => w.word == searchWord);
    }
}