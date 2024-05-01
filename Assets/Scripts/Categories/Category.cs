using System.Collections.Generic;
using Sketches.Model;

namespace Categories
{
    public class Category
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string CoverImageUrl { get; private set; }

        public List<Sketch> AllSketch { get; private set; }


        public Category(string name, string id, string coverImageUrl)
        {
            Name = name;
            Id = id;
            CoverImageUrl = coverImageUrl;
            AllSketch = new();
        }
    }
}