﻿using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using osum.Audio;
using osum.GameModes;
using osum.GameplayElements.Beatmaps;
using osum.Graphics.Sprites;
using osum.Graphics.Skins;
using osum.Helpers;
using osum.GameModes.SongSelect;
using OpenTK.Graphics;
using osum.GameModes.Play.Components;
using osum.Graphics.Drawables;
using osum.GameplayElements;
using System.Threading;

namespace osum.GameModes
{
    public partial class SongSelectMode : GameMode
    {
        private pSpriteCollection spritesDifficultySelection = new pSpriteCollection();

        private pSprite s_TabBarBackground;

        private pSprite s_ModeButtonStream;

        private pSprite s_ModeArrowLeft;
        private pSprite s_ModeArrowRight;
        private pSprite s_ModeButtonEasy;
        private pSprite s_ModeButtonExpert;

        /// <summary>
        /// True when expert mode is not yet unlocked for the current map.
        /// </summary>
        bool mapRequiresUnlock
        {
            get { return false; }
        }

        private void showDifficultySelection()
        {
            if (s_ModeButtonStream == null)
            {
                Vector2 border = new Vector2(4, 4);

                int ypos = 140;
                float spacing = border.X;

                Vector2 buttonSize = new Vector2((GameBase.BaseSize.Width - spacing * 4) / 3f, 100);

                float currX = spacing;

                s_ModeArrowLeft = new pSprite(TextureManager.Load(OsuTexture.songselect_mode_arrow), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(-150, 0), 0.45f, true, Color4.White);
                s_ModeArrowLeft.OnHover += delegate { s_ModeArrowLeft.ScaleTo(1.2f, 100, EasingTypes.In); };
                s_ModeArrowLeft.OnHoverLost += delegate { s_ModeArrowLeft.ScaleTo(1f, 100, EasingTypes.In); };
                s_ModeArrowLeft.OnClick += onSelectPreviousMode;

                spritesDifficultySelection.Add(s_ModeArrowLeft);

                s_ModeArrowRight = new pSprite(TextureManager.Load(OsuTexture.songselect_mode_arrow), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(150, 0), 0.45f, true, Color4.DarkGray);
                s_ModeArrowRight.OnHover += delegate { s_ModeArrowRight.ScaleTo(1.2f, 100, EasingTypes.In); };
                s_ModeArrowRight.OnHoverLost += delegate { s_ModeArrowRight.ScaleTo(1f, 100, EasingTypes.In); };
                s_ModeArrowRight.OnClick += onSelectNextMode;

                s_ModeArrowRight.Rotation = 1;
                spritesDifficultySelection.Add(s_ModeArrowRight);

                s_ModeButtonStream = new pSprite(TextureManager.Load(OsuTexture.songselect_mode_stream), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(0, 0), 0.4f, true, Color4.White);
                spritesDifficultySelection.Add(s_ModeButtonStream);

                s_ModeButtonEasy = new pSprite(TextureManager.Load(OsuTexture.songselect_mode_easy), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(-mode_button_width, 0), 0.4f, true, Color4.White);
                spritesDifficultySelection.Add(s_ModeButtonEasy);

                s_ModeButtonExpert = new pSprite(TextureManager.Load(OsuTexture.songselect_mode_expert), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(mode_button_width, 0), 0.4f, true, Color4.White);
                spritesDifficultySelection.Add(s_ModeButtonExpert);

                currX += buttonSize.X + spacing;

                s_TabBarBackground = new pSprite(TextureManager.Load(OsuTexture.songselect_tab_bar), FieldTypes.StandardSnapTopCentre, OriginTypes.TopCentre, ClockTypes.Mode, new Vector2(0, -100), 0.4f, true, Color4.White);
                spritesDifficultySelection.Add(s_TabBarBackground);

                spriteManager.Add(spritesDifficultySelection);
                spritesDifficultySelection.Sprites.ForEach(s => s.Alpha = 0);
            }

            //preview has finished loading.
            State = SelectState.DifficultySelect;

            foreach (pDrawable s in SelectedPanel.Sprites)
                s.MoveTo(new Vector2(0, 0), 500, EasingTypes.InDouble);

            s_TabBarBackground.Transform(new Transformation(new Vector2(0, -100), new Vector2(0, -100), Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));
            s_TabBarBackground.Transform(new Transformation(new Vector2(0, 0), new Vector2(0, BeatmapPanel.PANEL_HEIGHT), Clock.ModeTime + 400, Clock.ModeTime + 1000, EasingTypes.In));

            spritesDifficultySelection.Sprites.ForEach(s => s.FadeIn(200));

            s_Header.Transform(new Transformation(Vector2.Zero, new Vector2(0, -59), Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));
            s_Header.Transform(new Transformation(TransformationType.Rotation, s_Header.Rotation, 0.03f, Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));

            s_Footer.Transform(new Transformation(new Vector2(-60, -105), Vector2.Zero, Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));
            s_Footer.Transform(new Transformation(TransformationType.Rotation, 0.04f, 0, Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));

            updateModeSelectionArrows();
        }

