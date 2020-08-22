using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameInputView : MonoBehaviour
{
    public TMPro.TMP_Text display;
    public NameInput nameInput;
    public Appearer go;

    // Start is called before the first frame update
    void Start()
    {
        nameInput.Ask();

        if (PlayerPrefs.HasKey("PlayerName"))
            this.StartCoroutine(go.Show, 1.5f);

        nameInput.onDone += Done;
        nameInput.onUpdate += NameChanged;
    }

    private void NameChanged(string plr)
    {
        display.text = plr;

        if (plr.Length > 1)
            go.Show();
        else
            go.Hide();
    }

    private void Done(string plr)
    {
        nameInput.onDone -= Done;
        nameInput.onUpdate -= NameChanged;

        SceneChanger.Instance.ChangeScene("Main");
    }

    public void Go()
    {
        nameInput.Save();
    }
}
