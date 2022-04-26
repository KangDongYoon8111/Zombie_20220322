using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ����޽� ���� �ڵ�

// �ֱ������� �������� �÷��̾� ��ó�� �����ϴ� ��ũ��Ʈ
public class ItemSpanwer : MonoBehaviour
{
    public GameObject[] items; // ������ ������
    public Transform playerTransform; // �÷��̾��� Ʈ������

    // �÷��̾� ��ġ���� �������� ��ġ�� �ִ� �ݰ�
    public float maxDistance = 5f;

    public float timeBetSpawnMax = 7f; // �ִ� �ð� ����
    public float timeBetSpawnMin = 2f; // �ּ� �ð� ����
    private float timeBetSpawn; // ���� ����

    private float lastSpawnTime; // ������ ���� ����

    void Start()
    {
        // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    // �ֱ������� ������ ���� ó�� ����
    void Update()
    {
        // ���� ������ ������ ���� �������� ���� �ֱ� �̻� ����
        // && �÷��̾� ĳ���Ͱ� ������
        if (Time.time >= lastSpawnTime + timeBetSpawn 
            && playerTransform != null)
        {
            // ������ ���� �ð� ����
            lastSpawnTime = Time.time;
            // ���� �ֱ⸦ �������� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // ������ ���� ����
            Spawn();
        }
    }

    // ���� ������ ���� ó��
    private void Spawn()
    {
        // �÷��̾� ��ó���� ����޽� ���� ���� ��ġ ��������
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        // �ٴڿ��� 0.5��ŭ ���� �ø���
        spawnPosition += Vector3.up * 0.5f;

        // ������ �� �ϳ��� �������� ��� ���� ��ġ�� ����
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, 
            Quaternion.identity);

        // ������ �������� 5�� �ڿ� �ı�
        Destroy(item, 5f);
    }

    // ����޽� ���� ������ ��ġ�� ��ȯ�ϴ� �޼���
    // center�� �߽����� distance �ݰ� �ȿ����� ������ ��ġ�� ã��
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, 
        float distance)
    {
        // Center�� �߽����� �������� maxDistance�� �� �ȿ�����
        // ������ ��ġ �ϳ��� ����
        // Random.insideUnitSphere�� �������� 1�� �� �ȿ����� ������
        // �� ���� ��ȯ�ϴ� ������Ƽ
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ������ �����ϴ� ����
        NavMeshHit hit;

        // maxDistance �ݰ� �ȿ��� randomPos�� ���� ����� ����޽�
        // ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // ã�� �� ��ȯ
        return hit.position;
    }
}