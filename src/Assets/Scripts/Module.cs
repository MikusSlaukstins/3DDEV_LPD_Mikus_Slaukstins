using UnityEngine;

public class Module : MonoBehaviour
{
	public string[] Tags;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag == "corridor" || col.gameObject.tag  == "room" || col.gameObject.tag == "junction")
        //{
            Destroy(this);
        //}
    }
}
