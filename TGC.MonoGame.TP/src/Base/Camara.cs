using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.src.Camaras
{
    public class Camara
    {
        public Vector3 Posicion { get; private set; }
        public Vector3 Objetivo { get; private set; }
        public Vector3 Arriba { get; private set; } = Vector3.Up;
        public float Velocidad { get; set; } = 100f;

        public Matrix Vista { get; private set; }
        public Matrix Proyeccion { get; private set; }

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//

        public Camara(Vector3 posicionInicial, Vector3 objetivoInicial, float relacionAspecto)
        {
            Posicion = posicionInicial;
            Objetivo = objetivoInicial + new Vector3(0, -10, 0);
            ActualizarVista();
            Proyeccion = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, relacionAspecto, 0.1f, 1000f);
        }

        //----------------------------------------------Update--------------------------------------------------//

        public void Actualizar(GameTime gameTime)
        {
            var teclado = Keyboard.GetState();
            var direccionMovimiento = Vector3.Zero;

            // Dirección hacia adelante (normalizada)
            var frente = Vector3.Normalize(Objetivo - Posicion);
            var derecha = Vector3.Normalize(Vector3.Cross(frente, Arriba));

            if (teclado.IsKeyDown(Keys.I))
                direccionMovimiento += frente;
            if (teclado.IsKeyDown(Keys.K))
                direccionMovimiento -= frente;
            if (teclado.IsKeyDown(Keys.J))
                direccionMovimiento -= derecha;
            if (teclado.IsKeyDown(Keys.L))
                direccionMovimiento += derecha;
            if (teclado.IsKeyDown(Keys.O))
                direccionMovimiento += Arriba;
            if (teclado.IsKeyDown(Keys.U))
                direccionMovimiento -= Arriba;

            if (direccionMovimiento != Vector3.Zero)
            {
                direccionMovimiento.Normalize();
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Posicion += direccionMovimiento * Velocidad * delta;
                Objetivo += direccionMovimiento * Velocidad * delta;
            }

            ActualizarVista();
        }

        private void ActualizarVista()
        {
            Vista = Matrix.CreateLookAt(Posicion, Objetivo, Arriba);
        }
    }
}
