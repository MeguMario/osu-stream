﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osum.Helpers;
using osum.Graphics.Skins;
using OpenTK;

namespace osum.Graphics.Sprites
{
    public delegate pTexture TextureLoadDelegate();

    class pSpriteDynamic : pSprite
    {
        public pSpriteDynamic()
            : base(null, Vector2.Zero)
        {

        }

        public TextureLoadDelegate LoadDelegate;

        internal override pTexture Texture
        {
            get
            {
                pTexture t = base.Texture;
                if (t == null || t.IsDisposed)
                    LoadTexture();
                return base.Texture;
            }
            set
            {
                base.Texture = value;
            }
        }

        bool failedLoad;
        private void LoadTexture()
        {
            if (failedLoad) return;

            pTexture t = null;
            if (LoadDelegate != null)
                t = LoadDelegate();

            if (t == null)
                failedLoad = true;
            else
            {
                Texture = t;
                TextureManager.RegisterDisposable(t);
            }

            Update();
        }
    }
}
