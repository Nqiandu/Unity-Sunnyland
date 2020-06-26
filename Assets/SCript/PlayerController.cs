using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public AudioSource jumpAudio,hurtAudio,cherryAudio;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public static int Cherry = 0;
    public Text CherryNum;
    private bool isHurt;
    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt) {
            Movement();
        }
        
        SwitchAnim();
    }

    void Movement(){
        //float horizontalmove = Input.GetAxis("Horizontal");//横向输入,键盘
        float horizontalmove = joystick.Horizontal;//触控

        //float facedircetion =  Input.GetAxisRaw("Horizontal");
        //角色移动
        //if(horizontalmove != 0){
         //   rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime,rb.velocity.y);
         //   anim.SetFloat("running",Mathf.Abs(facedircetion));
        //}
        //if(facedircetion != 0){
        //    transform.localScale = new Vector3(facedircetion, 1, 1);
        //}
        //角色跳跃
        //if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)){
            //rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
           // jumpAudio.Play();
         //   anim.SetBool("jumping", true);
        //}

        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(horizontalmove));
        }
        if (horizontalmove > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontalmove < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //角色跳跃
        //if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
        //    jumpAudio.Play();
        //    anim.SetBool("jumping", true);
        //}

        if (joystick.Vertical>0.5f && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            jumpAudio.Play();
            anim.SetBool("jumping", true);
        }
    }

    //切换动画效果
    void SwitchAnim(){

        anim.SetBool("idle",false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground)) {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping")) {
            if (rb.velocity.y < 0) {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        } else if (isHurt) {
            anim.SetBool("Hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f) {
                anim.SetBool("Hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }

        } else if (coll.IsTouchingLayers(ground)) {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    //碰撞触发器
    //收集物品
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Collection"){
            cherryAudio.Play();
            Destroy(collision.gameObject);
            Cherry ++;
            CherryNum.text = Cherry.ToString();
        }
        if (collision.tag == "DeadLine") {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    //消灭敌人
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "青蛙") {

            //Enemy_frog frog = collision.gameObject.GetComponent<Enemy_frog>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                //Destroy(collision.gameObject);//
                //frog.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x) {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            }
        }
        
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
