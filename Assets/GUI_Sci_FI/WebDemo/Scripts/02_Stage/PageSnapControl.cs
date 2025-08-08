using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PageSnapControl : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
	private int pageCount;
	private int pageNum;
	private ScrollRect scrollRect;
	private GameObject content;
	private Vector3 firstPos;
	private Vector3 endPos;
	private Vector2 contentPos = Vector2.zero;
	private List<GameObject> naviIcons = new List<GameObject> ();
	//private StageController _StageController;

	void Start () {
		//_StageController = GameObject.FindWithTag ("StageController").GetComponent<StageController>();

		scrollRect = GetComponent<ScrollRect> ();
		content = scrollRect.content.gameObject;
		pageCount = content.gameObject.transform.childCount;
	}

	public void OnBeginDrag (PointerEventData data) {
		scrollRect.DOKill ();
		firstPos = Input.mousePosition;
	}

	public void OnEndDrag (PointerEventData data) {
		endPos = Input.mousePosition;
		float dist = Vector3.Distance (firstPos, endPos);

	}


	//public void ValueChange () { 
	//	for (int i = 0; i < _StageController._Stage.Length; i++) {
	//		_StageController._Stage [i].DefultSetting ();
	//	}
	//}


}
