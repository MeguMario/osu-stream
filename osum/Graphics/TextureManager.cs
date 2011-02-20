﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using osum.Support;
#if IPHONE
using OpenTK.Graphics.ES11;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.OpenGLES;

using TextureTarget = OpenTK.Graphics.ES11.All;
using TextureParameterName = OpenTK.Graphics.ES11.All;
using EnableCap = OpenTK.Graphics.ES11.All;
using BlendingFactorSrc = OpenTK.Graphics.ES11.All;
using BlendingFactorDest = OpenTK.Graphics.ES11.All;
using PixelStoreParameter = OpenTK.Graphics.ES11.All;
using VertexPointerType = OpenTK.Graphics.ES11.All;
using ColorPointerType = OpenTK.Graphics.ES11.All;
using ClearBufferMask = OpenTK.Graphics.ES11.All;
using TexCoordPointerType = OpenTK.Graphics.ES11.All;
using BeginMode = OpenTK.Graphics.ES11.All;
using MatrixMode = OpenTK.Graphics.ES11.All;
using PixelInternalFormat = OpenTK.Graphics.ES11.All;
using PixelFormat = OpenTK.Graphics.ES11.All;
using PixelType = OpenTK.Graphics.ES11.All;
using ShaderType = OpenTK.Graphics.ES11.All;
using VertexAttribPointerType = OpenTK.Graphics.ES11.All;
using ProgramParameter = OpenTK.Graphics.ES11.All;
using ShaderParameter = OpenTK.Graphics.ES11.All;
using ErrorCode = OpenTK.Graphics.ES11.All;
using TextureEnvParameter = OpenTK.Graphics.ES11.All;
using TextureEnvTarget =  OpenTK.Graphics.ES11.All;
#else
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using osum.Input;
#endif


namespace osum.Graphics.Skins
{
    internal class SpriteSheetTexture
    {
        internal string SheetName;
        internal int X;
        internal int Y;
        internal int Width;
        internal int Height;

        public SpriteSheetTexture(string name, int x, int y, int width, int height)
        {
            this.SheetName = name;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
    }

    /// <summary>
    /// Handle the loading of textures from various sources.
    /// Caching, reuse, unloading and everything else.
    /// </summary>
    internal static partial class TextureManager
    {
        static Dictionary<OsuTexture, SpriteSheetTexture> textureLocations = new Dictionary<OsuTexture, SpriteSheetTexture>();

