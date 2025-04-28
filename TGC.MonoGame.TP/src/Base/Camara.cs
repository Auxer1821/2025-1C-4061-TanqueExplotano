using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Camara
    {
        public Vector3 Posicion { get; private set; }
        public Vector3 Objetivo { get; private set; }
        public Vector3 Arriba { get; private set; } = Vector3.Up;
        public float Velocidad { get; set; } = 50f;

        public Matrix Vista { get; private set; }
        public Matrix Proyeccion { get; private set; }

        public Camara(Vector3 posicionInicial, Vector3 objetivoInicial, float relacionAspecto)
        {
            Posicion = posicionInicial;
            Objetivo = objetivoInicial + new Vector3(0, -10, 0);
            ActualizarVista();
            Proyeccion = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 
                relacionAspecto, 
                0.1f, 
                1000f
            );
        }

        public void Actualizar(GameTime gameTime)
        {
            var teclado = Keyboard.GetState();
            var direccionMovimiento = Vector3.Zero;

            // Dirección hacia adelante (normalizada)
            var frente = Vector3.Normalize(Objetivo - Posicion);
            var derecha = Vector3.Normalize(Vector3.Cross(frente, Arriba));

            if (teclado.IsKeyDown(Keys.W))
                direccionMovimiento += frente;
            if (teclado.IsKeyDown(Keys.S))
                direccionMovimiento -= frente;
            if (teclado.IsKeyDown(Keys.A))
                direccionMovimiento -= derecha;
            if (teclado.IsKeyDown(Keys.D))
                direccionMovimiento += derecha;

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
