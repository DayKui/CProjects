using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class home_scene : MonoBehaviour
{
    public Text unick;
    public Text uchip_lable;
    public Text diamond_lable;

    public Image header = null;
    public Sprite[] uface_img = null;

    public GameObject uinfo_dlg_prefab;
    public GameObject login_bonues_prefab;
    public GameObject rank_list_prefab;
    public GameObject email_list_prefab;
    public GameObject team_match_prefab;

    public Text ulevel_label;
    public Text express_label;
    public Image express_process;

    public GameObject home_page;
    public GameObject war_page;
    public GameObject loading_page;

    public Sprite[] normal_sprites;
    public Sprite[] hightlight_sprites;
    public Image[] tab_buttoms;
    // Use this for initialization
    void Start()
    {
        event_manager.Instance.add_event_listener("sync_uinfo", this.sync_uinfo);
        event_manager.Instance.add_event_listener("sync_ugame_info", this.sync_ugame_info);
        event_manager.Instance.add_event_listener("login_out", this.on_user_login_out);
        event_manager.Instance.add_event_listener("game_start", this.on_game_start);
        this.on_home_page_click(); // 默认是显示主页的;

        this.sync_uinfo("sync_uinfo", null);
        this.sync_ugame_info("sync_ugame_info", null);
    }

    void on_game_start(string name, object udata)
    {
        Debug.Log("GO GO GO GO !!!!!!!!"); // 游戏场景
        this.loading_page.SetActive(true); // 现实除加载进度页面;
    }

    void on_user_login_out(string name, object udata)
    {
        SceneManager.LoadScene("login");
    }
    // 负责同步我们的主信息;
    void sync_uinfo(string name, object udata)
    {
        if (this.unick != null)
        {
            this.unick.text = ugame.Instance.unick;
        }

        if (this.header != null)
        {
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
            GameObject login_bonues = GameObject.Instantiate(this.login_bonues_prefab);
            login_bonues.SetActive(true);
            login_bonues.GetComponent<login_bonues>().show_login_bonues(ugame.Instance.ugame_info.days);
            login_bonues.transform.SetParent(this.transform, false);
        }
        // 
    }

    public void on_home_page_click()
    {
        this.home_page.SetActive(true);
        this.war_page.SetActive(false);

        this.tab_buttoms[0].sprite = this.hightlight_sprites[0];
        this.tab_buttoms[1].sprite = this.normal_sprites[1];
    }

    public void on_war_page_click()
    {
        this.home_page.SetActive(false);
        this.war_page.SetActive(true);

        this.tab_buttoms[0].sprite = this.normal_sprites[0];
        this.tab_buttoms[1].sprite = this.hightlight_sprites[1];
    }

    public void on_login_bonues_click()
    {
        GameObject login_bonues = GameObject.Instantiate(this.login_bonues_prefab);
        login_bonues.SetActive(true);
        login_bonues.GetComponent<login_bonues>().show_login_bonues(ugame.Instance.ugame_info.days);
        login_bonues.transform.SetParent(this.transform, false);
    }

    public void on_uinfo_click()
    {
        GameObject uinfo_dlg = GameObject.Instantiate(this.uinfo_dlg_prefab);
        uinfo_dlg.transform.SetParent(this.transform, false);
    }

    public void on_get_rank_click()
    {
        GameObject rank_list = GameObject.Instantiate(this.rank_list_prefab);
        rank_list.transform.SetParent(this.transform, false);
    }

    public void on_zone_sdyg_click()
    {
        GameObject match_dlg = GameObject.Instantiate(this.team_match_prefab);
        match_dlg.transform.SetParent(this.transform, false);
        ugame.Instance.zid = Zone.SGYD;
    }

    public void on_get_sys_msg_click()
    {
        GameObject sys_email = GameObject.Instantiate(this.email_list_prefab);
        sys_email.transform.SetParent(this.transform, false);
    }

    void OnDestroy()
    {
        event_manager.Instance.remove_event_listener("sync_uinfo", this.sync_uinfo);
        event_manager.Instance.remove_event_listener("login_out", this.on_user_login_out);
        event_manager.Instance.remove_event_listener("game_start", this.on_game_start);
        event_manager.Instance.remove_event_listener("sync_ugame_info", this.sync_ugame_info);
    }
}
