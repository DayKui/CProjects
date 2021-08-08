using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_zygote : MonoBehaviour
{

    public joystick stick;

    public GameObject[] hero_characters = null; // 男, 女;

    public GameObject entry_A; // 
                               // Use this for initialization
    void Start()
    {
        GameObject hero = GameObject.Instantiate(this.hero_characters[ugame.Instance.usex]);
        hero.transform.SetParent(this.transform, false);
        hero.transform.position = this.entry_A.transform.position;
        charactor_ctrl ctrl = hero.AddComponent<charactor_ctrl>();
        ctrl.is_ghost = false; // 自己来控制;
        ctrl.stick = this.stick; // 测试
    }
}
