using System;
using UnityEngine;
using Zenject;

namespace Network
{
    public class ConnectionCheckerController : MonoBehaviour
    {
        [Inject] private ConnectionChecker _connectionChecker;


        private void Start()
        {
            if (!_connectionChecker.IsConnectedToNetwork())
                SplashScreenController.Instance.SetState(SplashScreenState.Failed);
        }
    }
}