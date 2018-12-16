using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	public static GameManager Instance { get { return instance; } }


	public ToggleGroup groupPrefab;
	public Text txtCurrentColumn;
	public AudioClip[] audioClips;

	private ToggleGroup[,] groups = new ToggleGroup[3, 8];

	public ToggleGroup[] Groups { get { return null; } }

	private GameGridHolder[] gameGrids = new GameGridHolder[3].Select (h => new GameGridHolder ()).ToArray ();

	GameGridHolder GameGrid { get { return gameGrids [currentActiveLoop]; } }

	private AudioSource audioSource;
	private GridCreation gridCreation;
	private int currentActiveLoop = 0;
	private int currentColumn = 0;

	protected int CurrentColumn {
		get { return currentColumn; } 
		set {
			txtCurrentColumn.text = currentColumn + 1 + "/8";
			currentColumn = value >= 8 ? 0 : value; 
		} 
	}



	private void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		gridCreation = GameObject.Find ("Grid").GetComponent<GridCreation> ();
		ChangeToggleGroup toggleGroup = GameObject.Find ("TopPanel").GetComponent<ChangeToggleGroup> ();
		toggleGroup.OnChange += toggleGroup_OnChange;

		// Uncomment next line to play multi sound
		InvokeRepeating ("playMulti", 1f, 1f);
	}

	void Update ()
	{
		// comment this line if multi sound mode selected
		//play ();
	}

	// Single sound per column
	void play ()
	{
		if (!audioSource.isPlaying) {
			int[] selectedCells = GameGrid.GetSelectedCells (currentColumn);

			if (selectedCells.Length == 0) {
				Debug.Log ("No selected cells in this column is, CurrentColumn = " + CurrentColumn);
				CurrentColumn++;
				return;
			}
			int y = selectedCells.FirstOrDefault ();

			audioSource.clip = audioClips [y];
			audioSource.time = 0f;
			audioSource.Play ();
			audioSource.SetScheduledEndTime (AudioSettings.dspTime + 1f); // Play one second
			Debug.Log ("Playing sound");
			CurrentColumn++;
		}
	}

	// Multi sound per column
	void playMulti ()
	{
		int[] selectedCells = GameGrid.GetSelectedCells (currentColumn);
		if (selectedCells.Length == 0) {
			Debug.Log ("No selected cells in this column is, CurrentColumn = " + CurrentColumn);
			CurrentColumn++;
			return;
		}

		foreach (int y in selectedCells) {
			audioSource.PlayOneShot (audioClips [y]);
			Debug.Log ("Playing sound");
		}
		CurrentColumn++;
	}

	public void toggleSelected (Toggle toggle)
	{
		var tag = toggle.GetComponent<GridTag> ();
		Debug.Log ("x = " + tag.X + " y = " + tag.Y);
		GameGrid.gameCells [tag.X, tag.Y] = toggle.isOn;
	}


	void toggleGroup_OnChange (Toggle toggle)
	{
		TagId tag = toggle.GetComponent<TagId> ();
		currentActiveLoop = tag.tagId;
		gridCreation.UpdateToggles (GameGrid.gameCells);
	}

}