using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class user_info_dlg : MonoBehaviour
{
    public InputField unick_edit;
    public GameObject guest_upgrade = null;
    public Image avator_img = null;
    public Sprite[] avator_sprites = null;
    public GameObject womale_check = null;
    public GameObject male_check = null;

    public GameObject face_dlg = null;

    public GameObject account_ugrade_dlg = null;
    public InputField uname_edit;
    public InputField upwd_edit;
    public InputField upwd_again;

    private int usex = 0;
    private int uface = 1;

    // Use this for initialization
    void Start()
    {
        // 是否显示账号升级的按钮，如果你是游客，那么就要显示;
        if (ugame.Instance.is_guest)
        {
            this.guest_upgrade.SetActive(true);
        }
        else
        {
            this.guest_upgrade.SetActive(false);
        }
        // end

        // unick
        if (ugame.Instance.unick != null)
        {
            this.unick_edit.text = ugame.Instance.unick;
        }

        // uface
        if (ugame.Instance.uface >= 1 && ugame.Instance.uface <= 9)
        {
            this.uface = ugame.Instance.uface;
        }
        this.avator_img.sprite = this.avator_sprites[this.uface - 1];

        // usex
        if (ugame.Instance.usex == 0 || ugame.Instance.usex == 1)
        {
            this.usex = ugame.Instance.usex;
        }
        if (this.usex == 0)
        { // man
            this.male_check.SetActive(true);
            this.womale_check.SetActive(false);
        }
        else
        {
            this.male_check.SetActive(false);
            this.womale_check.SetActive(true);
        }

        // 监听我们的事件
        event_manager.Instance.add_event_listener("upgrade_account_return", this.upgrade_account_return);
        // end 
    }

    void OnDestroy()
    {
        event_manager.Instance.remove_event_listener("upgrade_account_return", this.upgrade_account_return);
    }

    void upgrade_account_return(string name, object udata)
    {
        int status = (int)udata;
        Debug.Log("upgrade account status: " + status);

        if (status == Respones.OK)
        {
            this.on_hide_account_upgrade();
            this.guest_upgrade.SetActive(false);
        }
    }

    public void on_show_account_upgrade()
    {
        this.account_ugrade_dlg.SetActive(true);
    }

    public void on_hide_account_upgrade()
    {
        this.account_ugrade_dlg.SetActive(false);
    }

    public void on_do_account_upgrade()
    {
        if (!ugame.Instance.is_guest)
        {
            return;
        }

        if (this.uname_edit.text.Length <= 0 || this.upwd_edit.text.Length <= 0 ||
            !this.upwd_edit.text.Equals(this.upwd_again.text))
        {
            return;
        }

        string md5_pwd = utils.md5(this.upwd_edit.text);
        auth_service_proxy.Instance.do_account_upgrade(this.uname_edit.text, md5_pwd);
    }

    public void on_sex_change(int usex)
    {
        this.usex = usex;
        if (this.usex == 0)
        { // man
            this.male_check.SetActive(true);
            this.womale_check.SetActive(false);
        }
        else
        {
            this.male_check.SetActive(false);
            this.womale_check.SetActive(true);
        }
    }

    public void on_avator_click()
    {
        this.face_dlg.SetActive(true);
    }

    public void on_uface_select_close()
    {
        this.face_dlg.SetActive(false);
    }

    public void on_uface_select_click(int uface)
    {
        this.uface = uface;
        this.avator_img.sprite = this.avator_sprites[this.uface - 1];
        this.face_dlg.SetActive(false);
    }

    public void on_close_uinfo_dlg_click()
    {
        // this.gameObject.SetActive(false);
        GameObject.Destroy(this.gameObject);
    }

    // 用户注销
    public void on_user_login_out_click()
    {
        auth_service_proxy.Instance.user_login_out();
        GameObject.Destroy(this.gameObject);
    }

    public void on_editprofile_commit()
    {
        Debug.Log("on_editprofile_commit called" + this.unick_edit.text);
        // unick, usex, uface
        if (this.unick_edit.text.Length <= 0)
        {
            this.on_close_uinfo_dlg_click();
            return;
        }

        Debug.Log(this.unick_edit.text + " " + this.uface + " " + this.usex);
        // 提交修改资料请求到服务器
        auth_service_proxy.Instance.edit_profile(this.unick_edit.text, this.uface, this.usex);
        // end 

        this.on_close_uinfo_dlg_click();
    }
}
