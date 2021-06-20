using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class home_scene : MonoBehaviour {
    public Text unick;
    public Text uchip_lable;
    public Text diamond_lable;

    public Image header = null;
    public Sprite[] uface_img = null;

    public GameObject uinfo_dlg_prefab;
    public GameObject login_bonues;

    public Text ulevel_label;
    public Text express_label;
    public Image express_process;
    // Use this for initialization
    void Start () {
        event_manager.Instance.add_event_listener("sync_uinfo", this.sync_uinfo);
        event_manager.Instance.add_event_listener("sync_ugame_info", this.sync_ugame_info);
        event_manager.Instance.add_event_listener("login_out", this.on_user_login_out);
        this.sync_uinfo("sync_uinfo", null);
        this.sync_ugame_info("sync_ugame_info", null);
    }

    void on_user_login_out(string name, object udata)
    {
        SceneManager.LoadScene("login");
    }

    // 负责同步我们的主信息;
    void sync_uinfo(string name, object udata) {
        if (this.unick != null) {
            this.unick.text = ugame.Instance.unick;
        }

        if (this.header != null) { 
            this.header.sprite = this.uface_img[ugame.Instance.uface - 1];
        }
    }

    // 负责同步我们的游戏信息
    void sync_ugame_info(string name, object udata)
    {
        if (this.uchip_lable != null)
        {
            this.uchip_lable.text = "" + ugame.Instance.ugame_info.uchip;
        }

        if (this.diamond_lable != null)
        {
            this.diamond_lable.text = "" + ugame.Instance.ugame_info.uchip2;
        }

        // 计算我们的等级信息，并显示出来;
        int now_exp, next_level_exp;
        int level = ulevel.Instance.get_level_info(ugame.Instance.ugame_info.uexp, out now_exp, out next_level_exp);
        if (this.ulevel_label != null)
        {
            this.ulevel_label.text = "LV\n" + level;
        }

        if (this.express_label != null)
        {
            this.express_label.text = now_exp + " / " + next_level_exp;
        }

        if (this.express_process != null)
        {
            this.express_process.fillAmount = (float)now_exp / (float)next_level_exp;
        }
        // end

        // 同步登陆奖励信息
        if (ugame.Instance.ugame_info.bonues_status == 0)
        { // 有登陆奖励可以领取
            login_bonues.SetActive(true);
            login_bonues.GetComponent<login_bonues>().show_login_bonues(ugame.Instance.ugame_info.days);
        }
        else
        {  // 没有登陆奖励可以领取;
            login_bonues.SetActive(false);
        }
        // 
    }

    public void on_uinfo_click() {
        GameObject uinfo_dlg = GameObject.Instantiate(this.uinfo_dlg_prefab);
        uinfo_dlg.transform.SetParent(this.transform, false);
    }
    void OnDestroy() {
        event_manager.Instance.remove_event_listener("sync_uinfo", this.sync_uinfo);
        event_manager.Instance.remove_event_listener("login_out", this.on_user_login_out);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
