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

        public Vector3 _OCentro;
        public Vector3 _OTamaño; // Half extents (es decir, la mitad del tamaño en cada eje)
        public Matrix _OOrientacion; // Rotación

        public BVCuboOBB(Vector3 centro, Vector3 tamaño, Matrix orientacion)
        {
            Centro = centro;
            Tamaño = tamaño;
            Orientacion = orientacion;

            _OCentro = Centro;
            _OTamaño = Tamaño;
            _OOrientacion = Orientacion;
        }

        public BVCuboOBB(Vector3 tamaño, Matrix orientacion)
        {
            Centro = Vector3.Zero;
            Tamaño = tamaño;
            Orientacion = orientacion;

            _OCentro = Centro;
            _OTamaño = Tamaño;
            _OOrientacion = Orientacion;
        }

        public override void Transformar(Vector3 nuevaPosicion, Vector3 rotacionEuler, float escala)
        {
            // 1. Aplicar la Escala Uniforme al Tamaño (Half Extents)
            // La escala se aplica directamente al tamaño del OBB.
            
            this.Tamaño = _OTamaño * escala;

            // 2. Crear la Matriz de Rotación a partir de los ángulos de Euler
            // El orden de las rotaciones puede ser importante (ej. ZYX, XYZ, etc.).
            // XNA/MonoGame comúnmente usa ZYX si construyes con CreateFromYawPitchRoll.
            /*
            Matrix rotacionX = Matrix.CreateRotationX(rotacionEuler.X);
            Matrix rotacionY = Matrix.CreateRotationY(rotacionEuler.Y);
            Matrix rotacionZ = Matrix.CreateRotationZ(rotacionEuler.Z);
            */
            // Combina las rotaciones. El orden importa. Un orden común es ZYX.
            // this.Orientacion = rotacionZ * rotacionY * rotacionX;
            // this.Orientacion = Matrix.CreateFromYawPitchRoll(rotacionEuler.Y, rotacionEuler.X, rotacionEuler.Z);
            // Escoge la que se ajuste a cómo interpretas tus rotacionesEuler (Yaw, Pitch, Roll vs X, Y, Z directo)
            this.Orientacion = Matrix.CreateFromYawPitchRoll(rotacionEuler.Y, rotacionEuler.X, rotacionEuler.Z);


            // 3. Establecer la Nueva Posición del Centro
            // La posición del centro se actualiza directamente.
            this.Centro = nuevaPosicion;
        }
    }
}
