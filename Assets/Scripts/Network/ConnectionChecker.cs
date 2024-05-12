using UnityEngine;

namespace Network
{
    public class ConnectionChecker
    {
        // Check whether device connected to the internet or not
        public bool IsConnectedToNetwork()
        {
            // Check if the device is connected to any network
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                return true;
            }

            return false;
        }
    }
}