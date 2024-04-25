using System;
using Firebase.Firestore;

namespace Repositories
{
    public class SketchRepository<T>
    {
        public void GetSketches(string categoryId, Action<T> action)
        {
            var manager = new FirestoreSketchManager();
            manager.LoadSketchWithCategoryId(categoryId, action);
        }
    }
}