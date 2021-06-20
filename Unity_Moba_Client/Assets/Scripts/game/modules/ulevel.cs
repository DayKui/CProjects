using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ulevel : Singletom<ulevel> {
    private int[] level_exp;
    public void init() {
        // test,配置文件里面去读取出来;
        this.level_exp = new int[]{
            0,
            1000,
            2000,
            3000,
            4000,
            5000,
            6000,
            7000,
            8000,
            9000,
            10000,
            20000,
            30000
        };

        // // 2500, 1000(第1级),  1500/2000
        int now_exp;
        int next_level_exp;
        int level = ulevel.Instance.get_level_info(2500, out now_exp, out next_level_exp);
        Debug.Log("level = " + level + "now_exp=" + now_exp + "next_level_exp = " + next_level_exp);
        // end
    }

    public int get_level_info(int uexp, out int now_exp, out int next_level_exp) {
        now_exp = 0;
        next_level_exp = 0;

        int level = 0;
        int last_exp = uexp;

        while (level + 1 <= this.level_exp.Length - 1 && 
            last_exp >= this.level_exp[level + 1])  {
            last_exp -= this.level_exp[level + 1];
            level++;
        }

        // 我们已经到了最高级
        now_exp = last_exp;
        if (level == this.level_exp.Length - 1) {
            next_level_exp = now_exp;
        }
        else {
            next_level_exp = this.level_exp[level + 1];
        }

        return level;
    }
}
