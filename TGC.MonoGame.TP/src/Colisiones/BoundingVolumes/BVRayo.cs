using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase de esfera para los bounding Volume.
    /// </summary>
    public class BVRayo : BoundingsVolumes.BoundingVolume
    {

        // Variables
        public Vector3 _Direccion { get; set; }
        public Vector3 _PuntoPartda { get; set; }

        public Vector3 _ODireccion { get; set; }
        public Vector3 _OPuntoPartda { get; set; }

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
        public BVRayo(Vector3 Direccion, Vector3 PuntoPartda)
        {

            _Direccion = Vector3.Normalize(Direccion);
            _PuntoPartda = PuntoPartda;

            _ODireccion = _Direccion;
            _OPuntoPartda = _PuntoPartda;
        }

        public BVRayo(Vector3 Direccion)
        {

            _Direccion = Vector3.Normalize(Direccion);
            _PuntoPartda = Vector3.Zero;

            _ODireccion = _Direccion;
            _OPuntoPartda = _PuntoPartda;
        }

        public override void Transformar(Vector3 nuevaPosicionPartida, Vector3 rotacionEuler, float escala)
        {
            // 1. Aplicar la Rotación a la Dirección del Rayo
            // Creamos una matriz de rotación a partir de los ángulos de Euler.
            Matrix rotacionMatrix = Matrix.CreateFromYawPitchRoll(rotacionEuler.Y, rotacionEuler.X, rotacionEuler.Z);

            // Rotamos la dirección actual del rayo.
            this._Direccion = Vector3.TransformNormal(this._ODireccion, rotacionMatrix);


            // Aseguramos que la dirección siga siendo un vector unitario.
            this._Direccion.Normalize();

            // 2. Establecer el Nuevo Punto de Partida
            // La nueva posición se asigna directamente al punto de partida del rayo.
            this._PuntoPartda = nuevaPosicionPartida;

            // 3. Escala:
            // La escala no tiene un efecto. Se ignora.

        }

        public override Vector3 GetCentro()
        {
            return this._PuntoPartda;
        }



        //----------------------------------------------Funciones-de-Detección--------------------------------------------------// 


    }
}