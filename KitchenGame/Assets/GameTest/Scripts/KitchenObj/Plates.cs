using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Plates : BasicObj
{
    public List<GameObject> Foods = new List<GameObject>();


    private void FixedUpdate()
    {
        PlateUpdate();
    }
    private void PlateUpdate()
    {
        if (Foods.Count > 0)
        {
            for(int i=0;i<Foods.Count;i++)
            {
                if (Foods[i].transform.position.y < this.transform.position.y)
                {
                    Foods[i].transform.parent = null;
                    Foods.Remove(Foods[i]);
                }
                else if(Foods[i].transform.parent != this.transform)
                {
                    Foods.Remove(Foods[i]);
                }
            }
        }
    }

    public override void PickObjs()
    {
        base.PickObjs();
        MouseSFM.Instance.SwitchState(MouseState.HasPlate);
    }

    public override void UseTools(GameObject Obj)
    {
        Obj.transform.DOMove(this.transform.GetChild(0).position, 0.1f).
            OnComplete(()=> {
                Obj.layer = LayerMask.NameToLayer("Foods");
                Obj.transform.parent = this.transform;
                Foods.Add(Obj);
            });
                
    }

    public override void PutObjs()
    {
        MouseSFM.Instance.PickObj.transform.parent = this.transform;
        MouseSFM.Instance.PickObj.transform.DOMove(this.transform.GetChild(0).position, 0.3f).
            OnComplete(() => {
                MouseSFM.Instance.PickObj.GetComponent<Rigidbody>().isKinematic = false;
                MouseSFM.Instance.PickObj.GetComponent<Collider>().enabled = true;
                Foods.Add(MouseSFM.Instance.PickObj);
                MouseSFM.Instance.PickObj = null;          
            });
        MouseSFM.Instance.SwitchState(MouseState.Nothing);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Foods") || collision.gameObject.layer == LayerMask.NameToLayer("CutFoods"))
        {
            if(!Foods.Contains(collision.gameObject))
            {
                collision.transform.parent = this.transform;
                Foods.Add(collision.gameObject);
            }
        }
    }

    /*private void OnCollisionExit(Collision collision)
    {
        if(Foods.Contains(collision.gameObject))
        {
            collision.transform.parent = null;
            Foods.Remove(collision.gameObject);
        }
    }*/
}
