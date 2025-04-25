using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera cam;
    private Vector3 dragOffset;

    private RectTransform canvasRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cam = canvas.renderMode == RenderMode.WorldSpace ? canvas.worldCamera : Camera.main;
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            cam,
            out Vector3 worldMousePos))
        {
            dragOffset = rectTransform.position - worldMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            cam,
            out Vector3 worldMousePos))
        {
            Vector3 targetPos = worldMousePos + dragOffset;

            // Clamp the target position within the canvas bounds
            Vector3 localPoint = canvasRect.InverseTransformPoint(targetPos);
            Vector3 clampedLocalPoint = ClampToCanvas(localPoint);
            rectTransform.position = canvasRect.TransformPoint(clampedLocalPoint);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optional: cleanup
    }

    private Vector3 ClampToCanvas(Vector3 localPos)
    {
        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 halfSize = canvasSize * 0.5f;

        Vector2 elementSize = rectTransform.rect.size;
        Vector2 elementHalfSize = elementSize * 0.5f;

        float x = Mathf.Clamp(localPos.x, -halfSize.x + elementHalfSize.x, halfSize.x - elementHalfSize.x);
        float y = Mathf.Clamp(localPos.y, -halfSize.y + elementHalfSize.y, halfSize.y - elementHalfSize.y);

        return new Vector3(x, y, localPos.z);
    }
}