using UnityEngine;

namespace Firebase.Messaging
{
    public class NotificationManager : MonoBehaviour
    {
        private void Start()
        {
            Logger.Instance.InfoLog("Start config FirebaseMessaging");
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
            Logger.Instance.InfoLog("End config FirebaseMessaging");
        }

        private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        {
            Logger.Instance.InfoLog("token: " + e.Token);
        }
        
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Logger.Instance.InfoLog("Message: " +e.Message.MessageId);
        }
    }
}