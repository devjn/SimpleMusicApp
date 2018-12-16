using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCreation : MonoBehaviour
{

	public Toggle btnPrefab;
	public LayoutGroup gridView;

	private Toggle[,] toggles = new Toggle[8, 8];

	// Use this for initialization
	void Start ()
	{
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				//ToggleGroup group = GameManager.Instance.Groups [x];
				Toggle toggle = Instantiate (btnPrefab);
				toggles [x, y] = toggle;
				//toggle.group = group;
				toggle.GetComponent<GridTag> ().X = x;
				toggle.GetComponent<GridTag> ().Y = y;
				toggle.transform.SetParent (gridView.transform);
				toggle.onValueChanged.AddListener (delegate {
					GameManager.Instance.toggleSelected (toggle);
				});
			}
		}
	}

	// Update is called once per frame
	public void UpdateToggles (bool[,] data)
	{
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				toggles [x, y].isOn = data [x, y];
			}
		}
	}
}