        void onSelectPreviousMode(object sender, EventArgs e)
        {
            switch (Player.Difficulty)
            {
                case Difficulty.Normal:
                    Player.Difficulty = Difficulty.Easy;
                    break;
                case Difficulty.Expert:
                    Player.Difficulty = Difficulty.Normal;
                    break;
            }

            updateModeSelectionArrows();
        }

        void onSelectNextMode(object sender, EventArgs e)
        {
            switch (Player.Difficulty)
            {
                case Difficulty.Easy:
                    Player.Difficulty = Difficulty.Normal;
                    break;
                case Difficulty.Normal:
                    Player.Difficulty = Difficulty.Expert;
                    break;
            }

            updateModeSelectionArrows();
        }

        const float mode_button_width = 300;

        /// <summary>
        /// Updates the states of mode selection arrows depending on the current mode selection.
        /// </summary>
        private void updateModeSelectionArrows()
        {
            bool hasPrevious = false;
            bool hasNext = false;

            float horizontalPosition = 0;

            switch (Player.Difficulty)
            {
                case Difficulty.Easy:
                    hasNext = true;
                    horizontalPosition = mode_button_width;
                    break;
                case Difficulty.Normal:
                    hasPrevious = true;
                    hasNext = !mapRequiresUnlock;
                    break;
                case Difficulty.Expert:
                    hasPrevious = true;
                    horizontalPosition = -mode_button_width;
                    break;
            }

            s_ModeArrowLeft.Colour = hasPrevious ? Color4.White : Color4.DarkGray;
            s_ModeArrowLeft.HandleInput = hasPrevious;

            s_ModeArrowRight.Colour = hasNext ? Color4.White : Color4.DarkGray;
            s_ModeArrowRight.HandleInput = hasNext;

            s_ModeButtonEasy.MoveTo(new Vector2(horizontalPosition - mode_button_width, 0), 500, EasingTypes.In);
            s_ModeButtonStream.MoveTo(new Vector2(horizontalPosition, 0), 500, EasingTypes.In);
            s_ModeButtonExpert.MoveTo(new Vector2(horizontalPosition + mode_button_width, 0), 500, EasingTypes.In);

        }

        private void leaveDifficultySelection(object sender, EventArgs args)
        {
            State = SelectState.SongSelect;

            InitializeBgm();

            GameBase.Scheduler.Add(delegate
            {
                foreach (BeatmapPanel p in panels)
                {
                    p.s_BackingPlate.HandleInput = true;

                    foreach (pDrawable d in p.Sprites)
                        d.FadeIn(200);
                }

                spritesDifficultySelection.Sprites.ForEach(s => s.FadeOut(50));

                s_Header.Transform(new Transformation(s_Header.Position, Vector2.Zero, Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));
                s_Header.Transform(new Transformation(TransformationType.Rotation, s_Header.Rotation, 0, Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));

                s_Footer.Transform(new Transformation(s_Footer.Position, new Vector2(-60, -105), Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));
                s_Footer.Transform(new Transformation(TransformationType.Rotation, 0, 0.04f, Clock.ModeTime, Clock.ModeTime + 500, EasingTypes.In));
            }, true);
        }

        private void onStartButtonPressed(object sender, EventArgs args)
        {
            if (State == SelectState.Starting)
                return;

            State = SelectState.Starting;

            if (Player.Difficulty != Difficulty.Easy) s_ModeButtonEasy.FadeOut(200);
            if (Player.Difficulty != Difficulty.Normal) s_ModeButtonStream.FadeOut(200);
            if (Player.Difficulty != Difficulty.Expert) s_ModeButtonExpert.FadeOut(200);

            s_ModeArrowLeft.FadeOut(200);
            s_ModeArrowRight.FadeOut(200);

            GameBase.Scheduler.Add(delegate
            {
                Director.ChangeMode(OsuMode.Play);
            }, 900);
        }
    }
}