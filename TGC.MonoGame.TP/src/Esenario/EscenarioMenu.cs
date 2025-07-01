using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.HUD;
using TGC.MonoGame.TP.src.Entidades;
using TGC.MonoGame.TP.src.Tanques;
using TGC.MonoGame.TP.src.Graficos.Temporales;



namespace TGC.MonoGame.TP.src.Escenarios
{
    /// <summary>
    ///     Esta es la clase que controla los diferentes esenarios como en esenario de juego y el menu
    /// </summary>
    public class EscenarioMenu : IEscenario
    {

        //----------------Variables---------------------//
        private GraphicsDevice _graphicsDevice;
        private HUD.HImagen _TituloJuego;
        private HUD.HImagen _botonTanqueAnt;
        private HUD.HImagen _botonTanqueSig;
        private HUD.HImagen _fondo;

        //------ botones ---
        private BotonMenuJugar _botonJugar;
        private BotonMenuOpciones _botonOpciones;
        private BotonMenuSalir _botonSalir;
        private IBotonMenu _botonElecto;
        private float _velocidadDeGiro;
        
         private MTanque _tanque;
        private string _texturaTanque = "2";
        private float Rotation = 0.0f;
        private float _tiempoDeCambio = 1.0f;
        private DirectorEscenarios _dEsenarios;
        private Managers.ManagerSonido _managerSonido;


        //---------------Metodos--------------------------//

        public EscenarioMenu()
        {

        }

        public void Initialize(GraphicsDevice device, ContentManager Content, DirectorEscenarios directorEscenarios)
        {
            _graphicsDevice = device;
            _dEsenarios = directorEscenarios;

            this._TituloJuego = new HImagen();
            this._TituloJuego.Initialize(new Vector2(0.0f, 0.7f), Content, "Textures/ui/cooltext483425277740560");
            this._TituloJuego.setQuad(0.4f, device);


            this._botonJugar = new BotonMenuJugar();
            this._botonJugar.CargarImagen(device, Content, "Textures/ui/cooltext483425676931020", new Vector2(0.0f, -0.2f));
            this._botonJugar.Inicializar(_dEsenarios);

            this._botonOpciones = new BotonMenuOpciones();
            this._botonOpciones.CargarImagen(device, Content, "Textures/ui/cooltext483425703672866", new Vector2(0.0f, -0.5f));
            this._botonOpciones.Inicializar(_dEsenarios);

            this._botonSalir = new BotonMenuSalir();
            this._botonSalir.CargarImagen(device, Content, "Textures/ui/cooltext483425720746578", new Vector2(0.0f, -0.8f));
            this._botonSalir.Inicializar(_dEsenarios.GetGame());



            this._botonJugar.CargarBotones(this._botonSalir, this._botonOpciones);
            this._botonOpciones.CargarBotones(this._botonJugar, this._botonSalir);
            this._botonSalir.CargarBotones(this._botonOpciones, this._botonJugar);

            this._botonElecto = this._botonJugar;
            this._botonJugar.Electo(_graphicsDevice);



            this._botonTanqueSig = new HImagen();
            this._botonTanqueSig.Initialize(new Vector2(0.3f, 0.1f), Content, "Textures/ui/cooltext483425751817305");
            this._botonTanqueSig.setQuad(0.05f, device);

            this._botonTanqueAnt = new HImagen();
            this._botonTanqueAnt.Initialize(new Vector2(-0.3f, 0.1f), Content, "Textures/ui/cooltext483425762595384");
            this._botonTanqueAnt.setQuad(0.05f, device);

            this._fondo = new HImagen();
            this._fondo.Initialize(new Vector2(0f, 0f), Content, "Textures/ui/fondo3");
            this._fondo.setQuad(1f, device);
            this._fondo.cambioDeTecnica("Fondo");


            Matrix World = Matrix.Identity;
            _tanque = new MTanque(new TanqueT90());
            _tanque.Initialize(device, World, Content);
            Matrix View = Matrix.CreateLookAt(Vector3.UnitZ * 50, Vector3.Zero, Vector3.Up);
            Matrix Projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1, 250);
            _tanque.EfectCamera(View, Projection);
            //setear la iluminacion
            _tanque.setCamara(new Vector3(0, 0, 0));
            _tanque.SetPosSOL(new Vector3(-30, 30, 10));


            this._velocidadDeGiro = 1;

            this._managerSonido = new Managers.ManagerSonido(Content);
            this._managerSonido.InstanciarSonidosMenu();

        }
        public void Update(GameTime gameTime)
        {
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) *this._velocidadDeGiro;

            _tanque.ActualizarMatrizMundo(Matrix.CreateScale(0.9f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(0, -1f, 0));


            if (!this.PuedeCambiarBoton()){
                //--Corta para que no cambie por tick--//
                this._tiempoDeCambio -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
        
                Environment.Exit(0);
            }

            // Botones del menu
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.CambiarBoton(_botonElecto.Up());
                this._managerSonido.ReproducirSonidoMenu("cambioBoton");
                this._tiempoDeCambio =0.25f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.CambiarBoton(_botonElecto.Down());
                this._managerSonido.ReproducirSonidoMenu("cambioBoton");
                this._tiempoDeCambio =0.25f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.CambiarTextura("sig");
                this._managerSonido.ReproducirSonidoMenu("cambioTanque");
                this._tiempoDeCambio =0.25f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.CambiarTextura("ant");
                this._managerSonido.ReproducirSonidoMenu("cambioTanque");
                this._tiempoDeCambio =0.25f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                this._managerSonido.ReproducirSonidoMenu("selecccion");
                this._botonElecto.Enter();
                this._tiempoDeCambio =0.25f;
            }

        }

        private bool PuedeCambiarBoton()
        {
            return this._tiempoDeCambio <=0;
        }

        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.Black);
            //_textoEffect.Parameters["View"].SetValue(Matrix.Identity);
            //_textoEffect.Parameters["Projection"].SetValue(Matrix.Identity);
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _fondo.Dibujado(graphicsDevice);


            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            _tanque.CambiarTecnica("Menu");
            _tanque.Dibujar(graphicsDevice);
            _tanque.CambiarTecnica("Main");
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _TituloJuego.Dibujado(_graphicsDevice);

            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            _botonJugar.Dibujar(_graphicsDevice);
            _botonOpciones.Dibujar(_graphicsDevice);
            _botonSalir.Dibujar(_graphicsDevice);


            _botonTanqueSig.Dibujado(_graphicsDevice);
            _botonTanqueAnt.Dibujado(_graphicsDevice);

        }
        
        private void CambiarTextura(string movimiento){
            if (movimiento == "sig")
            {
                if (_texturaTanque == "1")
                {
                    _texturaTanque = "2";
                }
                else if (_texturaTanque == "2")
                {
                    _texturaTanque = "3";
                }
                else if (_texturaTanque == "3")
                {
                    _texturaTanque = "1";
                }
            }
            else if (movimiento == "ant")
            {
                if (_texturaTanque == "1")
                {
                    _texturaTanque = "3";
                }
                else if (_texturaTanque == "2")
                {
                    _texturaTanque = "1";
                }
                else if (_texturaTanque == "3")
                {
                    _texturaTanque = "2";
                }
            }

            _tanque.CambiarTexturaT90(_texturaTanque);
            _dEsenarios.SetSkinTanque(_texturaTanque);
        }

        void CambiarBoton(IBotonMenu botonNuevo){
            this._botonElecto.NoElecto(_graphicsDevice);
            this._botonElecto = botonNuevo;
            this._botonElecto.Electo(_graphicsDevice);
        }
       
        
    }
}




