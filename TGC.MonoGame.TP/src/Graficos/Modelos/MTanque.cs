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

        public enum Animacion
        {
            Detenido,
            MarchaAdelante,
            MarchaAtras,
            giroDer,
            giroIzq
        }

        //----------------------------------------------Variables--------------------------------------------------//
        private TipoTanque _tipoTanque;
        private Effect effectRueda;
        Texture2D tanqueTexture;
        Texture2D texturaCinta;
        float giroTorreta = 0f;
        float alturaTorreta = 0f;
        Vector2 rotacionRuedas = new Vector2(0f, 0f);
        Vector2 offsetCintas = new Vector2(0f, 0f);
        float modificadorDanio = 1.0f;
        string[] meshes;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public MTanque(TipoTanque tipoTanque)
        {
            _tipoTanque = tipoTanque;
        }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            ActualizarColor(Color.Gray);
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//

        protected override void ConfigurarModelo(ContentManager Content)
        {
            this._modelo = Content.Load<Model>("Models/tgc-tanks" + this._tipoTanque.directorioModelo());
            tanqueTexture = Content.Load<Texture2D>("Models/tgc-tanks" + this._tipoTanque.directorioTextura());
            texturaCinta = Content.Load<Texture2D>("Models/tgc-tanks/T90/textures_mod/treadmills");
            effectRueda = Content.Load<Effect>("Effects/shaderRuedas");
            offsetCintas = getAnimacionTanque(Animacion.Detenido, 0.01f,offsetCintas); // esta es la animacion por defecto, 0 = detenido
            rotacionRuedas = getAnimacionTanque(Animacion.Detenido, 0.1f,rotacionRuedas); // esta es la animacion por defecto, 0 = detenido
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
            this._matixBase = Matrix.CreateScale(this._tipoTanque.escala()) * Matrix.CreateRotationX(this._tipoTanque.angulo().X) * Matrix.CreateRotationY(this._tipoTanque.angulo().Y) * Matrix.CreateRotationZ(this._tipoTanque.angulo().Z) * Matrix.CreateTranslation(new Vector3(0,1,0)); ;
        }

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Texture"].SetValue(tanqueTexture);
            _effect2.Parameters["Opaco"].SetValue(modificadorDanio);

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
                    _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta - MathHelper.PiOver2)  * _matrixMundo);
                    Matrix transform = mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta - MathHelper.PiOver2) * _matrixMundo;
                    if (mesh.Name == "Cannon")
                    {
                        _effect2.Parameters["World"].SetValue(Matrix.CreateRotationX(-alturaTorreta - MathHelper.PiOver2 + 1f) * transform);
                    }
                    
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
            giroTorreta += giro;
            
        }

        // 0 detenido
        // 1 marcha adelante
        // 2 marcha atras
        // 3 giro a la derecha
        // 4 giro a la izquierda
        public Vector2 getAnimacionTanque(Animacion animacion, float velocidad, Vector2 resultado)
        {
            switch (animacion)
            {
                case Animacion.Detenido:
                    //Que no haga nada así mantiene el valor
                    break;
                case Animacion.MarchaAdelante:
                    resultado += new Vector2(1, 1) * velocidad;
                    break;
                case Animacion.MarchaAtras:
                    resultado += new Vector2(1, 1) * velocidad;
                    break;
                case Animacion.giroDer:
                    resultado += new Vector2(-1, 1) * velocidad;
                    break;
                case Animacion.giroIzq:
                    resultado += new Vector2(1, -1) * velocidad;
                    break;
                default:
                    resultado = new Vector2(0, 0);
                    break;
            }
            return resultado;
        }

        public bool esRuedaDerecha(string nombre)
        {
            return nombre.Contains("9") || nombre.Contains("10") || nombre.Contains("11") || nombre.Contains("12") || nombre.Contains("13") || nombre.Contains("14") || nombre.Contains("15") || nombre.Contains("16");
            
        }


        internal override void EfectoDaño(float porcentajeVida)
        {
            modificadorDanio = porcentajeVida;
        }

        public void ActualizarMovimeinto(float velocidad, float angulo){//update de entidad
            Animacion animacion = Animacion.Detenido;
            if(angulo>0) animacion= Animacion.giroDer;
            else if(angulo<0) animacion=Animacion.giroIzq;
            else if(velocidad>0) animacion=Animacion.MarchaAdelante;
            else if(velocidad<0) animacion=Animacion.MarchaAtras;
            offsetCintas = getAnimacionTanque(animacion, velocidad, offsetCintas); // esta es la animacion por defecto, 0 = detenido
            rotacionRuedas = getAnimacionTanque(animacion, velocidad, rotacionRuedas); // esta es la animacion por defecto, 0 = detenido
        }

        internal void ActualizarTorreta(Vector2 dirMovimiento, Vector3 dirApuntado)
        {
            //throw new NotImplementedException();
            if (dirMovimiento != Vector2.Zero)
                dirMovimiento.Normalize();
            // Calcular ángulo deseado en el plano horizontal (XZ)
            float anguloDeseadoHorizontal = (float)Math.Atan2(dirApuntado.X, dirApuntado.Z);

            // Aplicar el ángulo a la torreta
            giroTorreta = anguloDeseadoHorizontal;
            alturaTorreta = dirApuntado.Y;

            // Limitar el rango de movimiento de la torreta
            if (alturaTorreta > MathHelper.PiOver2/2)
                alturaTorreta = MathHelper.PiOver2;
            else if (alturaTorreta < -MathHelper.PiOver2)
                alturaTorreta = -MathHelper.PiOver2;

            // Limitar el rango de movimiento de la torreta
            if (giroTorreta > MathHelper.Pi)
                giroTorreta = MathHelper.Pi;
            else if (giroTorreta < -MathHelper.Pi)
                giroTorreta = -MathHelper.Pi;
        }
    }
}