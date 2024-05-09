using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Zenject;

namespace Repositories
{
    public class SketchRepository
    {
        [Inject] private FirestoreSketchManager _manager;

        public async Task<QuerySnapshot> GetSketches(string categoryId)
        {
            return await _manager.GetSketchesById(categoryId);
        }
    }
}