using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Definitions
{
    public struct ObstacleDefinition
    {
        public string Asset;
        public Vector2 Position;           
        public Rectangle Collider;         
        public bool AutoScaleToCollider;   
        public Vector2 DrawOffset;         
        public float UniformScaleIfNotAuto;
    }
}
