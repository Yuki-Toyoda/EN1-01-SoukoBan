using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    // �v���C���[�̃Q�[���I�u�W�F�N�g�ǂݍ���
    public GameObject playerPrefab;
    public GameObject boxPreafab;

	// �z��̗v�f
	int[,] map; // ���x���f�U�C���p
    GameObject[,] field; // �Q�[���Ǘ��p�̔z��

	// �v���C���[�ʒu
	Vector2Int playerIndex;

    // ������̐錾�Ə�����
    string debugText = "";

    // Start is called before the first frame update
    void Start()
	{

        // �z��̎��̂̍쐬�Ə�����
        map = new int[,] {
        {0, 0, 0, 0, 0 },
        {0, 2, 1, 2, 0 },
        {0, 0, 0, 0, 0 }
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
                    // �v���C���[�̃C���X�^���X���쐬
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                else if (map[y, x ] == 2)
                {
                    // �{�b�N�X�̃C���X�^���X�̍쐬
                    field[y, x] = Instantiate(
                        boxPreafab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
            }
        }

        // ���O�Ƀ}�b�v�����o��
        //OutputMapLog();

    }

	// Update is called once per frame
	void Update()
	{

        // �v���C���[�̍��W�擾
        playerIndex = GetPlayerIndex();

        // �v���C���[�̈ړ�����
        // �㉺
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveNumber(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveNumber(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));

        }

        // ���E
        if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            MoveNumber(playerPrefab.tag, playerIndex,new Vector2Int(playerIndex.x + 1, playerIndex.y));
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
            MoveNumber(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
        }
	}

    /// <summary>
    /// �z����̃v���C���[�̈ʒu��T���֐�
    /// </summary>
    /// <returns>�v���C���[�̈ʒu</returns>
    Vector2Int GetPlayerIndex()
    {

        // �v���C���[�̈ʒu��T��
        for(int y = 0; y<field.GetLength(0); y++)
        {
            for(int x = 0; x<field.GetLength(1); x++)
            {

                if (field[y,x] == null)
                {
                    // �����𑱍s����
                    continue;
                }
                if (field[y, x].tag == "Player")
                {
                    // �v���C���[�̍��W��Ԃ�
                    return new Vector2Int(x, y);
                }

            }
        }
        // ������Ȃ������ꍇ-1��Ԃ�
        return new Vector2Int(-1, -1);
    }

    /// <summary>
    /// �����Ŏw�肵���������ړ�������֐�
    /// </summary>
    /// <param name="number">�ړ������鐔��</param>
    /// <param name="moveFrom">�ړ�������O�̍��W</param>
    /// <param name="moveTo">�ړ���������̍��W</param>
    /// <returns>�ړ��ł���̂�</returns>
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        // �ړ��悪�z��͈̔͊O�Ȃ�ړ��ł��Ȃ�
        if(moveTo.x < 0 || field.GetLength(1) <= moveTo.x) {
            // �ړ��ł��Ȃ��ꍇ�ɂ�false��Ԃ�
            return false;
        }
        if (moveTo.y < 0 || field.GetLength(0) <= moveTo.y)
        {
            // �ړ��ł��Ȃ��ꍇ�ɂ�false��Ԃ�
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
    /// �R���\�[���Ƀ}�b�v�̏����o�͂���֐�
    /// </summary>
    void OutputMapLog()
    {
        for(int y = 0; y<map.GetLength(0); y++)
        {
            for(int x = 0; x<map.GetLength(1); x++)
            {
                debugText += map[y,x].ToString() + ",";
            }

            // ���s
            debugText += "\n";

            // ��������������̕`��
            Debug.Log(debugText);

        }

        // ���O�̏�����
        debugText = "";

    }

}
