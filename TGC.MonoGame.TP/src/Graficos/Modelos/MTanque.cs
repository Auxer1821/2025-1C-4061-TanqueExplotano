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
        Texture2D tanqueTexture;
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
        
        protected override void AjustarModelo(){
            this._matixBase = Matrix.CreateScale(this._tipoTanque.escala()) * Matrix.CreateRotationX(this._tipoTanque.angulo()) * Matrix.CreateRotationY(MathF.PI) ;//TODO - SOLO ROTA EN X. Si queres hacerlo hermoso, cambiar. Escala de modelo no amerita.
        }

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Texture"].SetValue(tanqueTexture);

            foreach (var mesh in _modelo.Meshes)
            {
                _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                mesh.Draw();
            }
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>("Effects/shaderT90");
        }

    }
}