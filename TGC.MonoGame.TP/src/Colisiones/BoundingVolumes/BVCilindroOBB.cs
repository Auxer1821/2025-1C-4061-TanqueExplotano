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

        public Vector3 _Ocentro { get; set; }
        public float _Oradio { get; set; }
        public float _Oalto { get; set; }
        public Vector3 _Odireccion { get; set; } // Unidad, define la orientación del eje del cilindro

        public BVCilindroOBB(Vector3 centro, float radio, float alto, Vector3 direccion)
        {
            _centro = centro;
            _radio = radio;
            _alto = alto;
            _direccion = Vector3.Normalize(direccion); // Aseguramos que sea unitario
            
            _Ocentro = centro;
            _Oradio = radio;
            _Oalto = alto;
            _Odireccion = Vector3.Normalize(direccion); // Aseguramos que sea unitario
        }

        public BVCilindroOBB( float radio, float alto, Vector3 direccion)
        {
            _centro = Vector3.Zero;
            _radio = radio;
            _alto = alto;
            _direccion = Vector3.Normalize(direccion); // Aseguramos que sea unitario
            
            _Ocentro = Vector3.Zero;
            _Oradio = radio;
            _Oalto = alto;
            _Odireccion = Vector3.Normalize(direccion); // Aseguramos que sea unitario
        }

        public override void Transformar(Vector3 nuevaPosicion, Vector3 rotacionEuler, float escala)
        {
            // 1. Aplicar la Escala Uniforme
            // La escala afecta las dimensiones del cilindro: radio y altura.
            this._radio = this._Oradio * escala;
            this._alto = this._alto * escala;

            // 2. Aplicar la Rotación a la Dirección del Cilindro
            // Creamos una matriz de rotación a partir de los ángulos de Euler.
            Matrix rotacionMatrix = Matrix.CreateFromYawPitchRoll(rotacionEuler.Y, rotacionEuler.X, rotacionEuler.Z);

            // La dirección del cilindro debe ser rotada.
            // Asumimos que la `_direccion` original del cilindro (antes de esta transformación específica)
            // es la que queremos rotar. Si las rotaciones son acumulativas sobre la dirección actual,
            // entonces usarías `this._direccion` actual.
            // Si la rotación siempre parte de una orientación base (ej. siempre alineado con Y antes de rotar),
            // necesitarías una `_direccionOriginal` o una `_direccionBase`.
            // Para este ejemplo, asumiremos que la rotación se aplica a la dirección actual.
            // Es importante que `_direccion` sea un vector de solo dirección (sin magnitud > 1),
            // por eso Vector3.TransformNormal es apropiado.
            this._direccion = Vector3.TransformNormal(this._Odireccion, rotacionMatrix);
            // Nos aseguramos de que la dirección siga siendo un vector unitario después de la transformación
            // (aunque TransformNormal con una matriz de rotación pura debería mantener la longitud si era unitaria).
            this._direccion.Normalize();


            // 3. Establecer la Nueva Posición del Centro
            // La posición del centro se actualiza directamente.
            this._centro = nuevaPosicion;
        }

        public override Vector3 GetCentro()
        {
            // Retorna el centro del cilindro
            return _centro;
        }
    }
}
