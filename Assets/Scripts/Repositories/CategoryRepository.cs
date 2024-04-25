using System;
using Firebase.Firestore;

namespace Repositories
{
    public class CategoryRepository<T>
    {
        public void GetCategories(Action<T> action)
        {
            var manager = new FirestoreCategoryManager();
            manager.GetCategories(action);
        }
    }
}