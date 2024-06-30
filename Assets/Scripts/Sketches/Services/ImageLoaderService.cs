using System;
using System.Threading.Tasks;
using UnityEngine;
using imageLoader = Extensions.Unity.ImageLoader.ImageLoader;

namespace Sketches.Services
{
    public class ImageLoaderService
    {
        public ImageLoaderService()
        {
            Debug.Log("ImageLoader service Instantiated!");
        }

        public async Task<Texture2D> TryGetTexture(string url, Action<Exception> onFailed)
        {
            Texture2D t = null;

            await imageLoader.LoadSprite(url).Then((sprite => { t = sprite.texture; })).Failed(onFailed);

            return t;
        }
    }
}