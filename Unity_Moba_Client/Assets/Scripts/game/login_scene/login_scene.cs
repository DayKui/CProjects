using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login_scene : MonoBehaviour
{
    public InputField uname_input;
    public InputField upwd_input;

    void on_get_ugame_info_success(string name, object udata)
    {
        SceneManager.LoadScene("home_scene");
    }

    void on_login_success(string name, object udata)
    {
        //SceneManager.LoadScene("home_scene");
        Debug.Log("load game data...");
        system_service_proxy.Instance.load_user_ugame_info();
    }

    // Use this for initialization
    void Start()
    {
        event_manager.Instance.add_event_listener("get_ugame_info_success", this.on_get_ugame_info_success);
        event_manager.Instance.add_event_listener("login_success", this.on_login_success);
    }

    void OnDestroy()
    {
        event_manager.Instance.remove_event_listener("get_ugame_info_success", this.on_get_ugame_info_success);
        event_manager.Instance.remove_event_listener("login_success", this.on_login_success);
    }

    public void on_guest_login_click()
    {
        auth_service_proxy.Instance.guest_login();
    }

    public void on_uname_login_click()
    {
        if (this.uname_input.text.Length <= 0 ||
            this.upwd_input.text.Length <= 0)
        {
            return;
        }
        auth_service_proxy.Instance.uname_login(this.uname_input.text, this.upwd_input.text);
    }
}
