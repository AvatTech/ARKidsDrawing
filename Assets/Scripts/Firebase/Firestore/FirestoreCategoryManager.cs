using System;
using Firebase.Extensions;
using Unity.VisualScripting;

namespace Firebase.Firestore
{
    public class FirestoreCategoryManager
    {
        private const string CollectionPath = "categories";

        private readonly FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;


        public void GetCategories<T>(Action<T> action)
        {
            var collection = _firestore.Collection(CollectionPath);
            collection.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Logger.Instance.InfoLog("task is faulted");
                }

                if (task.IsCompleted)
                {
                    Logger.Instance.InfoLog(task.Result.Count + "");

                    var data = task.Result;
                    action.Invoke(data.ConvertTo<T>());
                }

            });
        }
    }
}