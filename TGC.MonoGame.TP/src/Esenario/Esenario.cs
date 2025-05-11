using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;



namespace TGC.MonoGame.TP.src.Esenarios
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla los managers
    /// </summary>
    public class Esenario 
    {

        // Variables
        private Terrenos.Terreno _terreno;
        private Managers.ManagerGrafico _managerGrafico;
        private Managers.ManagerColisiones _managerColision;

        private Camaras.Camara _camara;

        
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Esenario()
        {
            _managerGrafico = new Managers.ManagerGrafico();
            _managerColision = new Managers.ManagerColisiones();
        }

        public void AgregarEntidad(Entidades.Entidad entidad){
            _managerGrafico.AgregarEntidad(entidad);
            _managerColision.AgregarEntidad(entidad);
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, ContentManager content)
        {
            _camara = new Camaras.Camara(Vector3.UnitZ * 150, Vector3.Zero , graphicsDevice.Viewport.AspectRatio);
            Matrix view = _camara.Vista;
            Matrix projection = _camara.Proyeccion;

            // Inicializar terreno
            _terreno = new Terrenos.Terreno();
            _terreno.Initialize(graphicsDevice, world, view, projection, content);

            //Posiciones usadas
            List<Vector3> posicionesUsadas = new List<Vector3>();

            // Crear un pequeño pueblo (casas y cajas)
            for (int x = -50; x <= 50; x += 20)
            {
                for (int z = -50; z <= 50; z += 20)
                {
                    var casa = new Entidades.ECasa();
                    casa.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content);
                    this.AgregarEntidad(casa);
                    posicionesUsadas.Add(new Vector3(x,z,4));

                    var caja = new Entidades.ECaja();
                    caja.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x + 8, 0, z + 8), view, projection, content);
                    this.AgregarEntidad(caja);
                    posicionesUsadas.Add(new Vector3(x+8,z+8,2));
                }
            }

            // Crear un bosque (árboles)
            Random random = new Random(0);
            for (int i = 0; i < 500; i++)
            {
                var arbol = new Entidades.EArbol();
                float x = random.Next(-300, 300);
                float z = random.Next(100, 500);
                var pos = new Vector2(x,z); 

                if(PosicionesLibre(pos, posicionesUsadas, 1)){
                    arbol.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content);
                    this.AgregarEntidad(arbol);
                    posicionesUsadas.Add(new Vector3(x,z,1));
                }
                else
                {
                    i--;
                }
            }

            // Crear algunas rocas dispersas
            for (int i = 0; i < 120; i++)
            {
                var roca = new Entidades.ERoca();
                float x = random.Next(-300, 300);
                float z = random.Next(-300, 300);
                var pos = new Vector2(x,z); 
                 if(PosicionesLibre(pos, posicionesUsadas,1)){
                    roca.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content);
                    this.AgregarEntidad(roca);
                    posicionesUsadas.Add(new Vector3(x,z,1));
                }
                else
                {
                    i--;
                }
            }

            // Crear Coordillera
            for(int i = 0; i< 5; i++){
                //IZQUIERDA
                var montana = new Entidades.EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(-400, 0, -400 + 200 * i), view, projection, content);
                this.AgregarEntidad(montana);
                    //DERECHA
                montana = new Entidades.EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(400, 0, -400 + 200 * i), view, projection, content);
                this.AgregarEntidad(montana);
            }
            
            // crear tanks
            
            for (int i = 0; i < 5; i++)
            {
                var tank = new Entidades.Etanque();
                float Ax = random.Next(-150, 150);
                float Az = random.Next(-150, 150);
                var pos = new Vector2(Ax,Az); 
                if(PosicionesLibre(pos, posicionesUsadas, 10)){
                    tank.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Ax, 0, Az), view, projection, content);
                    this.AgregarEntidad(tank);
                    posicionesUsadas.Add(new Vector3(Ax,Az,10));
                }
                else
                {
                    i--;
                }
            }

        }
        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            _terreno.Dibujar(graphicsDevice);
            _managerGrafico.DibujarObjetos(graphicsDevice);
        }

        public void ActualizarCamara(GameTime gameTime){
            _camara.Actualizar(gameTime);
            _managerGrafico.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
            _terreno.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
        }

        public void Update(GameTime gameTime){
            
            //manager gamplay
            _managerColision.VerificarColisiones();
            this.ActualizarCamara(gameTime);
        }


        //----------------------------------Funciones-auxiliares--------------------------------------------//
        private bool PosicionesLibre( Vector2 newPos, List<Vector3> usadas, float distancia){
            foreach (var pos in usadas){
                if(Vector2.Distance(new Vector2(pos.X,pos.Y), newPos) < (distancia+pos.Z) )
                return false;
            }
            return true;
        }

    }

}
