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
    public GameObject[] outwall; // 外围墙
    public GameObject[] ground; // 地面
    public GameObject[] wall; // 障碍物：地面上的墙
    public GameObject[] food;
    public GameObject[] suda;
    public GameObject[] enemy;
    public GameObject[] exit; // 待简化：后续可以无需定义为一个数组
    public GameObject player;
    public List<Vector3> posList = new List<Vector3>(); // 地面上的所有位置信息
    public Vector3 exitPos;
    public Vector3 playerStartPos = new Vector3(1, 1, 0);

    void Awake()
    {
        initMap();
    }
    public void initMap()
    {
        // 初始化退出的位置
        exitPos.x = rows - 2;
        exitPos.y = cols - 2;
        exitPos.z = 0;
        // 创建退出
        InitBackground(exitPos, exit); // 待简化

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                    InitBackground(new Vector3(i, j, 0), outwall); // 创建外围墙: i = 0 和 rows-1
                else
                {
                    InitBackground(new Vector3(i, j, 0), ground); // 创建地面: 除外围墙以外的位置
                    if (i != exitPos.x && j != exitPos.y && i != playerStartPos.x && j != playerStartPos.y) posList.Add(new Vector3(i, j, 0)); // 将地面位置存储下来
                }


        // 创建障碍物、食物
        // 1. 当前等级下产生的障碍物数量
        GameManager gameManager =  GetComponent<GameManager>();
        int wallCount = gameManager.level * 2 + 1; // 数量与等级的关系
        wallCount = wallCount <= minWallCount ? minWallCount : wallCount; // 限制最小数量
        wallCount = wallCount >= maxWallCount ? maxWallCount : wallCount; // 限制最大数量
        // 2. 根据数量创建障碍物
        initItems(wall, wallCount);
        initItems(food, 3);
        initItems(suda, 3);

        // 创建敌人
        int enemyCount = gameManager.level * 2 - 2;
        initItems(enemy, enemyCount);


        // 创建玩家
        GameObject.Instantiate(player, new Vector3(1, 1, 0), Quaternion.identity);
    }

    public void InitBackground(Vector3 pos, GameObject[] gameObj) {
        int index = Random.Range(0, gameObj.Length);
        GameObject.Instantiate(gameObj[index], pos, Quaternion.identity);
    }

    // 获取当前关卡的障碍物或食物的数量
    public int GetItemsCount(int level, int max, int min, bool isWall)
    {
        int count = isWall ? level * 2 + 1 : level / 2; // 数量与等级的关系
        count = count <= min ? min : count; // 限制最小数量
        count = count >= max ? max : count; // 限制最大数量
        return count;
    }

    // 初始化障碍物、食物、敌人
    public void initItems(GameObject[] items, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int itemIndex = Random.Range(0, items.Length); // 从对象列表中随机获取一个对象
            int randomPosIndex = Random.Range(0, posList.Count); // 从列表中随机获取一个位置
            Vector3 randomPos = posList.ToArray()[randomPosIndex];
            posList.RemoveAt(randomPosIndex); // 删除已使用的位置
            GameObject.Instantiate(items[itemIndex], randomPos, Quaternion.identity);
        }
    }

}
