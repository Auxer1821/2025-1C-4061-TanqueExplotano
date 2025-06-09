using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using TGC.MonoGame.TP.src.HUD;
using System.Linq;



namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class ManagerInterfaz 
    {
        private Entidades.EJugador _jugador;

        //------------Htexto---------------//
        private HTexto _vida;
        private HTexto _progreso;
        private HTexto _tiempo;

        private Effect _textoEffect;

        //-------------HImagen--------------//
        private List<HImagen> _imagenesHud;
        private HImagen _misil;

        //buffers para dibujar el HUD letra
        private ContentManager _Content;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private GraphicsDevice _graphicsDevice;
        /*
            IMPLEMETACION DE LISTA IMAGENES HUD
            COMPLATR DEL DRAW
        */
        public ManagerInterfaz()
        {
            //_jugador = new Entidades.EJugador();
            _vida = new HTexto();
            _progreso = new HTexto();
            _tiempo = new HTexto();
            _imagenesHud = new List<HImagen>();
            _misil = new HImagen();
        }

        public void Inicializar(GraphicsDevice device, ContentManager Content, Entidades.EJugador jugador)
        {
            _Content = Content;
            _jugador = jugador;
            _graphicsDevice = device;

            _textoEffect = Content.Load<Effect>(@"Effects/shaderInterfazTexto");//todo
            Texture2D texture = Content.Load<Texture2D>(@"Textures/ui/CaracteresNegros");
            _textoEffect.Parameters["Texture"].SetValue(texture);
            _textoEffect.Parameters["TamanioTextura"]?.SetValue(texture.Width);
            _textoEffect.Parameters["PixelSize"].SetValue(1.0f / texture.Width);
            _textoEffect.Parameters["WorldViewProjection"]?.SetValue(Matrix.Identity);

            _vida.Initialize(new Vector2(-0.9f, 0.9f));
            _vida.setValor("vida:" + ((int)_jugador.getVida()).ToString());

            _progreso.Initialize(new Vector2(0.5f, 0.9f));
            _progreso.setValor("Kills:0/5");

            _tiempo.Initialize(new Vector2(-0.1f, 0.9f));
            _tiempo.setValor("00:00");


            HImagen mira = new HImagen();
            mira.Initialize(new Vector2(0.0f, 0.0f), Content, "Textures/miras/aim_PNG56");
            mira.setQuad(0.05f,device);
            _imagenesHud.Add(mira);

            _misil.Initialize(new Vector2(-0.9f, -0.9f), Content, "Textures/misiles/R");
            _misil.setQuad(0.10f,device);
            _misil.cambioDeTecnica("Recarga");
            //_imagenesHud.Add(mira);

            
            
            
            this.crearQuad();

        }

        public void Dibujar(){
            //_textoEffect.Parameters["View"].SetValue(Matrix.Identity);
            //_textoEffect.Parameters["Projection"].SetValue(Matrix.Identity);
            _vida.Dibujado(_graphicsDevice, _textoEffect, _indexBuffer, _vertexBuffer);
            _progreso.Dibujado(_graphicsDevice, _textoEffect, _indexBuffer, _vertexBuffer);
            _tiempo.Dibujado(_graphicsDevice, _textoEffect, _indexBuffer, _vertexBuffer);
            _misil.Dibujado(_graphicsDevice);

            foreach (HImagen hImagen in _imagenesHud)
            {
                hImagen.Dibujado(_graphicsDevice);
            }
        }

        public void Update()
        {
            _vida.setValor("vida:" + ((int)_jugador.getVida()).ToString());
            _progreso.setValor( (int)_jugador.GetKills() + "/3");//TODO
            float mseg = this._jugador.tiempoRestante();
            int minuto = (int) mseg / 60;
            int seg = (int) mseg % 60;
            _tiempo.setValor(minuto.ToString() + ":" + seg.ToString());//DIFERENCIA DE TIMES O ALGO
            
            float porcentajeRecargado = _jugador.porcentajeRecargado();
            _misil.setClaridad(porcentajeRecargado);
            
        }

        //------Funcion para crear donde se dibujan las texturas----//
        private void crearQuad()
        {
            // 1. Definir los vértices (4 vértices para un quad)
            var vertices = new VertexPositionNormalTexture[4];

            // Tamaño del quad (1 unidad de ancho y alto)
            float halfSize = 0.025f;

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