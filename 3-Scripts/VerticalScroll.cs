using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [Tooltip ("Game Unit per second")]
    [SerializeField] float scrollRate;
    [SerializeField] float scrollDown;
    [SerializeField] float timer = 0f;

    [SerializeField] Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       LavaWave();//TO DO: Code doesn't work need to find a solution
       NextWaveTimer();
    }

    private void LavaWave()
    {
        if(timer >= 10f)
        {
            transform.Translate(Vector3.up * scrollRate * Time.deltaTime);
        }

        if(timer >= 40f)
        {
            transform.Translate(Vector3.down * scrollDown * Time.deltaTime);
        }
    }

    private void NextWaveTimer()
    {
        timer += 1 * Time.deltaTime;
    }

    //if currentTime reaches 0
    //Lave goes up + lavaTimer starts
    //if Lava timer reaches 0
    //Lava goes down + currentTime restart
}
