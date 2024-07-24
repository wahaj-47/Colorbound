using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PossesionNotifier : MonoBehaviour
{

    public UnityEvent<int, Color> OnPosses;
    public UnityEvent OnUnPossess;

}
