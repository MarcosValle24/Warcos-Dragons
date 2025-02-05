using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string type;
    public int xPos, yPos;
    public PuzzleController puzzleController;
    [HideInInspector]
    public SpriteRenderer render;

    public void Constructor(PuzzleController _puzzleController, int xPosition, int yPosition)
    {
        puzzleController = _puzzleController;
        xPos = xPosition;
        yPos = yPosition;
        render = GetComponent<SpriteRenderer>();
    }

    public void ChangePosition(int newXPos,int newYPos)
    {
        xPos = newXPos;
        yPos = newYPos;
    }

    private void OnMouseDown()
    {
        puzzleController.DragTile(this);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            puzzleController.DropTile(this);
        }
    }
}
