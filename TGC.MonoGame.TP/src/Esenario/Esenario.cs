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
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Esenario 
    {

        // Variables
        private Terrenos.Terreno _terreno;
        private Entidades.ESkyBox _skyBox;
        private ManagersObjetos.ManagerObjetos _managerObjetos;

        
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Esenario()
        {
            _managerObjetos = new ManagersObjetos.ManagerObjetos();
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, Matrix view, Matrix projection, ContentManager content)
        {
            //inicializar el skybox
            _skyBox = new Entidades.ESkyBox();
            _skyBox.Initialize(graphicsDevice, world, view, projection, content);
            _managerObjetos.AgregarEntidadGrafica(_skyBox);

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
                    this.AgregarEntidadFull(casa);
                    posicionesUsadas.Add(new Vector3(x,z,4));

                    var caja = new Entidades.ECaja();
                    caja.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x + 8, 0, z + 8), view, projection, content);
                    this.AgregarEntidadFull(caja);
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
                    this.AgregarEntidadFull(arbol);
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
                    this.AgregarEntidadFull(roca);
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
                this.AgregarEntidadFull(montana);
                    //DERECHA
                montana = new Entidades.EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(400, 0, -400 + 200 * i), view, projection, content);
                this.AgregarEntidadFull(montana);
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
                    this.AgregarEntidadFull(tank);
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
            _managerObjetos.DibujarObjetos(graphicsDevice);
            _terreno.Dibujar(graphicsDevice);
        }


        public void AgregarEntidadFull(Entidades.EntidadFull eFull)
        {
            _managerObjetos.AgregarEntidadFull(eFull);
        }
        public void AgregarEntidadGrafica(Entidades.EntidadGrafica eGrafica)
        {
            _managerObjetos.AgregarEntidadGrafica(eGrafica);
        }
        public void AgregarEntidadColicion(Entidades.EntidadColision eColicion)
        {
            _managerObjetos.AgregarEntidadColicion(eColicion);
        }


        public void ActualizarCamara(Camaras.Camara camara){
            _managerObjetos.ActualizarVistaProyeccion(camara.Vista, camara.Proyeccion);
            _terreno.ActualizarVistaProyeccion(camara.Vista,camara.Proyeccion);
        }

        private bool PosicionesLibre( Vector2 newPos, List<Vector3> usadas, float distancia){
            foreach (var pos in usadas){
                if(Vector2.Distance(new Vector2(pos.X,pos.Y), newPos) < (distancia+pos.Z) )
                return false;
            }
            return true;
        }

    }

}
