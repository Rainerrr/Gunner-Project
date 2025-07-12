using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] float destroyAfter = 1f;
    void Awake()
    {
        Destroy(this.gameObject, destroyAfter);
    }

}
