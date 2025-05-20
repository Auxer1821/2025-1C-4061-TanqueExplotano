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
        private List<Entidad> _entidadesEliminar;
        private bool _faltaEliminar;
        private List<Entidad> _entidadesCrear;
        private bool _faltaCrear;

        private Cameras.FreeCamera _camara;
        //TODO: Que sea el primero en ser dibujado
        private Entidades.ESkyBox _skyBox; //TODO -> Actualizar en el manager Graficos para que se dibujen primeros / ultimos
        private Entidades.EPasto[] pastos = new Entidades.EPasto[100];

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Escenario()
        {
            _managerGrafico = new ManagerGrafico();
            _managerColision = new ManagerColisiones();
            _managerGameplay = new ManagerGameplay();
            _entidadesEliminar = new List<Entidad>();
            _entidadesCrear = new List<Entidad>();
            _faltaEliminar = false;
            _faltaCrear = false;
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, ContentManager content)
        {
            var screenSize = new Point(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            _camara = new Cameras.FreeCamera(graphicsDevice.Viewport.AspectRatio, Vector3.UnitZ * 150, screenSize);
            Matrix view = _camara.Vista;
            Matrix projection = _camara.Proyeccion;

            //-----------------Inicializar el skybox-------------------//
            _skyBox = new ESkyBox();
            _skyBox.Initialize(graphicsDevice, world, view, projection, content, this);
            this.AgregarACrear(_skyBox);

            //-----------------Inicializar terreno--------------------//
            _terreno = new Terrenos.Terreno();
            _terreno.Initialize(graphicsDevice, world, view, projection, content);

            //-----------------Posiciones usadas----------------------//
            List<Vector3> posicionesUsadas = new List<Vector3>();

            //-------Crear un pequeño pueblo (casas y cajas)-----------//
            for (int x = -50; x <= 50; x += 20)
            {
                for (int z = -50; z <= 50; z += 20)
                {
                    var casa = new ECasa();
                    casa.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x,z), z), view, projection, content, this, new Vector3 (x, _terreno.GetHeightAt(x,z), z));
                    this.AgregarACrear(casa);
                    posicionesUsadas.Add(new Vector3(x, z, 4));

                    var caja = new ECaja();
                    caja.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x + 8, _terreno.GetHeightAt(x,z), z + 8), view, projection, content, this);
                    this.AgregarACrear(caja);
                    posicionesUsadas.Add(new Vector3(x + 8, z + 8, 2));
                }
            }

            //--------Crear un bosque (árboles)---------------//
            Random random = new Random(0);
            for (int i = 0; i < 500; i++)
            {
                var arbol = new EArbol();
                float x = random.Next(-300, 300);
                float z = random.Next(100, 500);
                var pos = new Vector2(x, z);
                if (PosicionesLibre(pos, posicionesUsadas, 1))
                {
                    arbol.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x,z), z), view, projection, content, this);
                    this.AgregarACrear(arbol);
                    posicionesUsadas.Add(new Vector3(x, z, 1));
                }
                else
                {
                    i--;
                }
            }

            //-------Crear algunas rocas dispersas----------//
            for (int i = 0; i < 120; i++)
            {
                var roca = new ERoca();
                float x = random.Next(-300, 300);
                float z = random.Next(-300, 300);
                var pos = new Vector2(x, z);
                if (PosicionesLibre(pos, posicionesUsadas, 1))
                {
                    roca.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x,z), z), view, projection, content, this);
                    this.AgregarACrear(roca);
                    posicionesUsadas.Add(new Vector3(x, z, 1));
                }
                else
                {
                    i--;
                }
            }

            //------Crear pasto--------------------//
            for (int i = 0; i < 100; i++)
            {
                var pasto = new Entidades.EPasto();
                float x = random.Next(-300, 300);
                float z = random.Next(-300, 300);
                var pos = new Vector2(x,z); 
                 if(PosicionesLibre(pos, posicionesUsadas,1)){
                    pasto.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x,z), z), view, projection, content, this);
                    pastos[i] = pasto;
                    posicionesUsadas.Add(new Vector3(x,z,1));
                }
                else
                {
                    i--;
                }

            }

            //---------Crear Coordillera--------//            
            for (int i = 0; i < 5; i++)
            {
                //IZQUIERDA
                var montana = new EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(-400, 0, -400 + 200 * i), view, projection, content, this);
                this.AgregarACrear(montana);
                //DERECHA
                montana = new EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(400, 0, -400 + 200 * i), view, projection, content, this);
                this.AgregarACrear(montana);
            }

            //-------------------Crear tanks--------------------//
            //---Jugador---//
            var jugador = new EJugador();
            float Jx = random.Next(-150, 150);
            float Jz = random.Next(-150, 150);
            var Jpos = new Vector2(Jx, Jz);

            while (!PosicionesLibre(Jpos, posicionesUsadas, 10))    //ENCONTRAR UNA POS LIBRE
            {
                Jx = random.Next(-150, 150);
                Jz = random.Next(-150, 150);
                Jpos = new Vector2(Jx, Jz);
            }

            jugador.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Jx, 0f, Jz), view, projection, content, this);
            jugador.setCamara(_camara);
            this.AgregarACrear(jugador);
            this._managerGameplay.AgregarJugador(jugador);
            posicionesUsadas.Add(new Vector3(Jx, Jz, 10));

            //----IA---//
            
            for (int i = 0; i < 4; i++)
            {
                var tank = new ETanqueIA();
                float Ax = random.Next(-150, 150);
                float Az = random.Next(-150, 150);
                var pos = new Vector2(Ax, Az);
                if (PosicionesLibre(pos, posicionesUsadas, 10))
                {
                    tank.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Ax, _terreno.GetHeightAt(Ax, Az), Az), view, projection, content, this);
                    this.AgregarACrear(tank);
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
            //pasto debe ser dibujado al final por la transparencia
            foreach (var pasto in pastos)
            {
                if (pasto != null)
                {
                    pasto.Dibujar(graphicsDevice);
                }
            }
        }

        public void ActualizarCamara(GameTime gameTime){
            _camara.Update(gameTime);
            _managerGrafico.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
            _terreno.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
            //TODO pasto dentro de manager grafico
            foreach (var pasto in pastos)
            {
                if (pasto != null)
                {
                    pasto.ActualizarVistaProyeccion(_camara.Vista, _camara.Proyeccion);
                }
            }
        }

        public void AgregarAEliminar(Entidad entidad)
        {
            this._entidadesEliminar.Add(entidad);
            this._faltaEliminar = true;
        }

        public void AgregarACrear(Entidad entidad)
        {
            this._entidadesCrear.Add(entidad);
            this._faltaCrear = true;
        }

        public void Update(GameTime gameTime)
        {

            _managerGameplay.Update(gameTime);
            _managerColision.VerificarColisiones();
            this.ActualizarCamara(gameTime);
            if (_faltaEliminar) this.EliminarEntidades();
            if (_faltaCrear) this.CrearEntidades();

        }


        //----------------------------------Funciones-auxiliares--------------------------------------------//
        private bool PosicionesLibre(Vector2 newPos, List<Vector3> usadas, float distancia)
        {
            foreach (var pos in usadas)
            {
                if (Vector2.Distance(new Vector2(pos.X, pos.Y), newPos) < (distancia + pos.Z))
                    return false;
            }
            return true;
        }

        internal void EliminarEntidades()
        {
            foreach (var entidad in _entidadesEliminar)
            {
                this._managerGrafico.RemoverEntidad(entidad);
                this._managerColision.RemoverEntidad(entidad);
                this._managerGameplay.RemoverEntidad(entidad);
            }
            this._entidadesEliminar.Clear();
            this._faltaEliminar = false;
        }

        internal void CrearEntidades()
        {
            foreach (var entidad in _entidadesCrear)
            {
                this._managerGrafico.AgregarEntidad(entidad);
                this._managerColision.AgregarEntidad(entidad);
                this._managerGameplay.AgregarEntidad(entidad);
            }
            this._entidadesCrear.Clear();
            this._faltaCrear = false;
        }

        internal float getAltura(Vector3 pos)
        {
            return _terreno.GetHeightAt(pos.X, pos.Z);
        }
    }

}
