using UnityEngine;
using System.IO;
using System.Text;

public static class SaveSystem
{
    // 保存先のファイル名
    private static string fileName = "save_data.json";

    // 保存処理
    public static void SaveGame(GameSaveData data)
    {
        // 1. クラスをJSON文字列に変換
        string json = JsonUtility.ToJson(data, true); // trueにすると人間が読みやすい整形になる

        // 2. パスを決める（Application.persistentDataPath はスマホでもPCでも安全な保存場所）
        string path = Path.Combine(Application.persistentDataPath, fileName);

        // 3. 書き込み
        File.WriteAllText(path, json, Encoding.UTF8);

        Debug.Log("セーブしました: " + path);
    }

    // 読込処理
    public static GameSaveData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        // ファイルが存在するか確認
        if (File.Exists(path))
        {
            // 1. テキストを読み込む
            string json = File.ReadAllText(path);

            // 2. JSON文字列をクラスに変換して返す
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        else
        {
            // ファイルがない＝初回プレイなので、空のデータを作って返す
            Debug.Log("セーブデータがありません。新規作成します。");
            return new GameSaveData();
        }
    }
}