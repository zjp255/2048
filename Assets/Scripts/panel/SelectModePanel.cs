using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModePanel : MonoBehaviour
{
    public void OnSelectModeclick(int count)
    {
        //选择模式
        PlayerPrefs.SetInt(Const.GameMode, count);
        //加载游戏场景
        SceneManager.LoadSceneAsync(1);
    }
}
