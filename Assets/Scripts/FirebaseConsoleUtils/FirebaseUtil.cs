using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

namespace FirebaseConsoleUtils
{
    public class FirebaseUtil : MonoBehaviour
    {
        private FirebaseFirestore db;

        void Start()
        {
            Debug.Log("Start");
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                db = FirebaseFirestore.DefaultInstance;
                AddFieldToAllDocuments();
                Debug.Log("finish check");
            });
        }

        void AddFieldToAllDocuments()
        {
            // Reference to your collection
            CollectionReference collectionRef = db.Collection("categories")
                .Document("objects")
                .Collection("sketches");

            // Get all documents in the collection
            collectionRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    QuerySnapshot snapshot = task.Result;

                    // Loop through each document in the collection
                    foreach (DocumentSnapshot document in snapshot.Documents)
                    {
                        // Create a Dictionary to hold the new field
                        Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                            { "isPremium", false } // Add your field and its value here
                        };

                        // Update the document with the new field
                        document.Reference.UpdateAsync(updates).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                Debug.Log($"Updated document: {document.Id}");
                                document.TryGetValue<string>("imageUrl", out string url);
                                Debug.Log($"{url}");
                            }
                            else
                            {
                                Debug.LogError($"Failed to update document: {document.Id}");
                            }
                        });
                    }
                }
                else
                {
                    Debug.LogError("Failed to get documents in collection");
                }
            });
        }
    }
}