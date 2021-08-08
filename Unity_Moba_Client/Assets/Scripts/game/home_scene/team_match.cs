using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;

public class team_match : MonoBehaviour
{
    public ScrollRect scrollview;
    public GameObject opt_prefab;
    public Sprite[] uface_img = null;

    private int member_count;
    // Use this for initialization
    void Start()
    {
        this.member_count = 0;
        event_manager.Instance.add_event_listener("user_arrived", this.on_user_arrived);
        event_manager.Instance.add_event_listener("exit_match", this.on_self_exit_match);
        event_manager.Instance.add_event_listener("other_user_exit", this.on_other_user_exit_match);
        event_manager.Instance.add_event_listener("game_start", this.on_game_start);
    }


    void on_game_start(string event_name, object udata)
    {
        GameObject.Destroy(this.gameObject);
    }

    void on_other_user_exit_match(string event_name, object udata)
    {
        int index = (int)udata;
        this.member_count--;

        GameObject.Destroy(this.scrollview.content.GetChild(index).gameObject);
        this.scrollview.content.sizeDelta = new Vector2(0, this.member_count * 106);
    }

    void on_self_exit_match(string event_name, object udata)
    {
        ugame.Instance.zid = -1;
        GameObject.Destroy(this.gameObject);
    }


    void on_user_arrived(string event_name, object udata)
    {
        UserArrived user_info = (UserArrived)udata;
        this.member_count++;

        GameObject user = GameObject.Instantiate(this.opt_prefab);
        user.transform.SetParent(this.scrollview.content.transform, false);
       // this.scrollview.content.sizeDelta = new Vector2(0, this.member_count * 106);

        user.transform.Find("name").GetComponent<Text>().text = user_info.unick;
        user.transform.Find("header/avator").GetComponent<Image>().sprite = this.uface_img[user_info.uface - 1];
        user.transform.Find("sex").GetComponent<Text>().text = (user_info.usex == 0) ? "male" : "wmale";
    }
    public void on_begin_match_click()
    {
        int zid = ugame.Instance.zid;
        logic_service_proxy.Instance.enter_zone(zid);
    }

    public void on_exit_match_click()
    {
        logic_service_proxy.Instance.exit_match();
    }

    public void OnDestroy()
    {
        event_manager.Instance.remove_event_listener("user_arrived", this.on_user_arrived);
        event_manager.Instance.remove_event_listener("exit_match", this.on_self_exit_match);
        event_manager.Instance.remove_event_listener("other_user_exit", this.on_other_user_exit_match);
        event_manager.Instance.remove_event_listener("game_start", this.on_game_start);
    }
}
