using System.Threading.Tasks;
using Sketches.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryController : MonoBehaviour
{
    [Header("References")] private RawImage image;
    [Header("References")] private TextMeshProUGUI textMeshPro;


    private ImageLoaderService _imageLoaderService = new();


    private void Awake()
    {
        image = GetComponentInChildren<RawImage>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public async Task SetImageFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return;

        var texture = await fetchImageFromUrl(url);

        await Task.Yield();

        image.texture = texture;
    }

    public void SetText(string text)
    {
        textMeshPro.SetText(text);
    }

    private async Task<Texture2D> fetchImageFromUrl(string url)
    {
        if (_imageLoaderService is null)
        {
            Debug.Log("image loader is null");
        }

        return await _imageLoaderService.TryGetTexture(url);
    }
}