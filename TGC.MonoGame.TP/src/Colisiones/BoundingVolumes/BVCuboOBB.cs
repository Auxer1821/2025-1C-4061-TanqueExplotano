using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    public class BVCuboOBB : BoundingVolume
    {
        public Vector3 Centro;
        public Vector3 Tamaño; // Half extents (es decir, la mitad del tamaño en cada eje)
        public Matrix Orientacion; // Rotación

        public BVCuboOBB(Vector3 centro, Vector3 tamaño, Matrix orientacion)
        {
            Centro = centro;
            Tamaño = tamaño;
            Orientacion = orientacion;
        }
    }
}
