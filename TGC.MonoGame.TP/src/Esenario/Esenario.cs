using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;



namespace TGC.MonoGame.TP.src.Esenario
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Esenario 
    {

        // Variables
        private Terreno _terreno;
        private ManageObjetos _manageObjetos;
        private Montana.Montana _montana;

        
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Esenario()
        {
            _manageObjetos = new ManageObjetos();
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, Matrix view, Matrix projection, ContentManager content)
        {
            // Inicializar terreno
            _terreno = new Terreno();
            _terreno.Initialize(graphicsDevice, world, view, projection, content);

            // Crear un pequeño pueblo (casas y cajas)
            for (int x = -50; x <= 50; x += 20)
            {
                for (int z = -50; z <= 50; z += 20)
                {
                    var casa = new Casa.Casa();
                    casa.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content);
                    _manageObjetos.AgregarCasa(casa);

                    var caja = new Casa.Caja();
                    caja.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x + 8, 0, z + 8), view, projection, content);
                    _manageObjetos.AgregarCaja(caja);
                }
            }

            // Crear un bosque (árboles)
            Random random = new Random(0);
            for (int i = 0; i < 500; i++)
            {
                var arbol = new Casa.Arbol1();
                float x = random.Next(-300, 300);
                float z = random.Next(100, 500);
                arbol.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content);
                _manageObjetos.AgregarArbol(arbol);
            }

            // Crear algunas rocas dispersas
            for (int i = 0; i < 120; i++)
            {
                var roca = new Casa.Roca();
                float x = random.Next(-300, 300);
                float z = random.Next(-300, 300);
                roca.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content);
                _manageObjetos.AgregarRoca(roca);
            }
            _montana = new Montana.Montana();
            _montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(-400, 0, 0), view, projection, content);
        }
        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            _terreno.Dibujar(graphicsDevice);
            _manageObjetos.DibujarObjetos(graphicsDevice);
            _montana.Dibujar(graphicsDevice);
        }

        public void AgregarObjeto(Objetos.Objetos objeto)
        {
            _manageObjetos.AgregarObjeto(objeto);
        }

        public void ActualizarCamara(Camara camara){
            _manageObjetos.ActualizarVistaProyeccion(camara.Vista, camara.Proyeccion);
            _terreno.ActualizarVistaProyeccion(camara.Vista,camara.Proyeccion);
            _montana.ActualizarVistaProyeccion(camara.Vista,camara.Proyeccion);
        }

    }

}
