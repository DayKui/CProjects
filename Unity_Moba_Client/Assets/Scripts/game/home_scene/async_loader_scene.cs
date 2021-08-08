using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class async_loader_scene : MonoBehaviour
{
    public string scene_name; // 要加载得场景得名字;
    public Image process; // 进度条

    private AsyncOperation ao;
    // Use this for initialization
    void Start()
    {
        this.process.fillAmount = 0;
        this.StartCoroutine(this.async_load_scene());
    }

    IEnumerator async_load_scene()
    {
        this.ao = SceneManager.LoadSceneAsync(this.scene_name);
        this.ao.allowSceneActivation = false; // 设置程不自动切换;

        yield return this.ao;
    }
    // Update is called once per frame
    void Update()
    {
        float per = this.ao.progress; // 当前加载进度的百分比; 最大的值 0.9f
        Debug.Log(per); // 当前预加载场景的百分比;

        if (per >= 0.9f)
        {  // 加载完了
            this.ao.allowSceneActivation = true;
        }
        this.process.fillAmount = per / 0.9f; // [0, 1]
    }
}
