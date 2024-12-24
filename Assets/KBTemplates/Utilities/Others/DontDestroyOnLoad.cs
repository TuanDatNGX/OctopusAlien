using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBTemplate.Utils.Others
{
    public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
}

