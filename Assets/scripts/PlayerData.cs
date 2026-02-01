using System.Collections.Generic;
using System;

// 1. 保存したい「単語1個分のデータ」と「単語名」のセット
// (Dictionaryは保存できないので、このクラスのリストとして保存します)
[Serializable]
public class WordScoreEntry
{
    public string wordKey;      // 単語のスペル（辞書のキー）
    public WordScore scoreData; // 成績データ（辞書の値）

    // コンストラクタ（作成時にデータを入れやすくする）
    public WordScoreEntry(string key, WordScore val)
    {
        wordKey = key;
        scoreData = val;
    }
}

// 2. プレイヤー1人分のデータ
[Serializable]
public class PlayerData
{
    public string playerName; // 名前
    public string lastLoginDate; // 最後に遊んだ日（必要なら）

    // ★保存用：JSONにするためのリスト
    public List<WordScoreEntry> scoreList = new List<WordScoreEntry>();

    // ---------------------------------------------------------
    // 以下の2つの関数で、Dictionary <-> List を変換します
    // ---------------------------------------------------------

    // A. セーブ前：今のDictionaryの中身を、保存用のListに詰め替える
    public void PrepareForSave(Dictionary<string, WordScore> runtimeDict)
    {
        scoreList.Clear();
        foreach (var pair in runtimeDict)
        {
            scoreList.Add(new WordScoreEntry(pair.Key, pair.Value));
        }
    }

    // B. ロード後：保存されていたListの中身を、使いやすいDictionaryに戻す
    public Dictionary<string, WordScore> ConvertToDictionary()
    {
        Dictionary<string, WordScore> dict = new Dictionary<string, WordScore>();
        foreach (var entry in scoreList)
        {
            // 同じ単語が万が一あってもエラーにならないようにチェック
            if (!dict.ContainsKey(entry.wordKey))
            {
                dict.Add(entry.wordKey, entry.scoreData);
            }
        }
        return dict;
    }
}

// 3. ゲーム全体のセーブデータ（全プレイヤー分をまとめる箱）
[Serializable]
public class GameSaveData
{
    // 登録されているプレイヤーのリスト（最大4人などを想定）
    public List<PlayerData> players = new List<PlayerData>();
}