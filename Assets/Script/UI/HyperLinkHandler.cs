using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// Handles TMP text hyper links.
[RequireComponent(typeof(TMP_Text))]
public class HyperLinkHandler : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Texture2D _linkCursor;

	private TMP_Text _text;
	private Vector2 _linkCursorHotSpot = new Vector2(2, 1);

	public void Start()
	{
		_text = GetComponent<TMP_Text>();
	}

	public void Update()
	{
		var linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, null);
		if (linkIndex != -1) {
			Cursor.SetCursor(_linkCursor, _linkCursorHotSpot, CursorMode.ForceSoftware);
		}
		else {
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		var linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, null);
		if (linkIndex != -1) {
			TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[linkIndex];
			Application.OpenURL(linkInfo.GetLinkID());
		}
	}
}
