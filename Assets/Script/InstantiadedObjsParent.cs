using UnityEngine;

// Acts as parent transform to objects instantiated during runtime to avoid cluttering scene hierarchy.
public class InstantiadedObjsParent : MonoBehaviour
{
	public void Awake()
	{
		Services.INSTANTIATED_OBJS_PARENT = this;
	}
}
