using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    public class BVCilindroOBB : BoundingVolume
    {
        public Vector3 _centro { get; set; }
        public float _radio { get; set; }
        public float _alto { get; set; }
        public Vector3 _direccion { get; set; } // Unidad, define la orientación del eje del cilindro

        public BVCilindroOBB(Vector3 centro, float radio, float alto, Vector3 direccion)
        {
            _centro = centro;
            _radio = radio;
            _alto = alto;
            _direccion = Vector3.Normalize(direccion); // Aseguramos que sea unitario
        }
    }
}
