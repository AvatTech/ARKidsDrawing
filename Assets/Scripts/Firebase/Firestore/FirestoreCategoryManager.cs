using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using Unity.VisualScripting;
using UnityEngine;

namespace Firebase.Firestore
{
    public class FirestoreCategoryManager
    {
        private const string CollectionPath = "categories";

        private readonly FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;


        public async Task<QuerySnapshot> GetCategories()
        {
            var collection = _firestore.Collection(CollectionPath);
            return await collection.GetSnapshotAsync();

            // ContinueWithOnMainThread(task =>
            // {
            //     if (task.IsFaulted)
            //     {
            //         Debug.Log("Task is Faulted!");
            //     }
            //
            //     if (task.IsCompleted)
            //     {
            //         Debug.Log("Task is completed!");
            //         var data = task.Result;
            //
            //         action.Invoke(data.ConvertTo<T>());
            //     }
            // });
        }
    }
}