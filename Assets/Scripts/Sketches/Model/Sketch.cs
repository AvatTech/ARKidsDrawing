namespace Sketches.Model
{
    public class Sketch
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }


        public Sketch(string name = "non", string imageUrl = "non")
        {
            Name = name;
            ImageUrl = imageUrl;
        }
    }
}