using System.Collections.Generic;
using Categories.Model;
using Newtonsoft.Json;
using UnityEngine;
using Utills;

namespace Storage
{
    //todo: Clean storage anti-pattern and bad code.

    public interface ILocalStorage
    {
        public bool TryLoadInt(string key, out int value);
        public void SaveInt(string key, int value);


        void SaveCategory(List<Category> categories);
        List<Category> LoadCategory();
    }


    public class LocalStorage : ILocalStorage
    {
        public void SaveData(string key, object value)
        {
            var jsonData = JsonConvert.SerializeObject(value);

            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public bool TryLoadData<T>(string key, out T value)
        {
            value = default;

            if (PlayerPrefs.HasKey(key))
            {
                string jsonData = PlayerPrefs.GetString(key);
                value = JsonConvert.DeserializeObject<T>(jsonData);
                return true;
            }

            return false;
        }


        //----------------------------------

        public bool TryLoadInt(string key, out int value)
        {
            value = default;

            if (PlayerPrefs.HasKey(key))
            {
                value = PlayerPrefs.GetInt(key);
                return true;
            }

            return false;
        }

        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
            Debug.Log($"Saveeeeeed {key}");
        }


        //----------------------------------------------------------
        public void SaveCategory(List<Category> categories)
        {
            var jsonData = JsonConvert.SerializeObject(categories);
            //string jsonData = 
            PlayerPrefs.SetString(Constants.KeyCategories, jsonData);
            PlayerPrefs.Save();

            Debug.Log(jsonData);
        }

        public List<Category> LoadCategory()
        {
            if (PlayerPrefs.HasKey(Constants.KeyCategories))
            {
                string jsonData = PlayerPrefs.GetString(Constants.KeyCategories);
                return JsonConvert.DeserializeObject<List<Category>>(jsonData);
            }

            return null;
        }
    }
}