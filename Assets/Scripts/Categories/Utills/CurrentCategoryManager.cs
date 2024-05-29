using Categories.Model;
using UnityEngine;

namespace Categories.Utills
{
    public class CurrentCategoryManager : MonoBehaviour
    {
        public static CurrentCategoryManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }


        public Category CurrentCategory = null;
    }
}