using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum charactor_state
{
    walk = 1,
    free = 2,
    idle = 3,
    attack = 4,
    attack2 = 5,
    attack3 = 6,
    skill = 7,
    skill2 = 8,
    death = 9,
}

// 角色控制, 主角 3v3;
// 玩家控制得，其它玩家控制
public class charactor_ctrl : MonoBehaviour
{
    // test
    public joystick stick;
    // end 

    public bool is_ghost = false; // is_ghost: 标记是否为别人控制的 ghost;
    public float speed = 8.0f; // 给我们的角色定义一个速度

    private CharacterController ctrl;
    private Animation anim;
    private charactor_state state = charactor_state.idle;
    private Vector3 camera_offset; // 主角离摄像机的相对距离;
                                   // Use this for initialization
    void Start()
    {
        GameObject ring = Resources.Load<GameObject>("effect/other/guangquan_fanwei");
        this.ctrl = this.GetComponent<CharacterController>();
        this.anim = this.GetComponent<Animation>();

        if (!this.is_ghost)
        {  // 玩家控制角色;
            GameObject r = GameObject.Instantiate(ring);
            r.transform.SetParent(this.transform, false);
            r.transform.localPosition = Vector3.zero;
            r.transform.localScale = new Vector3(2, 1, 2);
            this.camera_offset = Camera.main.transform.position - this.transform.position;
        }

        this.anim.Play("idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.state != charactor_state.idle && this.state != charactor_state.walk)
        {
            return;
        }

        if (this.stick.dir.x == 0 && this.stick.dir.y == 0)
        {
            if (this.state == charactor_state.walk)
            {
                this.anim.CrossFade("idle");
                this.state = charactor_state.idle;
            }
            return;
        }

        if (this.state == charactor_state.idle)
        {
            this.anim.CrossFade("walk");
            this.state = charactor_state.walk;
        }
        float r = Mathf.Atan2(this.stick.dir.y, this.stick.dir.x);

        float s = this.speed * Time.deltaTime;
        float sx = s * Mathf.Cos(r - Mathf.PI * 0.25f);
        float sz = s * Mathf.Sin(r - Mathf.PI * 0.25f);
        this.ctrl.Move(new Vector3(sx, 0, sz));



        float degree = r * 180 / Mathf.PI;
        degree = 360 - degree + 90 + 45;
        this.transform.localEulerAngles = new Vector3(0, degree, 0);

        if (!this.is_ghost)
        {
            Camera.main.transform.position = this.transform.position + this.camera_offset;
        }
    }
}
