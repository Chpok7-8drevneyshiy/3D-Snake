using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    public bool Yellow, Blue = false;
    public snake1 BlueSnake;
    public snake2 YellowSnake;
    // Start is called before the first frame update
    void Start()
    {
        BlueSnake = GetComponent<snake1>();
        YellowSnake = GetComponent<snake2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
