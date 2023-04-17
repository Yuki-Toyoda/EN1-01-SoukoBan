using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

	// �z��̗v�f
	int[] map;
	// �v���C���[�ʒu
	int playerIndex;

    // ������̐錾�Ə�����
    string debugText = "";

    // Start is called before the first frame update
    void Start()
	{
		// �z��̎��̂̍쐬�Ə�����
		map = new int[] { 0, 2, 0, 1, 0, 2, 2, 0, 0 };

        OutputMapLog();

    }

	// Update is called once per frame
	void Update()
	{

        // �v���C���[�̍��W�擾
        playerIndex = GetPlayerIndex();

        // �v���C���[�̈ړ�����
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
    /// �z����̃v���C���[�̈ʒu��T���֐�
    /// </summary>
    /// <returns>�v���C���[�̈ʒu</returns>
    int GetPlayerIndex()
    {
        // �v���C���[�̈ʒu��T��
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        // ������Ȃ������ꍇ-1��Ԃ�
        return -1;
    }

    /// <summary>
    /// �����Ŏw�肵���������ړ�������֐�
    /// </summary>
    /// <param name="number">�ړ������鐔��</param>
    /// <param name="moveFrom">�ړ�������O�̍��W</param>
    /// <param name="moveTo">�ړ���������̍��W</param>
    /// <returns>�ړ��ł���̂�</returns>
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        // �ړ��悪�z��͈̔͊O�Ȃ�ړ��ł��Ȃ�
        if(moveTo < 0 || map.Length <= moveTo)
        {
            // �ړ��ł��Ȃ��ꍇ�ɂ�false��Ԃ�
            return false;
        }

        if (map[moveTo] == 2)
        {
            // �ړ���̌��o
            int velocity = moveTo - moveFrom;

            // �����ړ��ł��邩�ǂ��������m����
            bool sucess = MoveNumber(2, moveTo, moveTo + velocity);

            // �����ړ��ł��Ȃ���΃v���C���[���ړ����Ȃ�
            if (!sucess) { return false; }
        }

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;

    }

    /// <summary>
    /// �R���\�[���Ƀ}�b�v�̏����o�͂���֐�
    /// </summary>
    void OutputMapLog()
    {
        for (int i = 0; i < map.Length; ++i)
        {
            debugText += map[i].ToString() + ",";

        }

        // ��������������̕`��
        Debug.Log(debugText);

        // ���O�̏�����
        debugText = "";

    }

}
