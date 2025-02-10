# Fill_the_Fridge_Replica 🥛🚪

![Fill_the_Fridge_Replica](https://user-images.githubusercontent.com/62818241/204718605-c319b63d-3c07-4168-9da0-76837868b5cb.PNG)

## 📌 Introduction
**Fill_the_Fridge_Replica** is an interactive game that simulates filling a fridge with milk cartons. The game involves opening and closing fridge doors and shelves, where the player can place milk cartons in different spots. The game features smooth animations for object transformations, leveraging the DoTween package for seamless position and rotation transitions.

🔗 Video Trailer

https://youtube.com/shorts/PhOjPi6cUlk?si=96MTg4_pFkewVlb9

## 🔥 Features
- 🥛 **Milk Carton Interaction**: Players can interact with fridge shelves to add milk cartons.
- 🚪 **Door Animation**: Fridge doors can be opened and closed with smooth animations.
- 🧑‍🍳 **Shelf Mechanics**: Shelves can be opened or closed, providing space for milk cartons.
- 🎶 **Sound Effects**: Interactive sound effects for opening the fridge door and shelves.
- ✨ **DoTween Integration**: Smooth, animated transformations for game objects (position and rotation).

---

## 🏗️ How It Works

This game is controlled through simple interactions like clicking on fridge doors and shelves to open them and placing milk cartons on the shelves. The transformations of the fridge door, shelves, and milk cartons are handled using DoTween for smooth animations.

### 📌 **Fridge Script**

The **Fridge_Script** is the main script that manages fridge door opening and shelf actions. It handles the logic for opening and closing the fridge door, as well as placing new milk cartons on the shelves.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fridge_Script : MonoBehaviour
{
    public GameObject MilkPrefab;
    public float AddMilkDelay = 0.2f;
    private bool addNewMilk = false;
    private Transform holderMilk;
    private bool Door;
    Shelf_Script Upper_Shelf;
    Shelf_Script Lower_Shelf;
    [SerializeField] private AudioSource DoorSoundEffect;
    [SerializeField] private AudioSource ShelfSoundEffect;

    private void Start() {
        addNewMilk = false;
        StartCoroutine( AddNewMilk() );
    }

    void Update() {  
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.CompareTag("Door")) {
                    HandleDoor(hit.transform);
                }

                if (hit.transform.CompareTag("Upper_Shelf")) {
                    HandleShelf(hit.transform, ref Upper_Shelf);
                }

                if (hit.transform.CompareTag("Lower_Shelf")) {
                    HandleShelf(hit.transform, ref Lower_Shelf);
                }

                if( hit.transform.CompareTag("Holder") ) {
                    holderMilk = hit.transform;                    
                }
            }  
        }

        else if (Input.GetMouseButton(0)) {   
            if(holderMilk) {   
                addNewMilk = true;
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            addNewMilk = false;
            holderMilk = null;
        }
    }

    private void HandleDoor(Transform door) {
        if (Door == false) {
            door.DOLocalRotate(new Vector3(0, -100f, 0), 1f).OnComplete(() => { Door = true; });
            DoorSoundEffect.Play();
        } else {
            door.DOLocalRotate(new Vector3(0, 0f, 0), 1f).OnComplete(() => { Door = false; });
            DoorSoundEffect.Play();
        }
    }

    private void HandleShelf(Transform shelf, ref Shelf_Script shelfScript) {
        shelfScript = shelf.GetComponent<Shelf_Script>(); 

        if (shelfScript.IsOpen) {
            shelf.DOLocalRotate(new Vector3(0,0,0), 0.5f).OnComplete(() => { shelf.DOLocalMoveX(-0.0052f, 1); shelfScript.IsOpen = false; });
            ShelfSoundEffect.Play();
        } else {
            shelf.DOLocalMoveX(-0.026f, 1).OnComplete(() => shelf.DOLocalRotate(new Vector3(0, 0, 25), 0.5f).OnComplete(() => shelfScript.IsOpen = true));
            ShelfSoundEffect.Play();
        }
    }

    IEnumerator AddNewMilk() {
        yield return null;
        GameObject newMilk;
        int childIdx = 0;

        while (true) {
            yield return null;
            if( addNewMilk && holderMilk) {
                if(holderMilk && holderMilk.childCount < 4) {
                    childIdx = holderMilk.childCount;
                    newMilk = Instantiate(MilkPrefab, holderMilk);
                    newMilk.transform.localPosition = new Vector3(childIdx * 0.25f, 0, 0);
                }
                yield return new WaitForSeconds(AddMilkDelay);
            }
        }
    }
}
```

### 📌 **Shelf Script**

This script manages the opening and closing state of the shelves. The `IsOpen` property tracks whether the shelf is currently open or closed.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf_Script : MonoBehaviour
{
   public bool IsOpen;
}
```

### 📌 **UI Script**

The **UI** script controls the game's start and quit actions. It handles loading the game scene and quitting the application.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
```

---

## 🎯 Conclusion
The **Fill_the_Fridge_Replica** game provides a fun, interactive experience with smooth animations and a focus on object interactions. By utilizing DoTween, it offers a fluid and responsive experience for players. The mechanics of the fridge door, shelves, and milk carton placement are straightforward, making it an ideal project for learning basic game interactions and animation handling in Unity. 🥛🎮
