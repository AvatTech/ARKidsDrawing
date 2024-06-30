using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using Repositories;
using Sketches.Builder;
using Sketches.Model;
using Zenject;

namespace Sketches.Services
{
    public class FetchSketchesService
    {
        [Inject] private readonly SketchRepository _sketchRepository;

        private List<Sketch> OnQueryReceived(QuerySnapshot querySnapshot)
        {
            List<Sketch> sketches = new();

            foreach (var doc in querySnapshot.Documents)
            {
                var dic = doc.ToDictionary();

                dic.TryGetValue("name", out var sketchName);
                dic.TryGetValue("imageUrl", out var sketchImageUrl);

                Sketch sketch = SketchBuilder.Builder()
                    .SetName(sketchName?.ToString())
                    .SetImageUrl(sketchImageUrl?.ToString())
                    .Build();


                sketches.Add(sketch);
            }

            return sketches;
        }


        public async Task<List<Sketch>> FetchSketchesById(string categoryId)
        {
            QuerySnapshot snapshot = await _sketchRepository.GetSketches(categoryId);

            var readySketches = OnQueryReceived(snapshot);


            return readySketches;
        }
    }
}