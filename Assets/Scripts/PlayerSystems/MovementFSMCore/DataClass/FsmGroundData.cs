﻿using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/MovementData/GroundData")]
    public class FsmGroundData : FsmData
    {
        public float friction;
        public float jumpHeldFriction;
    }
}