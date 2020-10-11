using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeminRaycast : MonoBehaviour
{
    private PlaceInfo lastBlock;

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        if (DragDropPlayerController.SelectedDropObject != null)
        {
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                PlaceInfo pi = hit.collider.GetComponent<PlaceInfo>();
                var selection = hit.transform;

                if (pi != null)
                {
                    if (selection.CompareTag("RenkliZeminler") && pi.isEmpty)
                    {
                        if (lastBlock != null)
                        {
                            lastBlock.rend.material = lastBlock.defaultRend;
                            DragDropPlayerController.SelectedCube = null;
                        }

                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            if (DragDropPlayerController.SelectedDropObject != null)
                            {
                                pi.rend.material = pi.dragMaterial;
                            }

                            DragDropPlayerController.SelectedCube = hit.collider.gameObject;
                            DragDropPlayerController.SelectedRow = pi.row;
                            DragDropPlayerController.SelectedColumn = pi.column;
                        }
                        else
                        {
                            lastBlock.rend.material = lastBlock.defaultRend;
                            DragDropPlayerController.SelectedCube = null;
                        }
                        lastBlock = pi;
                    }
                    else
                    {
                        if (lastBlock != null)
                        {
                            lastBlock.rend.material = lastBlock.defaultRend;
                            DragDropPlayerController.SelectedCube = null;
                        }
                    }
                }
                else if (lastBlock != null)
                {
                    lastBlock.rend.material = lastBlock.defaultRend;
                    DragDropPlayerController.SelectedCube = null;
                }
            }
        }
    }
}
