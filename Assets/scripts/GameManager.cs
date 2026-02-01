using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UIを使う場合

public class GameManager : MonoBehaviour
{
    public TextMeshPro questionText;
    public TextMeshPro[] ansText;
    public TextMeshPro timerText;

    public AnsZone[] zones;

    public float timeLimit = 7.0f;
    float currentTimer = 0;
    private int correctAnsIndex;
    private bool isAnswering = false;
    private int questionCount=0;
    private ToeicWord correctWord;

    void Start()
    {
        // ゲーム開始時に1問出してみる
        currentTimer = timeLimit;
        DisplayNextQuestion();
    }

    private void Update()
    {
        if (isAnswering) { currentTimer -= Time.deltaTime; timerText.text = Mathf.Max(0, currentTimer).ToString("F1"); }
        if (currentTimer <0 )
        {
            currentTimer = timeLimit;
            EvaluateAnswer();
        }
    }
    public void DisplayNextQuestion()
    {
        isAnswering = true;
        var database = WordDatabase.instance;
        questionCount++;

        // 1. 正解の単語を1つ選ぶ
        int correctIndex = Random.Range(0, database.wordList.Count);
        correctWord = database.wordList[correctIndex];
        ScoreManager.RecordShow(correctWord.word);

        // 2. 「同じ品詞」かつ「正解ではない」単語をデータベースから抽出
        List<ToeicWord> samePosWords = database.wordList
            .Where(w => w.pos == correctWord.pos && w.word != correctWord.word)
            .ToList();
        // 3. 間違いの選択肢をランダムに3つ選ぶ
        List<ToeicWord> wrongChoices = samePosWords
            .OrderBy(x => Random.value)
            .Take(3)
            .ToList();

        // 4. 正解と間違いを1つのリストにまとめ、さらにシャッフルする
        List<ToeicWord> choices = new List<ToeicWord>(wrongChoices);
        choices.Add(correctWord);
        choices = choices.OrderBy(x => Random.value).ToList();

        // 5. UIに反映
        questionText.text = correctWord.word; // 問題は単語

        for (int i = 0; i < 4; i++)
        {
            if (i < choices.Count)
            {
                ansText[i].text = choices[i].meaning; // 選択肢は日本語

                if (choices[i].meaning == correctWord.meaning)
                {
                    correctAnsIndex = i;
                }
            }
        }
    }
    public void EvaluateAnswer()
    {
        isAnswering = false; // 判定中
        int playerZoneIndex = -1;

        // どのゾーンにプレイヤーがいるか探す
        for (int i = 0; i < zones.Length; i++)
        {
            if (zones[i].isPlayerInside)
            {
                playerZoneIndex = zones[i].zoneIndex;
                break;
            }
        }

        // 判定
        if (playerZoneIndex == correctAnsIndex)
        {
            timerText.text = "正解！";
            // 正解エフェクトなど
            ScoreManager.RecordAnswer(correctWord.word, true);
        }
        else
        {
            timerText.text = "不正解！正解は"+ correctAnsIndex;
            // 不正解エフェクトなど
            ScoreManager.RecordAnswer(correctWord.word, false);
        }

        // 2秒後に次の問題へ
        Invoke("DisplayNextQuestion", 2.0f);
        if (questionCount > 5)
        {
            SceneManager.LoadScene("Preparationscene");
        }
    }
}