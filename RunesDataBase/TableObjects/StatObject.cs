﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Runes.Net.Db;
using Runes.Net.Shared;
using RunesDataBase.Forms;

namespace RunesDataBase.TableObjects
{
    [TypeConverter(typeof(EntitySelectConverter<StatObject>))]
    public class StatObject : BasicVisualTableObject
    {
        public StatObject(BasicObject obj) : base(obj) { }

        [DisplayName("Effects")]
        public WearStat[] Stats
        {
            get
            {
                var a = new WearStat[10];
                for (var i = 0; i < 10; ++i)
                    a[i] = new WearStat(this, i);
                return a;
            }
        }

        [DisplayName("Rarity level")]
        public StatRarity Rarity
        {
            get { return (StatRarity)DbObject.GetFieldAsUInt("rarity"); }
            set { DbObject.SetField("rarity", (uint)value); }
        }

        [DisplayName("Inherent Value")]
        [Description("??")]
        public int InherentValue
        {
            get { return DbObject.GetFieldAsInt("inherentvalue"); }
            set { DbObject.SetField("inherentvalue", value); }
        }

        [DisplayName("Star Value")]
        public int StarValue
        {
            get { return DbObject.GetFieldAsInt("starvalue"); }
            set { DbObject.SetField("starvalue", value); }
        }

        [DisplayName("Standard ability level")]
        [Description("??")]
        public int StandardAbilitLevel
        {
            get { return DbObject.GetFieldAsInt("standardability_lv"); }
            set { DbObject.SetField("standardability_lv", value); }
        }

        public override Color GetColor()
            => Colors.GetColorForStat(Stats.Count(x => !x.IsEmpty), Rarity);

        public override string GetDescription() => Environment.NewLine + string.Join(Environment.NewLine, Stats.Where(o => !o.IsEmpty));

        public override string GetIconName() => "stat";

        public override string ToString() => $"{MainForm.Database.GetNameForGuidWithGuid2(Guid)}, {string.Join(", ", Stats.Where(o => !o.IsEmpty))}";


        public IEnumerable<WearStat> GetEffectsByType(WearEquipmentType type) => Stats.Where(e => e.Type == type);
        public double GetEffectValueByType(WearEquipmentType type) => GetEffectsByType(type).Sum(x => x.Value);
    }
}
