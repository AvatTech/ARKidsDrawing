namespace Sketches.Model
{
    public class Sketch
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPremium { get; set; }

        public Sketch(string name = "non", string imageUrl = "non", bool isPremium = false)
        {
            Name = name;
            ImageUrl = imageUrl;
            IsPremium = isPremium;
        }
    }
}