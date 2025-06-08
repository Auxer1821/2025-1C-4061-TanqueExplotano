using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.HUD;
using TGC.MonoGame.TP.src.Entidades;
using TGC.MonoGame.TP.src.Tanques;
using TGC.MonoGame.TP.src.Graficos.Temporales;
using System.Collections.Generic;



namespace TGC.MonoGame.TP.src.Escenarios
{
    /// <summary>
    ///     Esta es la clase que controla los diferentes esenarios como en esenario de juego y el menu
    /// </summary>
    public class EscenarioDerrota : IEscenario
    {

        //----------------Variables---------------------//
        private GraphicsDevice _graphicsDevice;
        private HUD.HImagen _imagenDerrota;
        private HUD.HImagen _fondo;
        private HUD.HTexto _mensajeMotivacional;
        private HUD.HTexto _mensajeVolverMenu;//press enter to return to the menu
        private DirectorEscenarios _dEsenarios;
        private Managers.ManagerSonido _managerSonido;
        private List<string> _mensajes;
        private Effect _textoEffect;
        private IndexBuffer _indexBuffer;
        private VertexBuffer _vertexBuffer;



        //----------------Metodos--------------------------//

        public EscenarioDerrota()
        {

        }

        public void Initialize(GraphicsDevice device, ContentManager Content, DirectorEscenarios directorEscenarios)
        {
            _graphicsDevice = device;
            _dEsenarios = directorEscenarios;
            this.CrearQuad();

            this._fondo = new HImagen();
            this._fondo.Initialize(new Vector2(0f, 0f), Content, "Textures/ui/fondo1");
            this._fondo.setQuad(1f, device);
            this._fondo.cambioDeTecnica("Fondo");

            this._imagenDerrota = new HImagen();
            this._imagenDerrota.Initialize(new Vector2(0.0f, 0.7f), Content, "Textures/ui/Derrota");
            this._imagenDerrota.setQuad(0.25f, device);

            _textoEffect = Content.Load<Effect>(@"Effects/shaderInterfazTexto");//todo
            Texture2D texture = Content.Load<Texture2D>(@"Textures/ui/CaracteresNegros");
            _textoEffect.Parameters["Texture"].SetValue(texture);
            _textoEffect.Parameters["TamanioTextura"]?.SetValue(texture.Width);
            _textoEffect.Parameters["PixelSize"].SetValue(1.0f / texture.Width);
            _textoEffect.Parameters["WorldViewProjection"]?.SetValue(Matrix.Identity);
            

            this._mensajes = new List<string>();
            this._mensajes.Add("/LEVANTATE SOLDADO ESTO NO HA TERMINADO");
            this._mensajes.Add("/VUELVE A INTENTARLO");
            this._mensajes.Add("/NO PUEDES RENDIRTE AHORA");
            this._mensajes.Add("/BOCCHI THE ROCK!!");

            
            
            this._mensajeMotivacional = new HTexto();
            this._mensajeMotivacional.Initialize(new Vector2 (-0.95f, 0.3f),
                this._mensajes[(int)new Random().Next(0, _mensajes.Count - 1)]);

            
            this._mensajeVolverMenu = new HTexto();
            this._mensajeVolverMenu.Initialize(new Vector2(-0.95f, -0.1f), "/PULSA ESPACIO PARA VOLVER AL MENU");

            this._managerSonido = new Managers.ManagerSonido(Content);
            this._managerSonido.InstanciarSonidosMenu();

        }
        public void Update(GameTime gameTime)
        {
            

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
        
                Environment.Exit(0);
            }

            // Botones del menu

            //if (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Space))
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                this._managerSonido.ReproducirSonidoMenu("selecccion");
                this._dEsenarios.CambiarEsenarioActivo(Escenarios.TipoEsenario.Menu);
            }

        }

        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.Black);
            //_textoEffect.Parameters["View"].SetValue(Matrix.Identity);
            //_textoEffect.Parameters["Projection"].SetValue(Matrix.Identity);
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _fondo.Dibujado(graphicsDevice);
            _imagenDerrota.Dibujado(graphicsDevice);
            _mensajeMotivacional.Dibujado(_graphicsDevice, _textoEffect, _indexBuffer, _vertexBuffer);
            _mensajeVolverMenu.Dibujado(_graphicsDevice, _textoEffect, _indexBuffer, _vertexBuffer);
            graphicsDevice.DepthStencilState = DepthStencilState.Default;


        }

        private void CrearQuad()
        {
            // 1. Definir los vértices (4 vértices para un quad)
            var vertices = new VertexPositionNormalTexture[4];

            // Tamaño del quad (1 unidad de ancho y alto)
            float halfSize = 0.02f;

            // Coordenadas de los vértices (en sentido horario)
            vertices[0] = new VertexPositionNormalTexture(
                new Vector3(-halfSize, -halfSize, 0), // Posición (inferior izquierda)
                Vector3.UnitZ,                        // Normal (apuntando hacia la cámara)
                new Vector2(0, 1));                   // Coordenadas UV

            vertices[1] = new VertexPositionNormalTexture(
                new Vector3(-halfSize, halfSize, 0),  // Superior izquierda
                Vector3.UnitZ,
                new Vector2(0, 0));

            vertices[2] = new VertexPositionNormalTexture(
                new Vector3(halfSize, -halfSize, 0),  // Inferior derecha
                Vector3.UnitZ,
                new Vector2(1, 1));

            vertices[3] = new VertexPositionNormalTexture(
                new Vector3(halfSize, halfSize, 0),   // Superior derecha
                Vector3.UnitZ,
                new Vector2(1, 0));

            // 2. Crear VertexBuffer
            _vertexBuffer = new VertexBuffer(
                _graphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly);

            _vertexBuffer.SetData(vertices);

            // 3. Definir índices (2 triángulos = 6 índices)
            var indices = new short[6];

            // Primer triángulo (inferior izquierda, superior izquierda, inferior derecha)
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            // Segundo triángulo (superior izquierda, superior derecha, inferior derecha)
            indices[3] = 1;
            indices[4] = 3;
            indices[5] = 2;

            // 4. Crear IndexBuffer
            _indexBuffer = new IndexBuffer(
                _graphicsDevice,
                IndexElementSize.SixteenBits,
                indices.Length,
                BufferUsage.WriteOnly);

            _indexBuffer.SetData(indices);
        }
       
        
    }
}




