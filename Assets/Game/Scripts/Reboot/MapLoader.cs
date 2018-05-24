using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Reboot
{
    public class MapLoader
    { 
        string PATH = Application.persistentDataPath + "/Maps/";

        public bool TryLoadMap(string fileName, out Map<Hex> map)
        {
            string fullPath = string.Format("{0}{1}", PATH, fileName);
            if (File.Exists(fullPath))
            {
                StreamReader streamReader = new StreamReader(fullPath);

                map = JsonUtility.FromJson<Map<Hex>>(streamReader.ReadToEnd());
                return true;
            }
            else
            {
                throw new System.Exception(string.Format("File: {0} does not exist", fileName));
            }
        }
    }
}