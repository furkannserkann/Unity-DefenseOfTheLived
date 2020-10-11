using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragObjects : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject createObject;
    private Vector3 DefaultPosition;


    private void Start()
    {
        DefaultPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragDropPlayerController.SelectedDropObject = createObject;
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 50, Input.mousePosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = DefaultPosition;

        if (DragDropPlayerController.SelectedCube != null && DragDropPlayerController.SelectedDropObject != null)
        {
            PlaceInfo localPlaceInfo = DragDropPlayerController.SelectedCube.GetComponent<PlaceInfo>();
            if (localPlaceInfo != null)
            {
                localPlaceInfo.isEmpty = false;
                localPlaceInfo.rend.material = localPlaceInfo.defaultRend;
            }

            var IPlayer = Instantiate(DragDropPlayerController.SelectedDropObject, DragDropPlayerController.SelectedCube.transform.position, Quaternion.identity);
            IPlayer.transform.rotation = DragDropPlayerController.SelectedDropObject.transform.rotation;
            IPlayer.transform.parent = GameObject.FindGameObjectWithTag("Parent_Hero").transform;

            PlayerController lPlayerController = IPlayer.GetComponent<PlayerController>();
            if (lPlayerController != null)
            {
                lPlayerController.WayNumber = DragDropPlayerController.SelectedRow;
                lPlayerController.SelectedCreateCube = DragDropPlayerController.SelectedCube;
            }
        }

        DragDropPlayerController.SelectedCube = null;
        DragDropPlayerController.SelectedDropObject = null;
    }
}
