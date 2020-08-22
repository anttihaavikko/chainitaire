using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    public TMPro.TMP_Text names, scores;
    public Appearer moreButton, prevButton;

    private int page = -1;
    private bool nextHidden;

    // Start is called before the first frame update
    void Start()
    {
        NextPage();
    }

    public void NextPage()
    {
        page++;
        ScoreManager.Instance.LoadLeaderBoards(page);

        if (page > 0)
            prevButton.Show();
    }

    public void PrevPage()
    {
        page--;
        ScoreManager.Instance.LoadLeaderBoards(page);

        if (page == 0)
            prevButton.Hide();
    }

    private void Update()
    {
        names.text = ScoreManager.Instance.leaderBoardPositionsString;
        scores.text = ScoreManager.Instance.leaderBoardScoresString;

        if(!nextHidden && ScoreManager.Instance.endReached)
        {
            moreButton.Hide();
            nextHidden = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
