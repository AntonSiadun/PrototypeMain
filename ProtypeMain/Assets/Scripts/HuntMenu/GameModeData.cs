using UnityEngine;

public class GameModeData : MonoBehaviour
{
    private void Start()
    {
        ProfileDataSave.Save(Profile.Instance);
    }
}
