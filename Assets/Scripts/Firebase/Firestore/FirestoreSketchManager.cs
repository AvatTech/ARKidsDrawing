using System.Threading.Tasks;

namespace Firebase.Firestore
{
    public class FirestoreSketchManager
    {
        private const string CollectionPath = "categories";
        private const string SketchesPath = "sketches";

        private readonly FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;

        public async Task<QuerySnapshot> GetSketchesById(string categoryId)
        {
            var collection = _firestore.Collection(CollectionPath).Document(categoryId).Collection(SketchesPath);
            return await collection.GetSnapshotAsync();
        }
    }
}