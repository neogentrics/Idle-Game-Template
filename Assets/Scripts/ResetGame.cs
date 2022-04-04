using UnityEngine;

public class ResetGame : MonoBehaviour
{
    public void Reset()
    {
        Controller.Instance.Reset();
    }
}
