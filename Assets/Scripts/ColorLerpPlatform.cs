using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ColorLerpPlatform : MonoBehaviour
{
    [SerializeField] float delayBetweenTiles = 1f;
    private ColorWhenTouch[] grassTilesSorted;
    private bool isLerping = false;

    // Use this for initialization
    void Start()
    {
        grassTilesSorted = transform.GetComponentsInChildren<ColorWhenTouch>();
        grassTilesSorted = grassTilesSorted.OrderBy(go => go.transform.position.x).ToArray();
        //Set id to sorted tiles...
        int id = 0;
        foreach (ColorWhenTouch grassTile in grassTilesSorted)
        {
            grassTile.SetIdToThisTile(id);
            id++;
        }
    }

    IEnumerator ChangeColorToTheRight(int startIndex)
    {
        for (int i = startIndex; i < grassTilesSorted.Length; i++)
        {
            grassTilesSorted[i].SetColorToWhite();
            yield return new WaitForSeconds(delayBetweenTiles);
        }
    }

    IEnumerator ChangeColorToTheLeft(int startIndex)
    {
        for (int i = startIndex; i >= 0; i--)
        {
            grassTilesSorted[i].SetColorToWhite();
            yield return new WaitForSeconds(delayBetweenTiles);
        }
    }


    public void StartLerpColor(int idNumber)
    {
        if (!isLerping)
        {
            isLerping = true;
            StartCoroutine(ChangeColorToTheRight(idNumber));
            StartCoroutine(ChangeColorToTheLeft(idNumber));
        }

    }
}