        public static void Initialize()
        {
	        //scoring sprites
            textureLocations.Add(OsuTexture.hit0, new SpriteSheetTexture("hit", 694, 279, 140, 134));
            textureLocations.Add(OsuTexture.hit50, new SpriteSheetTexture("hit", 369, 0, 133, 132));
            textureLocations.Add(OsuTexture.hit100, new SpriteSheetTexture("hit", 189, 0, 180, 180));
            textureLocations.Add(OsuTexture.hit100k, new SpriteSheetTexture("hit", 190, 180, 180, 180));
            textureLocations.Add(OsuTexture.hit300, new SpriteSheetTexture("hit", 0, 0, 189, 190));
            textureLocations.Add(OsuTexture.hit300k, new SpriteSheetTexture("hit", 0, 190, 190, 190));
            textureLocations.Add(OsuTexture.hit300g, new SpriteSheetTexture("hit", 626, 0, 208, 207));

            //sliders
            textureLocations.Add(OsuTexture.sliderfollowcircle, new SpriteSheetTexture("hit", 370, 132, 256, 257));
            textureLocations.Add(OsuTexture.sliderscorepoint, new SpriteSheetTexture("hit", 190, 366, 20, 18));
            textureLocations.Add(OsuTexture.sliderarrow, new SpriteSheetTexture("hit", 626, 206, 77, 58));
			textureLocations.Add(OsuTexture.sliderb_0, new SpriteSheetTexture("hit", 0, 508, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_1, new SpriteSheetTexture("hit", 118, 508, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_2, new SpriteSheetTexture("hit", 236, 508, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_3, new SpriteSheetTexture("hit", 354, 508, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_4, new SpriteSheetTexture("hit", 472, 508, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_5, new SpriteSheetTexture("hit", 0, 626, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_6, new SpriteSheetTexture("hit", 118, 626, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_7, new SpriteSheetTexture("hit", 236, 626, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_8, new SpriteSheetTexture("hit", 354, 626, 118, 118));
			textureLocations.Add(OsuTexture.sliderb_9, new SpriteSheetTexture("hit", 472, 626, 118, 118));


            //hitcircles
            textureLocations.Add(OsuTexture.hitcircle, new SpriteSheetTexture("hit", 834, 0, 108, 108));
            textureLocations.Add(OsuTexture.hitcircleoverlay, new SpriteSheetTexture("hit", 834, 109, 128, 128));
            textureLocations.Add(OsuTexture.approachcircle, new SpriteSheetTexture("hit", 0, 380, 126, 128));
            textureLocations.Add(OsuTexture.holdcircle, new SpriteSheetTexture("hit", 834, 238, 157, 158));
            
            //hitobject misc.
            textureLocations.Add(OsuTexture.followpoint, new SpriteSheetTexture("hit", 195, 387, 11, 11));
			textureLocations.Add(OsuTexture.connectionline, new SpriteSheetTexture("hit", 998, 176, 2, 13));

            //default font (hitobjects)
            textureLocations.Add(OsuTexture.default_0, new SpriteSheetTexture("hit", 131, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_1, new SpriteSheetTexture("hit", 201, 456, 13, 47));
            textureLocations.Add(OsuTexture.default_2, new SpriteSheetTexture("hit", 219, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_3, new SpriteSheetTexture("hit", 289, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_4, new SpriteSheetTexture("hit", 359, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_5, new SpriteSheetTexture("hit", 429, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_6, new SpriteSheetTexture("hit", 499, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_7, new SpriteSheetTexture("hit", 569, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_8, new SpriteSheetTexture("hit", 639, 456, 65, 47));
            textureLocations.Add(OsuTexture.default_9, new SpriteSheetTexture("hit", 709, 456, 65, 47));
            
            //score font
            textureLocations.Add(OsuTexture.score_0, new SpriteSheetTexture("hit", 126, 400, 42, 54));
            textureLocations.Add(OsuTexture.score_1, new SpriteSheetTexture("hit", 180, 400, 20, 54));
            textureLocations.Add(OsuTexture.score_2, new SpriteSheetTexture("hit", 215, 400, 41, 54));
            textureLocations.Add(OsuTexture.score_3, new SpriteSheetTexture("hit", 258, 400, 43, 54));
            textureLocations.Add(OsuTexture.score_4, new SpriteSheetTexture("hit", 302, 400, 43, 54));
            textureLocations.Add(OsuTexture.score_5, new SpriteSheetTexture("hit", 348, 400, 40, 54));
            textureLocations.Add(OsuTexture.score_6, new SpriteSheetTexture("hit", 392, 400, 40, 54));
            textureLocations.Add(OsuTexture.score_7, new SpriteSheetTexture("hit", 438, 400, 37, 54));
            textureLocations.Add(OsuTexture.score_8, new SpriteSheetTexture("hit", 480, 400, 42, 54));
            textureLocations.Add(OsuTexture.score_9, new SpriteSheetTexture("hit", 524, 400, 42, 54));
            textureLocations.Add(OsuTexture.score_comma, new SpriteSheetTexture("hit", 572, 400, 15, 56));
            textureLocations.Add(OsuTexture.score_dot, new SpriteSheetTexture("hit", 596, 400, 17, 54));
            textureLocations.Add(OsuTexture.score_percent, new SpriteSheetTexture("hit", 619, 400, 56, 56));
            textureLocations.Add(OsuTexture.score_x, new SpriteSheetTexture("hit", 834, 400, 36, 54));

			textureLocations.Add(OsuTexture.playfield, new SpriteSheetTexture("hit", 1024, 0, 1024, 768));
			
			GameBase.OnScreenLayoutChanged += delegate {
				DisposeDisposable();		
			};
        }

        public static void Update()
        {
#if DEBUG
            DebugOverlay.AddLine("TextureManager: " + SpriteCache.Count + " cached " + DisposableTextures.Count + " dynamic");
#endif
        } 


		public static void DisposeAll(bool force)
		{
			UnloadAll(force);
			
			SpriteCache.Clear();
			AnimationCache.Clear();
		}
		
		public static void UnloadAll(bool force)
		{
			foreach (pTexture p in SpriteCache.Values)
				if (!p.Permanent || force)
					p.UnloadTexture();

			DisposeDisposable();
		}
		
		public static void DisposeDisposable()
		{
			foreach (pTexture p in DisposableTextures)
				p.Dispose();
			DisposableTextures.Clear();
            availableSurfaces = null;
		}
		
		public static void ReloadAll()
		{
			foreach (pTexture p in SpriteCache.Values)
				p.ReloadIfPossible();
			
			PopulateSurfaces();
		}
		
		public static void RegisterDisposable(pTexture t)
		{
            DisposableTextures.Add(t);
		}
		
    	internal static Dictionary<string, pTexture> SpriteCache = new Dictionary<string, pTexture>();
        internal static Dictionary<string, pTexture[]> AnimationCache = new Dictionary<string, pTexture[]>();
		internal static List<pTexture> DisposableTextures = new List<pTexture>();

        internal static pTexture Load(OsuTexture texture)
        {
            SpriteSheetTexture info;

            if (textureLocations.TryGetValue(texture, out info))
            {
                pTexture tex = Load(info.SheetName);
				tex.Permanent = true;
                tex = tex.Clone(); //make a new instance because we may be using different coords.
                tex.X = info.X;
                tex.Y = info.Y;
                tex.Width = info.Width;
                tex.Height = info.Height;
                tex.OsuTextureInfo = texture;

                return tex;
            }
            else
            {
                //fallback to separate files
                return Load(texture.ToString());
            }
        }
        
        internal static pTexture Load(string name)
        {
            pTexture texture;

            if (SpriteCache.TryGetValue(name, out texture))
                return texture;

            string path = name.IndexOf('.') < 0 ? string.Format(@"Skins/Default/{0}.png", name) : @"Skins/Default/" + name;
	
			if (File.Exists(path))
            {
				texture = pTexture.FromFile(path);
                SpriteCache.Add(name, texture);
                return texture;
            }
			
            return null;
        }

        internal static pTexture[] LoadAnimation(OsuTexture osuTexture, int count)
		{
			pTexture[] textures;
			
			string name = osuTexture.ToString();
			
			if (AnimationCache.TryGetValue(name, out textures))
                return textures;
			
			textures = new pTexture[count];
			
			for (int i = 0; i < count; i++)
				textures[i] = Load((OsuTexture)(osuTexture + i));
			
			AnimationCache.Add(name, textures);
			return textures;
		}
			
		internal static pTexture[] LoadAnimation(string name)
        {
            pTexture[] textures;
            pTexture texture;

            if (AnimationCache.TryGetValue(name, out textures))
                return textures;

            texture = Load(name + "-0");

            // if the texture is found, load all subsequent textures
            if (texture != null)
            {
                List<pTexture> list = new List<pTexture>();
                list.Add(texture);

                for (int i = 1; true; i++)
                {
                    texture = Load(name + "-" + i);

                    if (texture == null)
                    {
                        textures = list.ToArray();
                        AnimationCache.Add(name, textures);
                        return textures;
                    }

                    list.Add(texture);
                }
            }
            
            // if the texture can't be found, try without number
            texture = Load(name);

            if (texture != null)
            {
                textures = new[] { texture };
                AnimationCache.Add(name, textures);
                return textures;
            }

            return null;
        }

        static Queue<pTexture> availableSurfaces;
		
		
		static bool requireSurfaces;
		internal static  bool RequireSurfaces
		{
			get {
				return requireSurfaces;
			}
			
			set
			{
				requireSurfaces = value;
				
				if (value)
				{
					PopulateSurfaces();	
				}
				else
				{
					if (availableSurfaces != null)
					{
						while (availableSurfaces.Count > 0)
							availableSurfaces.Dequeue().Dispose();
						availableSurfaces = null;
					}
				}
			}
		}
		
		internal static void PopulateSurfaces()
		{
			if (availableSurfaces == null && RequireSurfaces)
            {
                availableSurfaces = new Queue<pTexture>();
				
				int size = GameBase.NativeSize.Width;

                for (int i = 0; i < 4; i++)
                {
                    TextureGl gl = new TextureGl(size, size);
                    gl.SetData(IntPtr.Zero, 0, PixelFormat.Rgba);
                    pTexture t = new pTexture(gl, size, size);
					t.BindFramebuffer();
					
                    RegisterDisposable(t);
                    availableSurfaces.Enqueue(t);
                }
            }	
		}
	
        
        internal static pTexture RequireTexture(int width, int height)
        {
            PopulateSurfaces();
			
			if (availableSurfaces.Count == 0)
				return null;

            pTexture tex = availableSurfaces.Dequeue();
            tex.Width = width;
            tex.Height = height;

            return tex;
        }

        internal static void ReturnTexture(pTexture texture)
        {
            if (!texture.IsDisposed && texture.TextureGl.Loaded)
				availableSurfaces.Enqueue(texture);
        }
    }

    internal enum TextureSource
    {
        Game,
        Skin,
        Beatmap
    }

    internal enum OsuTexture
    {
        None = 0,
        hit50,
        hit100,
        hit300,
        sliderfollowcircle,
        hit100k,
        hit300g,
        hit300k,
        hit0,
        sliderscorepoint,
        hitcircle,
        hitcircleoverlay,
        approachcircle,
        sliderarrow,
        holdcircle,
        followpoint,
        connectionline,
        default_0,
        default_1,
        default_2,
        default_3,
        default_4,
        default_5,
        default_6,
        default_7,
        default_8,
        default_9,
		default_comma,
		default_dot,
		default_percent,
		default_x,
		sliderb_0,
		sliderb_1,
		sliderb_2,
		sliderb_3,
		sliderb_4,
		sliderb_5,
		sliderb_6,
		sliderb_7,
		sliderb_8,
		sliderb_9,
        score_0,
        score_1,
        score_2,
        score_3,
        score_4,
        score_5,
        score_6,
        score_7,
        score_8,
        score_9,
		score_comma,
		score_dot,
		score_percent,
		score_x,
		playfield
    }
}
