using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string type;
    public int xPos, yPos;
    //Update Position of One Tile
    public void ChangePosition(int newXPos,int newYPos)
    {
        xPos = newXPos;
        yPos = newYPos;
    }
    //Unity Function For Mouse Down
    private void OnMouseDown()
    {
        PuzzleController.Instance.DragTile(this);
    }
    //Unity Function For Mouse Over
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PuzzleController.Instance.DropTile(this);
        }
    }
}
