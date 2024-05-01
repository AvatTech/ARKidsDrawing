using System;
using UnityEngine;

namespace Sketches.Builder
{
    public class SketchBuilder
    {
        private Model.Sketch _sketch;


        public static SketchBuilder Builder()
        {
            var sketchBuilder = new SketchBuilder();
            sketchBuilder.CheckSketchNull();
            return sketchBuilder;
        }

        public Model.Sketch Build()
        {
            if (_sketch == null)
                throw new NullReferenceException();

            return _sketch;
        }


        private void CheckSketchNull()
        {
            if (_sketch is null)
                _sketch = new Model.Sketch();
        }


        #region Builder Methods

        public SketchBuilder SetName(string name)
        {
            _sketch.Name = name;
            return this;
        }

        public SketchBuilder SetSprite(Sprite sprite)
        {
            _sketch.SetSprite(sprite);
            return this;
        }

        public SketchBuilder SetScale(int scale)
        {
            _sketch.transform.localScale = new Vector3(0, scale, 0);
            return this;
        }

        /// <summary>
        /// Adjust transparency of sketch
        /// </summary>
        /// <param name="value">a value between 0 - 1</param>
        /// <returns></returns>
        public SketchBuilder SetTransparency(int value)
        {
            _sketch.SetTransparency(value);
            return this;
        }

        public SketchBuilder SetRotation(int value)
        {
            _sketch.SetRotation(value);
            return this;
        }

        #endregion
    }
}