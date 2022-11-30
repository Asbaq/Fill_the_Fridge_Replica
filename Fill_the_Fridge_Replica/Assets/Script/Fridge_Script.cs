using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Fridge_Script : MonoBehaviour
{
    // Variable Initialization
    public GameObject MilkPrefab;
    public float AddMilkDelay = 0.2f;
    private bool addNewMilk = false;
    private Transform holderMilk;
    private bool Door;
    Shelf_Script Upper_Shelf;
    Shelf_Script Lower_Shelf;
    [SerializeField] private AudioSource DoorSoundEffect;
    [SerializeField] private AudioSource ShelfSoundEffect;    
    // function for first frame
    private void Start() {
        
        addNewMilk = false;
        // calling Couroutine
        StartCoroutine( AddNewMilk() );
    
    }

    // Update is called once per frame    
    void Update() {  

        // OnClick Mouse Button
        if (Input.GetMouseButtonDown(0)) {  
            // Debug.Log("Mouse Down");

            // Returns a ray going from camera through a screen point.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            // Initializing RaycastHit variable
            RaycastHit hit;  
            
            // TAking ray and hit is rayhitinfo 
            if (Physics.Raycast(ray, out hit)) {  
                // Checking Condition - if ray hit the Tag Gameobject(true)
                if (hit.transform.CompareTag("Door")) {  
                    // Checking Condition
                    if(Door == false)
                    {
                    // Rotating at local position in time duration of 1 sec
                    hit.transform.DOLocalRotate( new Vector3(0, -100f, 0), 1f ).OnComplete(()=>{ Door = true;});
                    DoorSoundEffect.Play();
                    }
                    else
                    {
                    // Rotating at local position in time duration of 1 sec 
                    hit.transform.DOLocalRotate( new Vector3(0, 0f, 0), 1f ).OnComplete( ()=> { Door = false; });
                    DoorSoundEffect.Play();
                    }
                }  
                // I Had used DoTween for easy transformation - position and rotation of the gameobject with Smooth Animation. 
                
                // Checking Condition - if ray hit the Tag Gameobject(true)
                if( hit.transform.CompareTag("Upper_Shelf") )
                {
                    // Refrence the script to variable
                    Upper_Shelf = hit.transform.GetComponent<Shelf_Script>(); 
                        // Making IsOpen True
                     if(Upper_Shelf.IsOpen)
                     {
                        // First Rotating the Shelf in time duration of 0.5 sec and then moving in x axis in time duration of 1 sec & then making IsOpen false
                        // OnComplete give you the ablity to call next function after previous function is completed
                        hit.transform.DOLocalRotate( new Vector3(0,0,0), 0.5f).OnComplete( ()=> { hit.transform.DOLocalMoveX ( -0.0052f, 1 ); Upper_Shelf.IsOpen = false; } );
                        ShelfSoundEffect.Play();
                    }
                    else
                    {
                        // First moving the Shelf in x axis in time duration of 1 sec and then rotating the shelf in time duration of 0.5 sec & then making IsOpen true
                        // OnComplete give you the ablity to call next function after previous function is completed
                        hit.transform.DOLocalMoveX ( -0.026f, 1 ).OnComplete( ()=> hit.transform.DOLocalRotate( new Vector3(0,0,25), .5f).OnComplete( ()=> Upper_Shelf.IsOpen = true ));
                        ShelfSoundEffect.Play();
                    }
                }
                
                // Checking Condition - if ray hit the Tag Gameobject(true)
                if( hit.transform.CompareTag("Lower_Shelf") )
                {
                    // Refrence the script to variable
                    Lower_Shelf = hit.transform.GetComponent<Shelf_Script>(); 
                    // Making IsOpen True
                     if(Lower_Shelf.IsOpen)
                     {
                        // First Rotating the Shelf in time duration of 0.5 sec and then moving in x axis in time duration of 1 sec & then making IsOpen false
                        // OnComplete give you the ablity to call next function after previous function is completed
                        hit.transform.DOLocalRotate( new Vector3(0,0,0), .2f).OnComplete( ()=> { hit.transform.DOLocalMoveX ( -0.0060f, 1 ); Lower_Shelf.IsOpen = false; } );
                        ShelfSoundEffect.Play();
                     }
                     else
                     {  // First moving the Shelf in x axis in time duration of 1 sec and then rotating the shelf in time duration of 0.5 sec & then making IsOpen true
                        // OnComplete give you the ablity to call next function after previous function is completed
                        hit.transform.DOLocalMoveX ( -0.026f, 1 ).OnComplete( ()=> hit.transform.DOLocalRotate( new Vector3(0,0,25), .5f).OnComplete( ()=> Lower_Shelf.IsOpen = true ));
                        ShelfSoundEffect.Play();
                     }
                }           

                // Checking Condition - if ray hit the Tag Gameobject(true)
                if( hit.transform.CompareTag("Holder") )
                {   
                    // transforming holdermilk to Holder(tag) gameobject transformation 
                    holderMilk = hit.transform;                    
                }

            }  
        }
        // OnClick Mouse Button
        else if( Input.GetMouseButton(0) )
        {   
            // checking condition
            if(holderMilk)
            {   
                // intializing addNewMilk to true
                addNewMilk = true;
            }
        }
        // OnClick Mouse Button
        else if( Input.GetMouseButtonUp(0) )
        {
            // intializing addNewMilk to false
            addNewMilk = false;
            // intializing holdermilk to null
            holderMilk = null;
        }
    }

    IEnumerator AddNewMilk()
    {
        // to skip a previous frame
        yield return null;
        // Decalaring Gameobject Variable
        GameObject newMilk;
        // Declaring and Intializing Int Variable
        int childIdx = 0;
        while (true)
        {   // to skip a previous frame 
            yield return null;
            // Checking Condition
            if( addNewMilk && holderMilk)
            {
                // Checking Condition
                if(holderMilk && holderMilk.childCount < 4)
                {
                    //updating Variable
                    childIdx = holderMilk.childCount;
                    // Instating a Prefab in Holder
                    newMilk = Instantiate( MilkPrefab, holderMilk);
                    // transforming position in x axis
                    newMilk.transform.localPosition = new Vector3( childIdx *.25f, 0, 0 );
                }
                // Suspends/Stop the coroutine execution for the 0.2 sec
                yield return new WaitForSeconds(AddMilkDelay);
            }
        }
    }
}

