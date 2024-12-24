using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBTemplate.Utils.Others
{
    public class DestroyIfProd : MonoBehaviour
    {
        private void Awake()
        {
#if ENV_PROD
Destroy(gameObject);
#endif
        }
    }

}
