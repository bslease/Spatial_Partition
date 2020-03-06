using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    // The soldier base class for enemies and friendlies
    public class Soldier
    {
        public MeshRenderer soldierMeshRenderer;
        public Transform soldierTrans;
        protected float walkSpeed;

        // connect soldiers in a linked list so we can easily partition them in space
        public Soldier previousSoldier;
        public Soldier nextSoldier;

        public virtual void Move() { }
        public virtual void Move(Soldier soldier) { }
    }
}