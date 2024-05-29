using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public static class ConnectionChecker
    {
        private const string TEST_URL = "https://firebase.google.com/";
        
        public static bool IsNetworkChecked = false;

        
        // check for connection
        public static async Task<bool> IsConnectedToNetwork()
        {
            bool isConnected = true;
            
            UnityWebRequest request = new UnityWebRequest(TEST_URL);
            
            request.timeout = 5; // Timeout in seconds

            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                isConnected = false;
            }
            
            return isConnected;
        }
    }
}