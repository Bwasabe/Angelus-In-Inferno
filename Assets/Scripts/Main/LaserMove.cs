// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LaserMove : MonoBehaviour
// {
//     private bool isMin = false;
//     private bool isMax = false;
//     void Start(){
//         if(gameObject.transform.rotation.z <= 0) isMin = true;
//         if(gameObject.transform.rotation.z >= 0) isMax = true;
//     }
//     private void OnEnable()
//     {
//         StartCoroutine(LaserRote());
//     }
//     private IEnumerator LaserRote()
//     {
//         for (int i = 0; i < 59; i++)
//         {
//             if(isMin){
//                 gameObject.transform.Rotate(new Vector3(0f,0f,1f));
//                 gameObject.transform.Translate()
//             }
//             else if(isMax){
//                 gameObject.transform.Rotate(new Vector3(0f,0f,-1f));
//             }
//             yield return new WaitForSeconds(0.01f);
//         }
//     }
//     private void OnDisable(){
        
//     }
// }
