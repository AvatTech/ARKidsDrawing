using System;
using Sketches.Model;
using UnityEngine;

namespace Sketches.Builder
{
    public class SketchBuilder
    {
        private Sketch _sketch;


        public static SketchBuilder Builder()
        {
            var sketchBuilder = new SketchBuilder();
            sketchBuilder.CheckSketchNull();
            return sketchBuilder;
        }

        public Sketch Build()
        {
            if (_sketch == null)
                throw new NullReferenceException();

            return _sketch;
        }


        private void CheckSketchNull()
        {
            if (_sketch is null)
                _sketch = new Sketch();
        }


        #region Builder Methods

        public SketchBuilder SetName(string name)
        {
            _sketch.Name = name;
            return this;
        }

        public SketchBuilder SetImageUrl(string imageUrl)
        {
            _sketch.ImageUrl = imageUrl;
            return this;
        }

        public SketchBuilder SetIsPremium(bool isPremium)
        {
            _sketch.IsPremium = isPremium;
            if (isPremium)
                Debug.Log("we find a premium!");
            return this;
        }

        #endregion
    }
}