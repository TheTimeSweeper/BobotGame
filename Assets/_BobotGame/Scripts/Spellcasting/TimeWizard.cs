using SpellCasting.UI;
using UnityEngine;

public class TimeWizard : MonoBehaviour {

    private void Update()
    {
#if UNITY_EDITOR
        Tim();
#endif
    }

    private void Tim() {
        //time keys
        if (Input.GetKeyDown(KeyCode.I)) {
            if (TimeStopperInstance.UnpausedTime == 0) {
                setTimeScale(TimeStopperInstance.UnpausedTime + 0.1f);
            } else {
                setTimeScale(TimeStopperInstance.UnpausedTime + 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) {

            setTimeScale(TimeStopperInstance.UnpausedTime - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            setTimeScale(1);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            setTimeScale(0);
        }
    }

    private void setTimeScale(float tim) {
        TimeStopperInstance.UnpausedTime = tim;

        Debug.Log($"set tim: {TimeStopperInstance.UnpausedTime}");
        TimeStopperInstance.OnUpdasteInstanceList();
    }
}
