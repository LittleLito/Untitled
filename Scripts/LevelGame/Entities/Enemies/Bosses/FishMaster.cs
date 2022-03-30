using UnityEngine;

public class FishMaster : MonoBehaviour
{
    public void Init()
    {
        for (var i = 0; i < 10; i++)
        {
            transform.Find($"SGatling{i + 1}").GetComponent<EnemySGatling>().Init();
        }
        Invoke(nameof(SetItalianGunCanAttackTrue), 10);
    }

    private void SetItalianGunCanAttackTrue() 
    {
        transform.Find("FishMaster2x305mmGun1").GetComponent<FishMaster2x305mmGun>().CanAttack = true;
        transform.Find("FishMaster2x305mmGun2").GetComponent<FishMaster2x305mmGun>().CanAttack = true;
    }
}