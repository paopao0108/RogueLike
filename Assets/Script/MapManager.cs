using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int minWallCount = 4;
    private int maxWallCount = 15;
    private int minFoodCount = 1;
    private int maxFoodCount = 5;

    public int rows = 10;
    public int cols = 10;
    public GameObject[] outwall; // ��Χǽ
    public GameObject[] ground; // ����
    public GameObject[] wall; // �ϰ�������ϵ�ǽ
    public GameObject[] food;
    public GameObject[] suda;
    public GameObject[] enemy;
    public GameObject[] exit; // ���򻯣������������趨��Ϊһ������
    public GameObject player;
    public List<Vector3> posList = new List<Vector3>(); // �����ϵ�����λ����Ϣ
    public Vector3 exitPos;
    public Vector3 playerStartPos = new Vector3(1, 1, 0);

    void Awake()
    {
        initMap();
    }
    public void initMap()
    {
        // ��ʼ���˳���λ��
        exitPos.x = rows - 2;
        exitPos.y = cols - 2;
        exitPos.z = 0;
        // �����˳�
        InitBackground(exitPos, exit); // ����

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                    InitBackground(new Vector3(i, j, 0), outwall); // ������Χǽ: i = 0 �� rows-1
                else
                {
                    InitBackground(new Vector3(i, j, 0), ground); // ��������: ����Χǽ�����λ��
                    if (i != exitPos.x && j != exitPos.y && i != playerStartPos.x && j != playerStartPos.y) posList.Add(new Vector3(i, j, 0)); // ������λ�ô洢����
                }


        // �����ϰ��ʳ��
        // 1. ��ǰ�ȼ��²������ϰ�������
        GameManager gameManager =  GetComponent<GameManager>();
        int wallCount = gameManager.level * 2 + 1; // ������ȼ��Ĺ�ϵ
        wallCount = wallCount <= minWallCount ? minWallCount : wallCount; // ������С����
        wallCount = wallCount >= maxWallCount ? maxWallCount : wallCount; // �����������
        // 2. �������������ϰ���
        initItems(wall, wallCount);
        initItems(food, 3);
        initItems(suda, 3);

        // ��������
        int enemyCount = gameManager.level * 2 - 2;
        initItems(enemy, enemyCount);


        // �������
        GameObject.Instantiate(player, new Vector3(1, 1, 0), Quaternion.identity);
    }

    public void InitBackground(Vector3 pos, GameObject[] gameObj) {
        int index = Random.Range(0, gameObj.Length);
        GameObject.Instantiate(gameObj[index], pos, Quaternion.identity);
    }

    // ��ȡ��ǰ�ؿ����ϰ����ʳ�������
    public int GetItemsCount(int level, int max, int min, bool isWall)
    {
        int count = isWall ? level * 2 + 1 : level / 2; // ������ȼ��Ĺ�ϵ
        count = count <= min ? min : count; // ������С����
        count = count >= max ? max : count; // �����������
        return count;
    }

    // ��ʼ���ϰ��ʳ�����
    public void initItems(GameObject[] items, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int itemIndex = Random.Range(0, items.Length); // �Ӷ����б��������ȡһ������
            int randomPosIndex = Random.Range(0, posList.Count); // ���б��������ȡһ��λ��
            Vector3 randomPos = posList.ToArray()[randomPosIndex];
            posList.RemoveAt(randomPosIndex); // ɾ����ʹ�õ�λ��
            GameObject.Instantiate(items[itemIndex], randomPos, Quaternion.identity);
        }
    }

}
