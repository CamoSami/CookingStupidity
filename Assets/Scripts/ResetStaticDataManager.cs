using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour {

    private void Awake() {
        //  Since static event can get subscribers that have been deleted/destroyed, which will likely cause bugs
        //  Reset them in advance to not cause troubles later
        CuttingCounter.ResetStaticData();

        BaseCounter.ResetStaticData();

        TrashCounter.ResetStaticData();
    }

}
