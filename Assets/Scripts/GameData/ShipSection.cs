﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.GameData
{
    public class ShipSection
    {
        public string Name { get; set; }
        public List<Hardpoint> Hardpoints { get; set; }
        public Vector2 Offset { get; set; }
        public int Hull { get; set; }
        public ShipSectionType Type { get; set; }
    }
}
