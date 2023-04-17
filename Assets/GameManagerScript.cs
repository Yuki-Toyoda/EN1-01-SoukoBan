using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

	// 配列の要素
	int[] map;
	// プレイヤー位置
	int playerIndex;

    // 文字列の宣言と初期化
    string debugText = "";

    // Start is called before the first frame update
    void Start()
	{
		// 配列の実体の作成と初期化
		map = new int[] { 0, 2, 0, 1, 0, 2, 2, 0, 0 };

        OutputMapLog();

    }

	// Update is called once per frame
	void Update()
	{

        // プレイヤーの座標取得
        playerIndex = GetPlayerIndex();

        // プレイヤーの移動処理
        if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            MoveNumber(1, playerIndex, playerIndex + 1);
            OutputMapLog();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
            MoveNumber(1, playerIndex, playerIndex - 1);
            OutputMapLog();
        }
	}

    /// <summary>
    /// 配列内のプレイヤーの位置を探す関数
    /// </summary>
    /// <returns>プレイヤーの位置</returns>
    int GetPlayerIndex()
    {
        // プレイヤーの位置を探す
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        // 見つからなかった場合-1を返す
        return -1;
    }

    /// <summary>
    /// 引数で指定した数字を移動させる関数
    /// </summary>
    /// <param name="number">移動させる数字</param>
    /// <param name="moveFrom">移動させる前の座標</param>
    /// <param name="moveTo">移動させた後の座標</param>
    /// <returns>移動できるのか</returns>
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        // 移動先が配列の範囲外なら移動できない
        if(moveTo < 0 || map.Length <= moveTo)
        {
            // 移動できない場合にはfalseを返す
            return false;
        }

        if (map[moveTo] == 2)
        {
            // 移動先の検出
            int velocity = moveTo - moveFrom;

            // 箱が移動できるかどうかを検知する
            bool sucess = MoveNumber(2, moveTo, moveTo + velocity);

            // 箱が移動できなければプレイヤーも移動しない
            if (!sucess) { return false; }
        }

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;

    }

    /// <summary>
    /// コンソールにマップの情報を出力する関数
    /// </summary>
    void OutputMapLog()
    {
        for (int i = 0; i < map.Length; ++i)
        {
            debugText += map[i].ToString() + ",";

        }

        // 結合した文字列の描画
        Debug.Log(debugText);

        // ログの初期化
        debugText = "";

    }

}
