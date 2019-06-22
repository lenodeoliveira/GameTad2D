using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaMov : MonoBehaviour
{
    public bool face = true;
    public Transform heroiT;
    public float vel = 2.5f;
    public float force = 3.5f;
    public Rigidbody2D heroiRB;
    public bool liberaPulo = false;
    public Animator anim;
    public bool vivo = true;




    // Start is called before the first frame update
    void Start()
    {
       heroiT = GetComponent<Transform> ();
       heroiRB = GetComponent<Rigidbody2D> (); 
       anim = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
         if(vivo == true)  
         { 
         //Virar personagem à esquerda
         if(Input.GetKey(KeyCode.A) && face)
         {
            Virar();

         }
         
         //Virar personagem à direita
         if(Input.GetKey(KeyCode.D) && !face)
         {
              Virar();
         }
         }
         
         if(vivo == true)
         {
          
         //Andar à direita
         if(Input.GetKey(KeyCode.D))
         {
            transform.Translate(new Vector2(vel * Time.deltaTime,0));
            anim.SetBool ("idle", false);
            anim.SetBool ("imagem1", true); 
         }
         //Andar à esquerda 
         else if(Input.GetKey(KeyCode.A))
         { 
            transform.Translate(new Vector2(-vel * Time.deltaTime,0)); 
            anim.SetBool ("idle", false);
            anim.SetBool ("imagem1", true); 
         }
         else
         {
            anim.SetBool ("idle", true);
            anim.SetBool ("imagem1", false);

         }
         
         }
         
         if(vivo == true)
         { 

         //Pulo 
        
         if(Input.GetKeyDown(KeyCode.W) && liberaPulo == true && Input.GetKey(KeyCode.D))
         {
            heroiRB.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
            anim.SetBool ("pular", true);
            anim.SetBool ("imagem1", false);    

         }

         else if(Input.GetKeyDown(KeyCode.W) && liberaPulo == true && Input.GetKey(KeyCode.A))
         {
             heroiRB.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
             anim.SetBool ("pular", true);
             anim.SetBool ("imagem1", false); 
         }

         else if (Input.GetKeyDown(KeyCode.W) && liberaPulo == true)
         { 
             heroiRB.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
             anim.SetBool ("pular", true);
             anim.SetBool ("idle", false);

         }

        }
         
    }

    void Virar()
    {
       face = !face;

       Vector3 scala = heroiT.localScale;
       scala.x *= -1;
       heroiT.localScale = scala;

    }

    void OnCollisionEnter2D(Collision2D outro)
    {
       if(outro.gameObject.CompareTag("chao"))
       {
          liberaPulo = true; 
          anim.SetBool ("pular", false);
          anim.SetBool ("idle", true); 
       }
    }
    
    void OnCollisionExit2D(Collision2D outro)
    {
      if(outro.gameObject.CompareTag("chao"))
      {
      
      liberaPulo = false;
      
      }

    }

    void OnTriggerEnter2D(Collider2D outro)
    {
      if(outro.gameObject.CompareTag("bomba"))
      {
         vivo = false;

         anim.SetBool ("imagem1", false);
         //anim.SetBool ("Morte", true);
         //Destroy (heroiRB.gameObject);

      }

    }

}