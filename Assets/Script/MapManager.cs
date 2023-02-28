using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int minWallCount = 4;
    private int maxWallCount = 15;


    public int rows = 10;
    public int cols = 10;
    public GameObject[] outwall; // ��Χǽ
    public GameObject[] ground; // ����
    public GameObject[] wall; // �ϰ�������ϵ�ǽ
    public GameObject[] food;
    public GameObject[] suda;
    public List<Vector3> posList = new List<Vector3>(); // �����ϵ�����λ����Ϣ
    public Vector3 exitPos;
    public Vector3 playerStartPos = new Vector3(1, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        initMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initMap()
    {
        // ��ʼ���˳���λ��
        exitPos.x = rows - 2;
        exitPos.y = cols - 2;
        exitPos.z = 0;
        
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                    initOutwall(new Vector3(i, j, 0)); // ������Χǽ: i = 0 �� rows-1
                else
                {
                    initGround(new Vector3(i, j, 0)); // ��������: ����Χǽ�����λ��
                    if (i != exitPos.x && j != exitPos.y && i != playerStartPos.x && j != playerStartPos.y) posList.Add(new Vector3(i, j, 0)); // ������λ�ô洢����
                }


        // �����ϰ��ʳ��

        // 1. ��ǰ�ȼ��²������ϰ�������
        GameManager gameManager =  GetComponent<GameManager>();
        int wallCount = gameManager.level * 2 + 1; // ������ȼ��Ĺ�ϵ
        wallCount = wallCount <= minWallCount ? minWallCount : wallCount; // ������С����
        wallCount = wallCount >= maxWallCount ? maxWallCount : wallCount; // �����������
        
        int randomCount = Random.Range(minWallCount, maxWallCount);
        initItems(wall, randomCount);

        // ��������
    }

    public void initOutwall(Vector3 pos)
    {
        int wallIndex = Random.Range(0, outwall.Length);
        GameObject.Instantiate(outwall[wallIndex], pos, Quaternion.identity);
    }

    public void initGround(Vector3 pos)
    {
        int groundIndex = Random.Range(0, ground.Length);
        GameObject.Instantiate(ground[groundIndex], pos, Quaternion.identity);
    }

    // ��ʼ���ϰ��ʳ��
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
