using System.Collections.Generic;
using UnityEngine;

public class RefByScript : MonoBehaviour
{
	[Header("Prefab Referneces")]
	[SerializeField]
	private List<GameObject> prefab;

	[Header("Sprite References")]
	[SerializeField]
	private List<Sprite> sprite;

	[Header("AudioClip References")]
	[SerializeField]
	private List<AudioClip> clip;



}
