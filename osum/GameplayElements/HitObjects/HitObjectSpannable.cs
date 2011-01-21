﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using osum.Helpers;

namespace osum.GameplayElements.HitObjects
{
    class HitObjectSpannable : HitObject
    {
        public HitObjectSpannable(HitObjectManager hitObjectManager, Vector2 position, int startTime, HitObjectSoundType soundType, bool newCombo, int comboOffset)
            : base(hitObjectManager, position, startTime, soundType, newCombo, comboOffset)
        {
        }

        /// <summary>
        /// Internal judging of a Hit() call. Is only called after preliminary checks have been completed.
        /// </summary>
        /// <returns>A <see cref="ScoreChange"/></returns>
        protected override ScoreChange HitActionInitial()
        {
            return ScoreChange.Ignore;
        }

        /// <summary>
        /// Is this object currently within an active range?
        /// </summary>
        internal override bool IsActive
        {
            get
            {
                return StartTime < Clock.AudioTime && (EndTime > Clock.AudioTime || !IsHit);
            }
        }

        internal override bool IsVisible
        {
            get
            {
                return Clock.AudioTime >= StartTime - DifficultyManager.PreEmpt &&
                   Clock.AudioTime <= EndTime + DifficultyManager.FadeOut;
            }
        }
    }
}
