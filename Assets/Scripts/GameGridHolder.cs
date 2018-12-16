using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameGridHolder
{
	public bool[,] gameCells = new bool[8, 8];

	public int[] GetSelectedCells (int x)
	{
		return Enumerable.Range (0, gameCells.GetLength (1)).Where (i => gameCells [x, i]).ToArray ();
	}

}