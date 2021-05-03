﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChargeStateData", menuName = "Data/State Data/Charge State")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 6.0f;

    public float chargeTime = 1.0f;
}
