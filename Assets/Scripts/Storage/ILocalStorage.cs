using System.Collections.Generic;
using Categories.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Storage
{
    public interface ILocalStorage
    {
        void SaveData(List<Category> categories);
        List<Category> LoadData();
    }


    public class LocalStorage : ILocalStorage
    {
        private const string KEY_CATEGORIES = "KEY_CATEGORIES";


        public void SaveData(List<Category> categories)
        {
            var jsonData = JsonConvert.SerializeObject(categories);
            //string jsonData = 
            PlayerPrefs.SetString(KEY_CATEGORIES, jsonData);
            PlayerPrefs.Save();

            Debug.Log(jsonData);
        }

        public List<Category> LoadData()
        {
            if (PlayerPrefs.HasKey(KEY_CATEGORIES))
            {
                string jsonData = PlayerPrefs.GetString(KEY_CATEGORIES);
                return JsonConvert.DeserializeObject<List<Category>>(jsonData);
            }

            return null;
        }
    }
}