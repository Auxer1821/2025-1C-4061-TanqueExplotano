using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Tanques
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class MTanque : Modelos.Modelo
    {

        //----------------------------------------------Variables--------------------------------------------------//
        private TipoTanque _tipoTanque;
        private Effect effectRueda;
        Texture2D tanqueTexture;
        Texture2D texturaCinta;
        float giroTorrreta = 5f;
        Vector2 rotacionRuedas = new Vector2(0f, 0f);
        Vector2 offsetCintas = new Vector2(0f, 0f);
        string[] meshes;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public MTanque(TipoTanque tipoTanque)
        {
            _tipoTanque = tipoTanque;
        }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.Gray.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//

        protected override void ConfigurarModelo(ContentManager Content)
        {
            this._modelo = Content.Load<Model>("Models/tgc-tanks" + this._tipoTanque.directorioModelo());
            tanqueTexture = Content.Load<Texture2D>("Models/tgc-tanks" + this._tipoTanque.directorioTextura());
            texturaCinta = Content.Load<Texture2D>("Models/tgc-tanks/T90/textures_mod/treadmills");
            effectRueda = Content.Load<Effect>("Effects/shaderRuedas");
            offsetCintas = getAnimacionTanque(0, 0.01f); // esta es la animacion por defecto, 0 = detenido
            rotacionRuedas = getAnimacionTanque(0, 0.1f); // esta es la animacion por defecto, 0 = detenido
            //obtenemos los meshes del modelo
            int count = 0;
            int meshCount = _modelo.Meshes.Count;
            meshes = new string[meshCount];
            foreach (var mesh in _modelo.Meshes)
            {
                if (!string.IsNullOrEmpty(mesh.Name))
                {
                    meshes[count] = mesh.Name;
                    //Console.WriteLine($"Mesh {count}: {mesh.Name}");
                }
                else
                {
                    // Asignar nombre genérico si no tiene
                    mesh.Name = $"Mesh_{count}";
                    meshes[count] = mesh.Name;
                    //Console.WriteLine($"Mesh {count}: {mesh.Name}");
                }
                count++;
            }

        }

        protected override void AjustarModelo()
        {
            this._matixBase = Matrix.CreateScale(this._tipoTanque.escala()) * Matrix.CreateRotationX(this._tipoTanque.angulo()) * Matrix.CreateRotationY(MathF.PI + 2f);//TODO - SOLO ROTA EN X. Si queres hacerlo hermoso, cambiar. Escala de modelo no amerita.
        }

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Texture"].SetValue(tanqueTexture);

            effectRueda.Parameters["View"].SetValue(this._matrixView);
            effectRueda.Parameters["Projection"].SetValue(this._matrixProyection);
            effectRueda.Parameters["World"].SetValue(this._matrixMundo);
            effectRueda.Parameters["Texture"].SetValue(tanqueTexture);

            //---------------------------- variables prueba--------------------------------------------------------
            /*
            // prueba de rotacion de ruedas TODO crear funcion
            rotacionRuedas += new Vector2(1, 1) * 0.1f;

            //prueba de movimiento de la cinta
            offsetCintas.X += 0.01f;
            if (offsetCintas.X > 1.0f)
                offsetCintas.X -= 1.0f;

            offsetCintas.Y = -offsetCintas.X;

            */
            //------------------------------dibujado de los meshes---------------------------------------------------
            //TODO mejorar la lectura del codigo
            foreach (var mesh in _modelo.Meshes)
            {
                if (mesh.Name.Contains("Treadmill"))
                {
                    //desplazamiento de la cinta individual
                    if (mesh.Name.Contains("1"))
                    {
                        effectRueda.Parameters["UVOffset"].SetValue(new Vector2(0, offsetCintas.X));
                    }
                    else
                    {
                        effectRueda.Parameters["UVOffset"].SetValue(new Vector2(0, offsetCintas.Y));
                    }
                    effectRueda.Parameters["Texture"].SetValue(texturaCinta);
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effectRueda;
                    }

                    effectRueda.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);

                }
                else if (mesh.Name == "Turret" || mesh.Name == "Cannon")
                {
                    //posiblemente de problemas al calcular con las normales del mapa
                    _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorrreta) * _matrixMundo);
                }
                else if (mesh.Name.Contains("Wheel"))
                {
                    Vector3 wheelCenter = mesh.BoundingSphere.Center;

                    Matrix transform = mesh.ParentBone.Transform * Matrix.CreateRotationZ(MathF.PI);

                    //giro individual de cada rueda
                    if (esRuedaDerecha(mesh.Name))
                    {
                        transform = Matrix.CreateTranslation(-wheelCenter) *
                          Matrix.CreateRotationX(rotacionRuedas.Y) * // Rotación en X
                          Matrix.CreateTranslation(wheelCenter) *
                          transform;
                    }
                    else
                    {
                        transform = Matrix.CreateTranslation(-wheelCenter) *
                          Matrix.CreateRotationX(rotacionRuedas.X) * // Rotación en X
                          Matrix.CreateTranslation(wheelCenter) *
                          transform;
                    }

                    _effect2.Parameters["World"].SetValue(transform * _matrixMundo);
                }
                else
                {
                    _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                }
                mesh.Draw();
            }
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>("Effects/shaderT90");
        }

        // ajustar con delta time
        public void rotarTorreta(float giro)
        {
            giroTorrreta += giro;
            if (giroTorrreta > MathHelper.PiOver2)
            {
                giroTorrreta = MathHelper.PiOver2;
            }
            else if (giroTorrreta < -MathHelper.PiOver2)
            {
                giroTorrreta = -MathHelper.PiOver2;
            }
        }

        // 0 detenido
        // 1 marcha adelante
        // 2 marcha atras
        // 3 giro a la derecha
        // 4 giro a la izquierda
        public Vector2 getAnimacionTanque(int animacion, float velocidad)
        {
            if (animacion == 0)
            {
                return new Vector2(0, 0);
            }
            else if (animacion == 1)
            {
                return new Vector2(1, 1) * velocidad;
            }
            else if (animacion == 2)
            {
                return new Vector2(-1, -1) * velocidad;
            }
            else if (animacion == 3)
            {
                return new Vector2(1, -1) * velocidad;
            }
            else if (animacion == 4)
            {
                return new Vector2(-1, 1) * velocidad;
            }
            else
            {
                return new Vector2(0, 0);
            }
        }

        public bool esRuedaDerecha(string nombre)
        {
            if (nombre.Contains("9") || nombre.Contains("10") || nombre.Contains("11") || nombre.Contains("12") || nombre.Contains("13") || nombre.Contains("14") || nombre.Contains("15") || nombre.Contains("16"))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}