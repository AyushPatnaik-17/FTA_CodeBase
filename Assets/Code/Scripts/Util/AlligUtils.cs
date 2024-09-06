using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace AlligatorUtils
{
    public static class Extensions
    {
        public static Dictionary<string, string> Colors =
        new()
        {
            { "black", "#000000" },
            { "white", "#FFFFFF" },
            { "red", "#FF0000" },
            { "green", "#00FF00" },
            { "blue", "#0000FF" },
            { "yellow", "#FFFF00" },
            { "cyan", "#00FFFF" },
            { "magenta", "#FF00FF" },
            { "silver", "#C0C0C0" },
            { "gray", "#808080" },
            { "maroon", "#800000" },
            { "olive", "#808000" },
            { "purple", "#800080" },
            { "teal", "#008080" },
            { "navy", "#000080" },
            { "orange", "#FFA500" },
            { "pink", "#FFC0CB" },
            { "brown", "#A52A2A" },
            { "gold", "#FFD700" },
            { "beige", "#F5F5DC" }
        };

        /// <summary>
        /// Returns Child with given Tag
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Transform GetChildWithTag(this Transform parent, string tag)
        {
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tag))
                {
                    return child;
                }

                // Recursively search in child's children
                Transform childResult = child.GetChildWithTag(tag);
                if (childResult != null)
                {
                    return childResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Retruns child with given name
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform GetChildWithName(this Transform parent, string name)
        {
            Transform target = null;
            if (parent.name == name) return target;
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }
                Transform result = GetChildWithName(child, name);
                if (result != null) return result;
            }
            return target;
        }

        public static string Multiply(this string source, int multiplier)
        {
            StringBuilder sb = new StringBuilder(multiplier * source.Length);
            if (multiplier == 0)
                return source;
            for(int i = 0; i < multiplier; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

        public static List<GameObject> GetChildrenWithName(this Transform parent, string name)
        {
            List<GameObject> childrenWithName = new();
            foreach(Transform child in parent)
            {
                if (child.name == name)
                {
                    childrenWithName.Add(child.gameObject);
                }
                List<GameObject> childrenWithSameName = child.GetChildrenWithName(name);
                childrenWithName.AddRange(childrenWithSameName);
            }
            return childrenWithName;
        }

        public static string ToExplorerPath(this string str)
        {
            return str.Replace('/', '\\');
        }
        public static string ToUnityPath(this string str)
        {
            return str.Replace('\\', '/');
        }

        public static void Print<T>(this T value, string message = "", string color = "gray")
        {
            var hex = Colors[color.ToLower()]; 
            //Debug.Log(message + value);
            Debug.Log($"<color={hex}><i>{message}{value}</i></color>");
        }

        public static void Print<T>(this List<T> list, string message = "", string color = "cyan")
        {
            var hex = Colors[color.ToLower()];
            for (int i = 0; i < list.Count;i++)
            {
                //Debug.Log(message + list[i]);
                Debug.Log($"<color={hex}><b>{message}{list[i]}</b></color>");
            }
        }
        public static void Print<T>(this HashSet<T> list, string message = "", string color = "cyan")
        {
            var hex = Colors[color.ToLower()];
            for (int i = 0; i < list.Count;i++)
            {
                //Debug.Log(message + list.ElementAt(i));
                Debug.Log($"<color={hex}><b>{message}{list.ElementAt(i)}</b></color>");
            }
        }

        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out int result)) return result;
            else
            {
                "Couldnt convert string to int".Print(); return -1;
            }
        }

        public static float ToFloat(this string str)
        {
            if (float.TryParse(str, out float result)) return result;
            else
            {
                "Couldnt convert string to int".Print(); return -1;
            }
        }

        public static bool HasComponent<T>(this GameObject flag) where T : Component
        {
            return flag.GetComponent<T>() != null;
        }
    }

    public class ValuePair<T1, T2>
    {
        public T1 Val1 { get; set; }
        public T2 Val2 { get; set; }

        public ValuePair(T1 v1, T2 v2) 
        {
            Val1 = v1;
            Val2 = v2;
        } 
    }

    [System.Serializable]
    public class Triplet<TKey, T1, T2>
    {
        public SerializedDictionary<TKey, ValuePair<T1, T2>> m_Triplet;

        public Triplet()
        {
            // _triplet = new Dictionary<TKey, ValuePair<T1, T2>>();
            m_Triplet = new SerializedDictionary<TKey, ValuePair<T1, T2>>();
        }

        public void Add(TKey key, T1 v1, T2 v2) 
        {
            m_Triplet[key] = new ValuePair<T1, T2>(v1, v2);
        }

        public void Remove(TKey key)
        {
            m_Triplet.Remove(key);
        }

        public bool ContainsKey(TKey key) 
        {
            return m_Triplet.ContainsKey(key);
        }

        public object Get(TKey key, int index)
        {
            if (m_Triplet.TryGetValue(key, out ValuePair<T1, T2> pair))
            {
                if (index == 0)
                    return pair.Val1;
                else if (index == 1)
                    return pair.Val2;
                else
                    throw new ArgumentOutOfRangeException(nameof(index), "Index must be 0 or 1");
            }
            throw new KeyNotFoundException($"The key '{key}' was not found in the dictionary.");
        }

        public bool TryGetValue(TKey key, out T1 value1, out T2 value2)
        {
            if (m_Triplet.TryGetValue(key, out ValuePair<T1, T2> pair))
            {
                value1 = pair.Val1;
                value2 = pair.Val2;
                return true;
            }
            value1 = default(T1);
            value2 = default(T2);
            return false;
        }
    }
}

