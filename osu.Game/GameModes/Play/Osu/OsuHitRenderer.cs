﻿//Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
//Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Drawables;
using osu.Framework.Graphics.Transformations;
using osu.Game.Beatmaps.Objects;
using osu.Game.Beatmaps.Objects.Osu;
using OpenTK;

namespace osu.Game.GameModes.Play.Osu
{
    public class OsuHitRenderer : HitRenderer
    {
        List<OsuBaseHit> objects;
        private OsuPlayfield playfield;

        public override List<BaseHit> Objects
        {
            get
            {
                return objects.ConvertAll(o => (BaseHit)o);
            }

            set
            {
                //osu! mode requires all objects to be of OsuBaseHit type.
                objects = value.ConvertAll(o => (OsuBaseHit)o);

                if (Parent != null)
                    Load();
            }
        }

        public override void Load()
        {
            base.Load();

            if (playfield == null)
                Add(playfield = new OsuPlayfield());
            else
                playfield.Clear();

            if (objects == null) return;

            foreach (OsuBaseHit h in objects)
            {
                //render stuff!
                Sprite s = new Sprite(Game.Textures.Get(@"menu-osu"))
                {
                    Origin = Anchor.Centre,
                    Scale = 0.2f,
                    Alpha = 0,
                    Position = h.Position
                };

                s.Transformations.Add(new TransformAlpha(Clock) { StartTime = h.StartTime - 200, EndTime = h.StartTime, StartValue = 0, EndValue = 1 });
                s.Transformations.Add(new TransformAlpha(Clock) { StartTime = h.StartTime + h.Duration + 200, EndTime = h.StartTime + h.Duration + 400, StartValue = 1, EndValue = 0 });

                playfield.Add(s);
            }
        }
    }

    public class OsuPlayfield : Container
    {
        public OsuPlayfield()
        {
            Size = new Vector2(512, 384);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public override void Load()
        {
            base.Load();

            Add(new Box() { SizeMode = InheritMode.XY, Alpha = 0.5f });
        }
    }
}
