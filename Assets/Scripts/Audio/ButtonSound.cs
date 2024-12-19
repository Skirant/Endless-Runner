using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlaySound()
    {
        FindAnyObjectByType<AudioManager>().Play("Open");
    }
}
