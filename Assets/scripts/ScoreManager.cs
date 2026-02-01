using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Findを使うために必要

public static class ScoreManager
{
    // ★システム全体の設定データ（全プレイヤー分）
    private static GameSaveData masterData;

    // ★現在ログイン中のプレイヤーデータ（参照用）
    private static PlayerData currentUser;

    // ゲーム中はこの辞書を使って読み書きする（以前と同じ変数名）
    public static Dictionary<string, WordScore> scoreMap = new Dictionary<string, WordScore>();

    // -----------------------------------------------------------
    // 1. 初期化（ゲーム起動時に1回だけ呼ぶ）
    // -----------------------------------------------------------
    public static void Initialize()
    {
        // ファイルから全員分のデータを読み込む
        masterData = SaveSystem.LoadGame();
        Debug.Log($"データロード完了: 登録プレイヤー数 {masterData.players.Count}人");
    }
    public static void RecordShow(string word)
    {
        if (!scoreMap.ContainsKey(word))
        {
            scoreMap.Add(word, new WordScore());
        }
        scoreMap[word].shownCount++;
    }
    // -----------------------------------------------------------
    // 2. ログイン処理（名前を指定してデータを切り替える）
    // -----------------------------------------------------------
    public static void Login(string playerName)
    {
        // まだ初期化されてなければロードする
        if (masterData == null) Initialize();

        // 名前でリストから検索
        currentUser = masterData.players.FirstOrDefault(p => p.playerName == playerName);

        if (currentUser == null)
        {
            // 新規ユーザー作成
            Debug.Log($"新規ユーザー作成: {playerName}");
            currentUser = new PlayerData();
            currentUser.playerName = playerName;

            // マスターデータに追加
            masterData.players.Add(currentUser);

            // 辞書を空にする
            scoreMap = new Dictionary<string, WordScore>();
        }
        else
        {
            // 既存ユーザー発見
            Debug.Log($"ログイン成功: {playerName}");

            // ★重要: 保存されていたリストを、扱いやすい辞書(Dictionary)に変換してセット
            scoreMap = currentUser.ConvertToDictionary();
        }
    }

    public static void RecordAnswer(string word, bool isCorrect)
    {
        if (isCorrect)
        {
            scoreMap[word].correctCount++;
        }
        else
        {
            scoreMap[word].wrongCount++;
        }
    }
    //単語の成績を取得する
    public static WordScore GetScore(string word)
    {
        if (scoreMap.ContainsKey(word))
        {
            return scoreMap[word];
        }
        return new WordScore();
    }
}
