using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreasurePositionsInfo
{
    public string name;
    public Vector3 position;
    public Vector3 eulerAngles;
}

public class ModeratorOfTreasures : MonoBehaviour {

    private List<GameObject> treasureCandidates; // 全てのお宝候補 : 12
    private List<GameObject> treasures;          // 実際に迷路上に置くお宝たち : 7

    private List<GameObject> treasurePositions;  // お宝を迷路に置く位置 : 7

    public List<GameObject> InitializeAndGetTreasures()
    {
        // 全てのお宝候補を取得
        treasureCandidates = GameObject.FindGameObjectsWithTag("Treasures").ToList<GameObject>();

        if (treasureCandidates.Count == 0)
        {
            throw new Exception("Count of TreasureCandidates is zero.");
        }

        // お宝を迷路に置く位置
        treasurePositions = GameObject.FindGameObjectsWithTag("TreasurePosition").ToList<GameObject>();

        return treasureCandidates;
    }

    public void DeactivateTreasuresPositions()
    {
        foreach (GameObject treasurePosition in treasurePositions)
        {
            treasurePosition.SetActive(false);
        }
    }

    public void DeactivateTreasuresCandidates(List<GameObject> objects)
    {
        foreach (GameObject treasure in treasureCandidates)
        {
            if (!objects.Contains(treasure)) treasure.SetActive(false);
        }
    }

    public Dictionary<TreasurePositionsInfo, GameObject> CreateTreasuresPositionMap()
    {
        // お宝を置く位置を見えないようにする
        DeactivateTreasuresPositions();

        // 迷路上に置くお宝を決める
        treasures = new List<GameObject>();

        List<int> randomList = new List<int>();
        while (true)
        {
            int value = UnityEngine.Random.Range(0, treasureCandidates.Count);
            if (randomList.Contains(value)) continue;
            randomList.Add(value);
            treasures.Add(treasureCandidates[value]);
            if (randomList.Count == treasurePositions.Count) break;
        }

        // 配置しないお宝を見えないようにする
        DeactivateTreasuresCandidates(treasures);

        Dictionary<TreasurePositionsInfo, GameObject> treasureCandidatesMap = new Dictionary<TreasurePositionsInfo, GameObject>();
        for (int i = 0; i < treasures.Count; i++)
        {
            TreasurePositionsInfo treasurePositionInfo = new TreasurePositionsInfo();

            treasurePositionInfo.name = treasures[i].name;
            treasurePositionInfo.position = treasurePositions[i].transform.position;
            treasurePositionInfo.eulerAngles = treasurePositions[i].transform.eulerAngles;

            treasureCandidatesMap.Add(treasurePositionInfo, treasures[i]);
        }

        return treasureCandidatesMap;
    }
}
