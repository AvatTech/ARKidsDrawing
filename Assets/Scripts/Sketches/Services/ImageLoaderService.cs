using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Sketches.Services
{
    public class ImageLoaderService
    {
        public ImageLoaderService()
        {
            Debug.Log("ImageLoader service Instantiated!");
        }


        public async Task<Texture2D> TryGetTexture(string url)
        {
            Debug.Log("Start getting texture.");
            Texture2D texture = null;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            request.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }


            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("image loaded successfully!");
                texture = DownloadHandlerTexture.GetContent(request);
            }


            if (texture is null)
            {
                Debug.Log($"Texture is null. reuslt: {request.result}");
            }
            else
            {
                Debug.Log($"Texture is not null.reuslt: {request.result}");
            }

            return texture;
        }
    }
}