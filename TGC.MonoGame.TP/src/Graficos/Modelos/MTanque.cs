using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Modelos;
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
            giroIzq,
            giroSobreEjeDer,
            giroSobreEjeIzq
        }

        //----------------------------------------------Variables--------------------------------------------------//
        private TipoTanque _tipoTanque;
        Texture2D tanqueTexture;
        Texture2D texturaCinta;
        Texture2D texturaNormalTanque;
        Texture2D texturaNormalCinta;
        float giroTorreta = 0f;
        float alturaTorreta = 0.1f;
        Vector2 rotacionRuedas = new Vector2(0f, 0f);
        Vector2 offsetCintas = new Vector2(0f, 0f);
        float modificadorDanio = 1.0f;
        ContentManager Content;
        String _tecnica = "Main";

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public MTanque(TipoTanque tipoTanque)
        {
            _tipoTanque = tipoTanque;
        }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content)
        {
            base.Initialize(Graphics, Mundo, Content);
            this.GuardarVertices();
            /*
                //deformacion de prueba
                this.DeformModel(Vector3.UnitX , 2f , 1f);
                this.DeformModel(Vector3.UnitZ * 1 , 2f , 1f);
                this.DeformModel(Vector3.UnitY * 3 , 2f , 1f);
            */

        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//

        protected override void ConfigurarModelo(ContentManager Content)
        {
            this.Content = Content;
            this._modelo = this.Content.Load<Model>(@"Models/tgc-tanks" + this._tipoTanque.directorioModelo());
            tanqueTexture = this.Content.Load<Texture2D>(@"Models/tgc-tanks" + this._tipoTanque.directorioTextura());

            texturaCinta = this.Content.Load<Texture2D>(@"Models/tgc-tanks" + this._tipoTanque.directorioTexturaCinta());
            texturaNormalTanque = this.Content.Load<Texture2D>(@"Models/tgc-tanks" + this._tipoTanque.directorioTexturaNormal());
            texturaNormalCinta = this.Content.Load<Texture2D>(@"Models/tgc-tanks" + this._tipoTanque.directorioTexturaCintaNormal());
            offsetCintas = getAnimacionTanque(Animacion.Detenido, 0.01f, offsetCintas); // esta es la animacion por defecto, 0 = detenido
            rotacionRuedas = getAnimacionTanque(Animacion.Detenido, 0.1f, rotacionRuedas); // esta es la animacion por defecto, 0 = detenido

            //seteo del efecto
            //luz
            this._effect2.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._effect2.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._effect2.Parameters["specularColor"]?.SetValue(Color.White.ToVector3());
            this._effect2.Parameters["KAmbient"]?.SetValue(0.6f);
            this._effect2.Parameters["KDiffuse"]?.SetValue(2.5f);
            this._effect2.Parameters["KSpecular"]?.SetValue(0.5f);
            this._effect2.Parameters["shininess"]?.SetValue(16.0f);



        }


        protected override void AjustarModelo()
        {
            if (this._tipoTanque.directorioModelo().Contains("T90"))
            {
                this._matixBase = Matrix.CreateScale(this._tipoTanque.escala()) * Matrix.CreateRotationX(this._tipoTanque.angulo().X) * Matrix.CreateRotationY(this._tipoTanque.angulo().Y) * Matrix.CreateRotationZ(this._tipoTanque.angulo().Z) * Matrix.CreateTranslation(new Vector3(0, 1 * 3, 0));
            }
            else
            {
                this._matixBase = Matrix.CreateScale(this._tipoTanque.escala()) * Matrix.CreateRotationX(this._tipoTanque.angulo().X) * Matrix.CreateRotationY(this._tipoTanque.angulo().Y) * Matrix.CreateRotationZ(this._tipoTanque.angulo().Z) * Matrix.CreateTranslation(new Vector3(0, 0.5f, 0));
            }
        }


        //----------------------------------------------Dibujado--------------------------------------------------//
        #region Dibujar
        #endregion
        public override void Dibujar(GraphicsDevice Graphics)
        {
            //this.DeformarModelo();
            this.setDeformacion();
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Opaco"]?.SetValue(modificadorDanio);

            // Setear las texturas (necesarias ya que al tener varios tanques estas deben cambiar)
            _effect2.Parameters["Texture"]?.SetValue(tanqueTexture);
            _effect2.Parameters["TextureCinta"]?.SetValue(texturaCinta);
            _effect2.Parameters["TextureNormalTanque"]?.SetValue(texturaNormalTanque);
            _effect2.Parameters["TextureNormalCinta"]?.SetValue(texturaNormalCinta);


            //------------------------------dibujado de los meshes---------------------------------------------------
            //TODO mejorar la lectura del codigo
            if (this._tipoTanque.directorioModelo().Contains("T90"))
            {
                foreach (var mesh in _modelo.Meshes)
                {
                    _effect2.CurrentTechnique = _effect2.Techniques[_tecnica];

                    if (mesh.Name.Contains("Treadmill"))
                    {
                        //setear el efecto de la cinta
                        _effect2.CurrentTechnique = _effect2.Techniques["RuedasDrawing"];
                        //desplazamiento de la cinta individual
                        if (mesh.Name.Contains("1"))
                        {
                            _effect2.Parameters["UVOffset"].SetValue(new Vector2(0, offsetCintas.X));
                        }
                        else
                        {
                            _effect2.Parameters["UVOffset"].SetValue(new Vector2(0, offsetCintas.Y));
                        }
                        Matrix MundoShader = mesh.ParentBone.Transform * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);

                    }
                    else if (mesh.Name == "Turret" || mesh.Name == "Cannon")
                    {
                        Matrix MundoShader = mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta) * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta) * _matrixMundo);
                        Matrix transform = mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta) * _matrixMundo;
                        if (mesh.Name == "Cannon")
                        {
                            MundoShader = Matrix.CreateRotationX(-alturaTorreta - 0.3f) * transform;
                            _effect2.Parameters["World"].SetValue(MundoShader);
                            _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                            //_effect2.Parameters["World"].SetValue(Matrix.CreateRotationX(-alturaTorreta - 0.3f) * transform);
                        }

                    }
                    else if (mesh.Name.Contains("Wheel"))
                    {
                        Vector3 wheelCenter = mesh.BoundingSphere.Center;

                        Matrix transform = mesh.ParentBone.Transform * Matrix.CreateRotationZ(MathF.PI);

                        //giro individual de cada rueda
                        if (esRuedaDerechaT90(mesh.Name))
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
                        Matrix MundoShader = transform * Matrix.CreateTranslation(0.15f, 0, 0) * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(transform * Matrix.CreateTranslation(0.15f,0,0) * _matrixMundo);// Ajuste de la posición de las ruedas
                    }
                    else
                    {
                        Matrix MundoShader = mesh.ParentBone.Transform * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                    }
                    mesh.Draw();
                }
            }
            else
            {
                foreach (var mesh in _modelo.Meshes)
                {

                    _effect2.CurrentTechnique = _effect2.Techniques["Main"];
                    //logica para el tanque Panzer
                    if (mesh.Name.Contains("Treadmill"))
                    {
                        //setear el efecto de la cinta
                        _effect2.CurrentTechnique = _effect2.Techniques["RuedasDrawing"];
                        //desplazamiento de la cinta individual
                        if (mesh.Name.Contains("1"))
                        {
                            _effect2.Parameters["UVOffset"].SetValue(new Vector2(0, -offsetCintas.X));
                        }
                        else
                        {
                            _effect2.Parameters["UVOffset"].SetValue(new Vector2(0, -offsetCintas.Y));
                        }
                        Matrix MundoShader = mesh.ParentBone.Transform * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);

                    }
                    else if (mesh.Name == "Turret")
                    {
                        Matrix MundoShader = mesh.ParentBone.Transform * Matrix.CreateRotationY(giroTorreta) * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * Matrix.CreateRotationY(giroTorreta) * _matrixMundo);
                    }
                    else if (mesh.Name == "Cannon")
                    {
                        //por alguna razon el cannon eran muy pequeño
                        Matrix MundoShader = mesh.ParentBone.Transform * Matrix.CreateRotationY(giroTorreta) * Matrix.CreateScale(100f) * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * Matrix.CreateRotationY(giroTorreta) * Matrix.CreateScale(100f) * _matrixMundo);
                    }
                    else if (mesh.Name.Contains("Wheel"))
                    {
                        Vector3 wheelCenter = mesh.BoundingSphere.Center;

                        //por alguna razon las ruedas eran muy pequeño
                        Matrix transform = mesh.ParentBone.Transform *
                                           Matrix.CreateScale(100f);

                        //giro individual de cada rueda
                        if (esRuedaDerechaPanzer(mesh.Name))
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
                        Matrix MundoShader = transform * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(transform * _matrixMundo);
                    }
                    else
                    {
                        Matrix MundoShader = mesh.ParentBone.Transform * _matrixMundo;
                        _effect2.Parameters["World"].SetValue(MundoShader);
                        _effect2.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                        //_effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                    }

                    mesh.Draw();
                }
            }
            this.ResetDeformation();
        }

        public void DibujarShadowMap(GraphicsDevice Graphics, Matrix vista, Matrix proyeccion)
        {
            _effect2.CurrentTechnique = _effect2.Techniques["DepthPass"];
           //this.DeformarModelo();
            this.setDeformacion();

            foreach (var mesh in _modelo.Meshes)
            {
                _effect2.Parameters["WorldViewProjection"].SetValue(mesh.ParentBone.Transform * _matrixMundo * vista * proyeccion);/// mesh * (mundo * view * proy) =  (mesh * mundo * view )* proy
                if (mesh.Name == "Turret" || mesh.Name == "Cannon")
                {
                    Matrix MundoShader = mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta) * _matrixMundo;
                    _effect2.Parameters["WorldViewProjection"].SetValue(MundoShader * vista * proyeccion);/// mesh * (mundo * view * proy) =  (mesh * mundo * view )* proy
                    Matrix transform = mesh.ParentBone.Transform * Matrix.CreateRotationZ(giroTorreta) * _matrixMundo;
                    if (mesh.Name == "Cannon")
                    {
                        MundoShader = Matrix.CreateRotationX(-alturaTorreta - 0.3f) * transform;
                        _effect2.Parameters["WorldViewProjection"].SetValue(MundoShader * vista * proyeccion);/// mesh * (mundo * view * proy) =  (mesh * mundo * view )* proy
                        //_effect2.Parameters["World"].SetValue(Matrix.CreateRotationX(-alturaTorreta - 0.3f) * transform);

                    }
                }
                mesh.Draw();
            }
            this.ResetDeformation();
        }


        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/shaderT90");
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
            //en algun punto agregar dos casos mas, giro sobre el eje a la izquierda y derecha
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
                    resultado += new Vector2(1, 0.5f) * velocidad;
                    break;
                case Animacion.giroIzq:
                    resultado += new Vector2(0.5f, 1) * velocidad;
                    break;
                case Animacion.giroSobreEjeDer:
                    resultado += new Vector2(1f, -1f) * 0.06f;
                    break;
                case Animacion.giroSobreEjeIzq:
                    resultado += new Vector2(-1f, 1f) * 0.06f;
                    break;
                default:
                    resultado = new Vector2(0, 0);
                    break;
            }
            return resultado;
        }

        //T90
        public bool esRuedaDerechaT90(string nombre)
        {
            return nombre.Contains("9") || nombre.Contains("10") || nombre.Contains("11")
            || nombre.Contains("12") || nombre.Contains("13") || nombre.Contains("14")
            || nombre.Contains("15") || nombre.Contains("16");

        }

        //Panzer
        public bool esRuedaDerechaPanzer(string nombre)
        {
            return nombre.Contains("10") || nombre.Contains("11") || nombre.Contains("12")
            || nombre.Contains("13") || nombre.Contains("14") || nombre.Contains("15")
            || nombre.Contains("16") || nombre.Contains("17") || nombre.Contains("18")
            || nombre.Contains("19") || nombre.Contains("20");

        }

        internal override void EfectoDaño(float porcentajeVida)
        {
            modificadorDanio = porcentajeVida;
        }

        public void ActualizarMovimeinto(float velocidad, float angulo)
        {//update de entidad
            Animacion animacion = Animacion.Detenido;
            if (angulo > 0 && velocidad != 0) animacion = Animacion.giroDer;
            else if (angulo < 0 && velocidad != 0) animacion = Animacion.giroIzq;
            else if (velocidad > 0) animacion = Animacion.MarchaAdelante;
            else if (velocidad < 0) animacion = Animacion.MarchaAtras;
            else if (velocidad == 0 && angulo > 0) animacion = Animacion.giroSobreEjeDer;
            else if (velocidad == 0 && angulo < 0) animacion = Animacion.giroSobreEjeIzq;
            offsetCintas = getAnimacionTanque(animacion, velocidad / 10, offsetCintas); // esta es la animacion por defecto, 0 = detenido
            rotacionRuedas = getAnimacionTanque(animacion, velocidad, rotacionRuedas); // esta es la animacion por defecto, 0 = detenido
        }

        internal void ActualizarTorreta(Vector2 dirMovimiento, Vector3 dirApuntado)
        {
            if (dirMovimiento != Vector2.Zero)
                dirMovimiento.Normalize();
            // Calcular ángulo deseado en el plano horizontal (XZ)
            float anguloDeseadoHorizontal = (float)Math.Atan2(dirApuntado.X, dirApuntado.Z);


            // Calcular ángulo de la torreta en el plano horizontal (XZ)
            float anguloTorretaHorizontal = (float)Math.Atan2(dirMovimiento.X, dirMovimiento.Y);

            // Calcular la diferencia entre los ángulos
            float diferenciaHorizontal = anguloDeseadoHorizontal - anguloTorretaHorizontal;
            giroTorreta = MathHelper.WrapAngle(diferenciaHorizontal);
            alturaTorreta = dirApuntado.Y;

            // Limitar el rango de movimiento de la torreta
            if (alturaTorreta > MathHelper.PiOver2)
                alturaTorreta = MathHelper.PiOver2;
            else if (alturaTorreta < -MathHelper.PiOver2)
                alturaTorreta = -MathHelper.PiOver2;

            // Limitar el rango de movimiento de la torreta
            if (giroTorreta > MathHelper.Pi)
                giroTorreta = MathHelper.Pi;
            else if (giroTorreta < -MathHelper.Pi)
                giroTorreta = -MathHelper.Pi;
        }

        public void CambiarTexturaT90(string textura)
        {
            if (this._tipoTanque.directorioModelo().Contains("Panzer"))
            {
                // no se puede cambiar la textura del Panzer
                return;
            }
            if (textura == "1")
            {
                tanqueTexture = Content.Load<Texture2D>(@"Models/tgc-tanks/T90/textures_mod/hullA2");
                this._effect2.Parameters["Texture"].SetValue(tanqueTexture);
            }
            else if (textura == "2")
            {
                tanqueTexture = Content.Load<Texture2D>(@"Models/tgc-tanks/T90/textures_mod/hullB2");
                this._effect2.Parameters["Texture"].SetValue(tanqueTexture);
            }
            else if (textura == "3")
            {
                tanqueTexture = Content.Load<Texture2D>(@"Models/tgc-tanks/T90/textures_mod/hullC");
                this._effect2.Parameters["Texture"].SetValue(tanqueTexture);
            }

        }

        public void CambiarTecnica(string tecnica)
        {
            _tecnica = tecnica;
        }
        //
        private void deformarTanque(Vector3[] Deformacion)
        {
            //this._modelo.Meshes[0].MeshParts.VertexBuffer.SetData(Deformacion);
        }

        public void RecargarModelo()
        {
            this._modelo = this.Content.Load<Model>(@"Models/tgc-tanks" + this._tipoTanque.directorioModelo());
        }

        public void DeformModel(Vector3 impactPoint, float radius, float intensity)
        {
            foreach (ModelMesh mesh in this._modelo.Meshes)
            {
                if (mesh.Name == "Turret" || mesh.Name == "Hull")
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // Copiamos el vertex buffer original
                        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.NumVertices];
                        part.VertexBuffer.GetData(vertices);

                        for (int i = 0; i < vertices.Length; i++)
                        {
                            float dist = Vector3.Distance(vertices[i].Position, impactPoint);
                            Vector3 posv = vertices[i].Position;
                            if (dist < radius)
                            {
                                // Deformación: mover vértice lejos del punto de impacto
                                Vector3 direction = Vector3.Normalize(vertices[i].Position - impactPoint);
                                vertices[i].Position += direction * (1.0f - dist / radius) * intensity;
                            }
                        }

                        // Subimos los nuevos vértices
                        part.VertexBuffer.SetData(vertices);
                    }
                }
            }
        }



        // En tu clase de tanque
        public List<(Vector3 impactPoint, float radius, float intensity)> deformaciones = new List<(Vector3, float, float)>();

        public void AddImpact(Vector3 point, float radius, float intensity)
        {
            /*if (this.deformaciones.Count >= this._tipoTanque.CantidadMaxDeformaciones())
            {
                deformaciones.RemoveAt(0);
            }
            deformaciones.Add((point, radius, intensity));*/
            setDeformacion();
            DeformModel(point, radius, intensity);
            GuardarVerticesModificado();
            ResetDeformation();
        }

        private void DeformarModelo()
        {
            /*foreach (var deformacion in deformaciones)
            {
                this.DeformModel(deformacion.impactPoint, deformacion.radius, deformacion.intensity);
            }*/
        }


        private Dictionary<string, VertexPositionNormalTexture[]> _originalVertices = new();

        public void GuardarVertices()
        {
            foreach (ModelMesh mesh in this._modelo.Meshes)
            {
                if (mesh.Name == "Turret" || mesh.Name == "Hull")
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.NumVertices];
                        part.VertexBuffer.GetData(vertices);

                        // Crear una clave única por mesh y part
                        string key = mesh.Name + "_" + part.GetHashCode();


                        // Guardar una copia de los vértices originales
                        _originalVertices[key] = (VertexPositionNormalTexture[])vertices.Clone();
                    }
                }
            }
        }


        public void GuardarVerticesModificado()
        {
            foreach (ModelMesh mesh in this._modelo.Meshes)
            {
                if (mesh.Name == "Turret" || mesh.Name == "Hull")
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.NumVertices];
                        part.VertexBuffer.GetData(vertices);

                        // Crear una clave única por mesh y part
                        string key = mesh.Name + "_" + part.GetHashCode();


                        // Guardar una copia de los vértices originales
                        _modificadoVertices[key] = (VertexPositionNormalTexture[])vertices.Clone();
                    }
                }
            }
        }

        public void ResetDeformation()
        {
            if (_originalVertices.Count == 0)
                return;
            foreach (ModelMesh mesh in this._modelo.Meshes)
            {
                if (mesh.Name == "Turret" || mesh.Name == "Hull")
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        string key = mesh.Name + "_" + part.GetHashCode();

                        if (_originalVertices.TryGetValue(key, out var originalVerts))
                        {
                            part.VertexBuffer.SetData((VertexPositionNormalTexture[])originalVerts.Clone());
                        }
                        else
                        {
                            // Opcional: loguear si no se encuentra la clave
                            Console.WriteLine($"[ResetDeformation] No se encontró copia original para {key}");
                        }
                    }
                }
            }
        }

        public void setDeformacion()
        {
            if (_modificadoVertices.Count == 0)
                return;
            foreach (ModelMesh mesh in this._modelo.Meshes)
            {
                if (mesh.Name == "Turret" || mesh.Name == "Hull")
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        string key = mesh.Name + "_" + part.GetHashCode();

                        if (_modificadoVertices.TryGetValue(key, out var modificadoVertices))
                        {
                            part.VertexBuffer.SetData((VertexPositionNormalTexture[])modificadoVertices.Clone());
                        }
                        else
                        {
                            // Opcional: loguear si no se encuentra la clave
                            Console.WriteLine($"[ResetDeformation] No se encontró copia original para {key}");
                        }
                    }
                }
            }
        }
        
        private Dictionary<string, VertexPositionNormalTexture[]> _modificadoVertices = new();


        }
}