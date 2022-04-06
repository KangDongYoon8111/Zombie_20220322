using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corutines : MonoBehaviour
{
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CleaningHouse()); // A
        StartCoroutine("CleaningHouse"); // B

        Invoke("Test", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        int time = (int)Time.deltaTime;
        Debug.Log("Time : " + time);
    }

    void Test()
    {
        Debug.Log("Invoke B Room");
    }

    IEnumerator CleaningHouse()
    {
        Debug.Log("A Room");
        // A规 没家
        yield return new WaitForSeconds(10f); // 10檬 悼救 浆扁
        Debug.Log("B Room");
        // B规 没家
        yield return new WaitForSeconds(20f); // 20檬 悼救 浆扁
        // C规 没家
        Debug.Log("C Room");
    }
}
