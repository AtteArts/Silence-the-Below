using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovementScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    Camera mainCamera;
    float zAxis = 0;
    Vector3 clickOffset = Vector3.zero;
    public Transform defaultParent;
    public int defaultSibIndex;
    public GameObject currentCard;

    [SerializeField] private float zoomCoef = 1.2f;
    [SerializeField] private float overlapOffset = 50f;


    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private GameObject _draggedObject;
    private Vector3 _initialPosition;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = Vector3.one * zoomCoef;
        rectTransform.anchoredPosition += new Vector2(0, overlapOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition -= new Vector2(0, overlapOffset);
    }

    void Start()
    {
        mainCamera = Camera.allCameras[0];
        mainCamera.gameObject.AddComponent<Physics2DRaycaster>();

        zAxis = transform.position.z;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BlocksRaycasts(this, false);
        //_initialPosition = transform.position;
        currentCard = gameObject;
        defaultSibIndex = transform.GetSiblingIndex();
        clickOffset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zAxis));
        //defaultParent = transform.parent;
        //transform.SetParent(defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 tempVec = mainCamera.ScreenToWorldPoint(eventData.position) + clickOffset;
        tempVec.z = zAxis;

        transform.position = tempVec;
        //GameObject target = eventData.pointerEnter;
            
        // (target.CompareTag(_draggedObject.tag))
        //{
        //    transform.SetParent(defaultParent);
        //    GetComponent<CanvasGroup>().blocksRaycasts = true;
        //    return;
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //transform.SetParent(defaultParent);
        transform.SetSiblingIndex(defaultSibIndex);
        BlocksRaycasts(this, true);

    }

    public void BlocksRaycasts(CardMovementScript cardMovementScript, bool v)
    {
        for (int i = 0; i < cardMovementScript.transform.parent.childCount; i++)
        {
            Debug.Log("Это чайлд " + cardMovementScript.transform.parent.GetChild(i) );
            cardMovementScript.transform.parent.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = v;
        }
    }
}
