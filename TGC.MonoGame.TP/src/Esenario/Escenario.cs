using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Entidades;
using TGC.MonoGame.TP.src.Objetos;
using TGC.MonoGame.TP.src.Managers;



namespace TGC.MonoGame.TP.src.Escenarios
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla los managers
    /// </summary>
    public class Escenario 
    {

        // Variables
        private Terrenos.Terreno _terreno;
        private ManagerGrafico _managerGrafico;
        private ManagerColisiones _managerColision;
        private ManagerGameplay _managerGameplay;

        private Camaras.Camara _camara;
        private Entidades.ESkyBox _skyBox;
            //TODO: Que sea el primero en ser dibujado

        
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Escenario()
        {
            _managerGrafico = new ManagerGrafico();
            _managerColision = new ManagerColisiones();
            _managerGameplay = new ManagerGameplay();
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
            //inicializar el skybox
            _skyBox = new Entidades.ESkyBox();
            _skyBox.Initialize(graphicsDevice, world, view, projection, content, this);
            this.AgregarEntidad(_skyBox);

            // Inicializar terreno
            _terreno = new Terrenos.Terreno();
            _terreno.Initialize(graphicsDevice, world, view, projection,  content);
            
            //Posiciones usadas
            List<Vector3> posicionesUsadas = new List<Vector3>();

            // Crear un pequeño pueblo (casas y cajas)
            for (int x = -50; x <= 50; x += 20)
            {
                for (int z = -50; z <= 50; z += 20)
                {
                    var casa = new Entidades.ECasa();
                    casa.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content, this);
                    this.AgregarEntidad(casa);
                    posicionesUsadas.Add(new Vector3(x,z,4));

                    var caja = new Entidades.ECaja();
                    caja.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x + 8, 0, z + 8), view, projection, content, this);
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
                    arbol.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content, this);
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
                    roca.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, 0, z), view, projection, content, this);
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
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(-400, 0, -400 + 200 * i), view, projection, content, this);
                this.AgregarEntidad(montana);
                    //DERECHA
                montana = new Entidades.EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(400, 0, -400 + 200 * i), view, projection, content, this);
                this.AgregarEntidad(montana);
            }

            // crear tanks
            
            var jugador = new Entidades.EJugador();
            float Jx = random.Next(-150, 150);
            float Jz = random.Next(-150, 150);
            var Jpos = new Vector2(Jx,Jz);
        
            while (!PosicionesLibre(Jpos, posicionesUsadas, 10))    //ENCONTRAR UNA POS LIBRE
            {
                Jx = random.Next(-150, 150);
                Jz = random.Next(-150, 150);
                Jpos = new Vector2(Jx, Jz);
            }

            jugador.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Jx, 0, Jz), view, projection, content, this);
            this.AgregarEntidad(jugador);
            this._managerGameplay.AgregarJugador(jugador);
            posicionesUsadas.Add(new Vector3(Jx,Jz,10));
            
            for (int i = 0; i < 4; i++)
            {
                var tank = new Entidades.Etanque();
                float Ax = random.Next(-150, 150);
                float Az = random.Next(-150, 150);
                var pos = new Vector2(Ax, Az);
                if (PosicionesLibre(pos, posicionesUsadas, 10))
                {
                    tank.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Ax, 0, Az), view, projection, content, this);
                    this.AgregarEntidad(tank);
                    this._managerGameplay.AgregarEnemigo(tank);
                    posicionesUsadas.Add(new Vector3(Ax, Az, 10));
                }
                else
                {
                    i--;
                }
            }

        }
        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            _managerGrafico.DibujarObjetos(graphicsDevice);
            _terreno.Dibujar(graphicsDevice);
        }

        public void ActualizarCamara(GameTime gameTime){
            _camara.Actualizar(gameTime);
            _managerGrafico.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
            _terreno.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
        }

        public void Update(GameTime gameTime){
            
            _managerGameplay.Update(gameTime);
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

        internal void EliminarEntidad(Entidad entidad)
        {
            _managerGrafico.RemoverEntidad(entidad);
            _managerColision.RemoverEntidad(entidad);
        }
    }

}
