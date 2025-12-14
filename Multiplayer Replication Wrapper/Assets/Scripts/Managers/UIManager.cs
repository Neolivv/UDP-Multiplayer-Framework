using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI PingText;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdatePing(double ping)
    {
        PingText.SetText("Ping : " + ping.ToString().Substring(0,4));
    }
}
