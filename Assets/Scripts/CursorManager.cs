using UnityEngine;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    private HashSet<string> cursorRequests = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RequestCursor(string reason)
    {
        cursorRequests.Add(reason);
        UpdateCursorState();
    }

    public void ReleaseCursor(string reason)
    {
        cursorRequests.Remove(reason);
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        if (cursorRequests.Count > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ForceReset()
    {
        cursorRequests.Clear();
        UpdateCursorState();
    }
}