using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviour
{
    public Zombie zombiePrefab; // 생성할 좀비 원본 프리팹

    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치

    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비를 담는 리스트
    private int wave; // 현재 웨이브

    // Update is called once per frame
    void Update()
    {
        // 게임오버 상태일 때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if(zombies.Count <= 0)
        {
            SpawnWave();
        }
        UpdateUI(); // UI 갱신
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 좀비 수 표시
        UIManager.instance.UpdateWaveText(wave, zombies.Count);
    }

    // 현재 웨이브에 맞춰 좀비 생성
    private void SpawnWave()
    {

    }

    // 좀비를 생성하고 좀비에 추적할 대상 할당
    private void CreateZombie()
    {

    }
}
