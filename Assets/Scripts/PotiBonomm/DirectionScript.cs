using UnityEngine;

public class DirectionScript : MonoBehaviour
{
    public void SetActiveIfWaiting()
    {


        if (GameManager.Instance.State != GameManager.GameState.WaitingForMakeLaugh)
            return;

        var bonomms = TileManager.Instance.PotiBonommList;
        var directionsOfBonomms = bonomms.ConvertAll(b => b.DirectionScript);
        directionsOfBonomms.ForEach(d => d.gameObject.SetActive(false));

        gameObject.SetActive(true);
    }
}
