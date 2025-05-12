using UnityEngine;

public class IceWall : MonoBehaviour
{
    public void Melt()
    {
        // Optional: play melt animation or particle effect here
        Destroy(gameObject);
    }
}
