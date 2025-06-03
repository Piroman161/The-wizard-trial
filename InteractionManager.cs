using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoverdWeapon = null;
    public Throwable hoverdThrowable = null;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;


            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoverdWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoverdWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject); 
                }
            }
            else
            {
                if (hoverdWeapon)
                {
                    hoverdWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                hoverdThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoverdThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupThrowable(hoverdThrowable);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoverdThrowable)
                {
                    hoverdThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
