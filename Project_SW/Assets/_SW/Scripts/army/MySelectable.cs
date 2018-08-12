using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class MySelectable : MonoBehaviour, ISelectHandler, IPointerClickHandler, IDeselectHandler {

    public static HashSet<MySelectable> allMySelectables = new HashSet<MySelectable>();
    public static HashSet<MySelectable> currentlySelected = new HashSet<MySelectable>();
    public HashSet<MySelectable> enemySelected = new HashSet<MySelectable>();

    Renderer myRenderer;
    public int ID;
    [SerializeField]
    GameObject unselectedGameObject;
    //Material unselectedMaterial;
    [SerializeField]
    GameObject selectedGameObject;


    public enum UnitStatus
    {
        Idle,
        Run,
        Attack

    }
    public int campNum;
    public UnitStatus status;
    public Animator ani;
    public Transform targets;
    public float attack_radius;
    public Vector3 move_point;
    public float move_speed;
    RaycastHit hit;
    public StartServer startServer;

    public GameObject bullet;
    public float bullet_speed;
    
    public void Fire()
    {
        GameObject b = Instantiate(bullet, transform.position + transform.forward  + transform.up*0.5f , transform.rotation);
        b.transform.LookAt(targets.position);
        b.GetComponent<Rigidbody>().AddForce(b.transform.forward * bullet_speed);
        if (Vector3.Distance(transform.position, targets.transform.position) > attack_radius)
        {

        }
    }
    void Awake()
    {
        allMySelectables.Add(this);
        startServer = GameObject.Find("Game").GetComponent<StartServer>();
        if (campNum == DragSelectionHandler.mycamp)
        {
            
        }
        else
        {
            
        }

        //myRenderer = GetComponent<Renderer>();
    }
    void Start()
    {
        
        foreach (MySelectable selectable in allMySelectables)
        {
            Debug.Log(selectable.GetInstanceID().ToString());
            if (selectable.campNum != campNum)
            {
                enemySelected.Add(selectable);
            }

        }

        ani = GetComponent<Animator>();
    }


    public void UnitMove(Vector3 point,Vector3 pos)
    {
        move_point = point;
        transform.position = pos;
        status = UnitStatus.Run;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentlySelected.Contains(this))
        {
            
            if (Input.GetMouseButtonDown(1))
            {
                
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                   
                    if (hit.transform.tag == "Unit")
                    {
                        targets = hit.transform;
                        status = UnitStatus.Attack;
                    }
                    else
                    {
                        JSONObject json = new JSONObject();


                        json.AddField("Type", "Move");
                        json.AddField("ObjectID", ID);
                        json.AddField("Point", hit.point.ToString());
                        json.AddField("Pos", transform.position.ToString());
                        
                        
                        
                        startServer.onData_toServer(json);
                       // move_point = hit.point;
                       // status = UnitStatus.Run;
                    }

                }
            }
        }

        switch (status)
        {
            case UnitStatus.Idle:
                ani.SetFloat("Run", 0);
                foreach (MySelectable selectable in enemySelected)
                {
                    if (Vector3.Distance(transform.position,selectable.transform.position)<attack_radius)
                    {
                        targets = selectable.transform;
                        status = UnitStatus.Attack;
                    }
                }
                break;
            case UnitStatus.Run:
                ani.SetFloat("Run", 1);
                transform.LookAt(move_point);
                transform.position += transform.forward * move_speed * Time.deltaTime;
                if (Vector3.Distance(transform.position, move_point) < 0.08)
                {
                    status = UnitStatus.Idle;
                    move_point = Vector3.zero;
                }
                break;
            case UnitStatus.Attack:
                transform.LookAt(targets.position);
                ani.SetFloat("Run", -1);
                break;

        }
    }

    


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            DeselectAll(eventData);
        }
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (campNum == DragSelectionHandler.mycamp)
        {

        }
        currentlySelected.Add(this);
        selectedGameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedGameObject.SetActive(false);
    }

    public static void DeselectAll (BaseEventData eventData)
    {
        foreach(MySelectable selectable in currentlySelected)
        {
            selectable.OnDeselect(eventData);
        }
        currentlySelected.Clear();
    }
}
