using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadMov : MonoBehaviour
{
    
    [SerializeField]
    private int direcao = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
    }

    public void Direita()
    {
        direcao = 2;

    }

    public void Esquerda()
    {
       direcao = -2;
    }

    public void Parado()
    {
       direcao = 0;

    }

    void Mover()
    {
        transform.Translate(direcao * Time.deltaTime,0,0);
    }
}
