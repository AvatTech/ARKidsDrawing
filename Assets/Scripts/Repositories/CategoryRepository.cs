using System.Threading.Tasks;
using Firebase.Firestore;
using Zenject;

namespace Repositories
{
    public class CategoryRepository
    {
        [Inject] private FirestoreCategoryManager _manager;

        public async Task<QuerySnapshot> GetCategories()
        {
            return await _manager.GetCategories();
        }
    }
}