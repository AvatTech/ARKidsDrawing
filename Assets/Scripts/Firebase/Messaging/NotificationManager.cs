using UnityEngine;

namespace Firebase.Messaging
{
    public class NotificationManager : MonoBehaviour
    {
        private void Start()
        {
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        }

        private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        {
            //
        }
        
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //
        }
    }
}