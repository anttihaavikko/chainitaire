using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    public TMPro.TMP_Text names, scores;
    public Appearer moreButton;

    private int page = -1;
    private bool moreHidden;

    // Start is called before the first frame update
    void Start()
    {
        LoadMore();
    }

    public void LoadMore()
    {
        page++;
        ScoreManager.Instance.LoadLeaderBoards(page);
    }

    private void Update()
    {
        names.text = ScoreManager.Instance.leaderBoardPositionsString;
        scores.text = ScoreManager.Instance.leaderBoardScoresString;

        if(!moreHidden && ScoreManager.Instance.endReached)
        {
            moreButton.Hide();
            moreHidden = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
