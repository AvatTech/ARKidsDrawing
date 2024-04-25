using System;
using Firebase.Extensions;
using Unity.VisualScripting;

namespace Firebase.Firestore
{
    public class FirestoreSketchManager
    {
        private const string CategoryCollectionPath = "categories";

        private readonly FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;

        public void LoadSketchWithCategoryId<T>(string categoryId, Action<T> action)
        {
            var path = $"{CategoryCollectionPath}";
            var collection = _firestore.Collection(path).Document(categoryId).Collection(categoryId);
            collection.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Logger.Instance.InfoLog("task is faulted");
                }

                if (task.IsCompleted)
                {
                    var result = task.Result;

                    Logger.Instance.InfoLog($"documents count: {task.Result.Count}");

                    action.Invoke(result.ConvertTo<T>());

                }
            });
        }
    }
}