using System;
using System.Collections.Generic;
using UnityEngine;

namespace Customs
{
    static class UnityCustoms
    {
        /// <summary>
        /// Get size of game window whatever debug or any release platform
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetGameWindowSize()
        {
#if UNITY_EDITOR
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
#elif UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            return new Vector2(Screen.width, Screen.height);
#else
            return new Vector2(0, 0);
#endif
        }

        public static void DestroyAllChilds(GameObject obj)
        {
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(obj.transform.GetChild(i).gameObject);
#else
                GameObject.Destroy(obj.transform.GetChild(i).gameObject);
#endif
            }
        }

    }
}


