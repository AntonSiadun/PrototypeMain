using ProfileOperations;

public class ProfileSavePoint 
{
    private void Start()
    {
        Profile.Instance.Save();
    }    
}