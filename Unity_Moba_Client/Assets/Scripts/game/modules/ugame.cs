using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;

public class ugame : Singletom<ugame> {
    public string unick = "";
    public int uface = 1;
    public int usex = 0;
    public int uvip = 0;
    public bool is_guest = false;

    public void save_uinfo(UserCenterInfo uinfo, bool is_guest) {
        this.unick = uinfo.unick;
        this.uface = uinfo.uface;
        this.usex = uinfo.usex;
        this.uvip = uinfo.uvip;
        this.is_guest = is_guest;
    }

    public void save_edit_profile(string unick, int uface, int usex)
    {
        this.unick = unick;
        this.uface = uface;
        this.usex = usex;
    }
}
