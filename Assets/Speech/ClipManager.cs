using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;
using System.Text;
using System.Security.Cryptography.X509Certificates;

public class ClipManager : MonoBehaviour
{
    public static ClipManager Instance;
    public TextAsset debug_ClipData;
    public bool useDebugData = false;
    private void Awake() {
        Instance = this;
    }

    public void LoadNextClip()
    {
        StartCoroutine(LoadClip_Coroutine());
    }

    IEnumerator LoadClip_Coroutine()
    {
        DisplayLoading.Instance.FadeIn();

        Clip clip = new Clip();
        if (useDebugData) {
            clip.debugData = debug_ClipData.text;
        }

        yield return clip.DownloadData();
        yield return clip.DownloadAudio();
        yield return new WaitForEndOfFrame();

        DisplayLoading.Instance.FadeOut();

        yield return new WaitForSeconds(1f);

        ClipPlayer.Instance.InitPlayer(clip);
        GameManager.Instance.ShowGameUI();

        yield return new WaitForSeconds(2);

        ClipPlayer.Instance.Play();

    }
    

}
