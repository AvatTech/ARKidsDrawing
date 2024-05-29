using Sketches.Controller;
using UnityEngine;

namespace Sketches.Utills
{
    
    public class CurrentSketchHolder : MonoBehaviour
    {
        public static CurrentSketchHolder Instance;
        

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        
            DontDestroyOnLoad(gameObject);
        }


        [field: SerializeField] public string CurrentSketchUrl { get; set; }
    }
}