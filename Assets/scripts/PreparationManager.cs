using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Buttonなどを扱うため
using UnityEngine.EventSystems; // コントローラーでの選択制御に必要
using System.Collections.Generic; // Listを使うため

public class PreparationManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mistakeItemPrefab; // 手順1で作ったプレハブ
    public Transform contentParent;      // 手順2のContentオブジェクト
    public Button startButton;           // スタートボタン

    void Start()
    {
        GenerateMistakeList();

        // 開始時にスタートボタンを選択状態にする（コントローラーですぐ押せるように）
        if (startButton != null)
        {
            startButton.Select();
        }
    }

    void GenerateMistakeList()
    {
        // 念のため、既存のリストがあれば消しておく（再描画用）
        foreach (Transform child in contentParent){Destroy(child.gameObject);}

        bool isFirstItem = true;

        foreach (var pair in ScoreManager.scoreMap)
        {
            string word = pair.Key;
            WordScore score = pair.Value;

            // 間違えたことがある単語のみ表示
            if (score.wrongCount > 0)
            {
                // プレハブを生成
                GameObject obj = Instantiate(mistakeItemPrefab, contentParent);
                var database = WordDatabase.instance;
                // データを書き込む
                MistakeItem itemScript = obj.GetComponent<MistakeItem>();
                if (itemScript != null)
                {
                    itemScript.SetData(word, database.GetData(word).meaning, score.wrongCount, score.Accuracy, database.GetData(word).rank);
                }
            }
        }
    }

    public void startButtonDown()
    {
        SceneManager.LoadScene("GameScene");
    }
}