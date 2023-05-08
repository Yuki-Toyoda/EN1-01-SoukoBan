using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    // プレイヤーのゲームオブジェクト読み込み
    public GameObject playerPrefab;
    public GameObject boxPreafab;
    public GameObject GoalPreafab;

    // クリア時のテキスト
    public GameObject clearText;

	// 配列の要素
	int[,] map; // レベルデザイン用
    GameObject[,] field; // ゲーム管理用の配列

	// プレイヤー位置
	Vector2Int playerIndex;

    // 文字列の宣言と初期化
    string debugText = "";

    // Start is called before the first frame update
    void Start()
	{

        // 配列の実体の作成と初期化
        map = new int[,] {
        {0, 0, 0, 0, 0 },
        {0, 2, 1, 2, 0 },
        {0, 3, 0, 0, 3 }
        };

        field = new GameObject
           [
            map.GetLength(0),
            map.GetLength(1)
           ];

        for(int y = 0; y<map.GetLength(0); y++)
        {
            for(int x = 0; x<map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    // プレイヤーのインスタンスを作成
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                else if (map[y, x ] == 2)
                {
                    // ボックスのインスタンスの作成
                    field[y, x] = Instantiate(
                        boxPreafab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                else if (map[y, x] == 3)
                {
                    Instantiate(
                        GoalPreafab,
                        new Vector3(x, map.GetLength(0) - y, 1),
                        Quaternion.identity
                        );

                }
            }
        }

        // ログにマップ情報を出力
        //OutputMapLog();

    }

	// Update is called once per frame
	void Update()
	{

        // プレイヤーの座標取得
        playerIndex = GetPlayerIndex();

        // プレイヤーの移動処理
        // 上下
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveNumber(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveNumber(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));

        }

        // 左右
        if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            MoveNumber(playerPrefab.tag, playerIndex,new Vector2Int(playerIndex.x + 1, playerIndex.y));
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
            MoveNumber(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
        }

        if(IsCleared())
        {
            // クリア時のテキストをアクティブ
            clearText.SetActive(true);

        }
        else
        {
            // クリアしていないときは非アクティブ
            clearText.SetActive(false);
        }

	}

    /// <summary>
    /// 配列内のプレイヤーの位置を探す関数
    /// </summary>
    /// <returns>プレイヤーの位置</returns>
    Vector2Int GetPlayerIndex()
    {

        // プレイヤーの位置を探す
        for(int y = 0; y<field.GetLength(0); y++)
        {
            for(int x = 0; x<field.GetLength(1); x++)
            {

                if (field[y,x] == null)
                {
                    // 処理を続行する
                    continue;
                }
                if (field[y, x].tag == "Player")
                {
                    // プレイヤーの座標を返す
                    return new Vector2Int(x, y);
                }

            }
        }
        // 見つからなかった場合-1を返す
        return new Vector2Int(-1, -1);
    }

    /// <summary>
    /// 引数で指定した数字を移動させる関数
    /// </summary>
    /// <param name="number">移動させる数字</param>
    /// <param name="moveFrom">移動させる前の座標</param>
    /// <param name="moveTo">移動させた後の座標</param>
    /// <returns>移動できるのか</returns>
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        // 移動先が配列の範囲外なら移動できない
        if(moveTo.x < 0 || field.GetLength(1) <= moveTo.x) {
            // 移動できない場合にはfalseを返す
            return false;
        }
        if (moveTo.y < 0 || field.GetLength(0) <= moveTo.y)
        {
            // 移動できない場合にはfalseを返す
            return false;
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if(!success) { return false; }
        }

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;

    }


    /// <summary>
    /// クリア判定関数
    /// </summary>
    /// <returns>クリアしているかどうか</returns>
    bool IsCleared()
    {
        // Vector2Int型の可変配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        // ゴールのマスを探す
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {

                // 格納場所かを判断する
                if (map[y, x] == 3)
                {
                    // 格納場所のインデックスを控える
                    goals.Add(new Vector2Int(x, y));
                }

            }
        }

        // 全てのゴールと箱が重なっているかを見る
        for(int i = 0; i < goals.Count; i++)
        {
            // fieldを格納
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                // 条件に一致している者がなければゲームを続ける
                return false;
            }

        }

        // 条件達成ならゲームクリア
        return true;

    }

    /// <summary>
    /// コンソールにマップの情報を出力する関数
    /// </summary>
    void OutputMapLog()
    {
        for(int y = 0; y<map.GetLength(0); y++)
        {
            for(int x = 0; x<map.GetLength(1); x++)
            {
                debugText += map[y,x].ToString() + ",";
            }

            // 改行
            debugText += "\n";

            // 結合した文字列の描画
            Debug.Log(debugText);

        }

        // ログの初期化
        debugText = "";

    }

}
