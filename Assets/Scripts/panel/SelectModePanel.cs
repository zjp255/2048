using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModePanel : MonoBehaviour
{
    public void OnSelectModeclick(int count)
    {
        //ѡ��ģʽ
        PlayerPrefs.SetInt(Const.GameMode, count);
        //������Ϸ����
        SceneManager.LoadSceneAsync(1);
    }
}
