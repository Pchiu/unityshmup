﻿using System;
using System.Collections.Generic;
using Assets.Scripts.DataManagement;
using Assets.Scripts.Enums;
using Assets.Scripts.Movement;
using Assets.Scripts.Ships;
using Assets.Scripts.Shots;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ShipController : MonoBehaviour {

        public List<Ship> Ships;
        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public Ship SpawnShip(string name, Vector2 position, float rotation)
        {
            GameObject ShipObject = Instantiate(Resources.Load("Prefabs/" + name), position, Quaternion.AngleAxis(rotation, Vector3.forward)) as GameObject;
            return ShipObject.GetComponent<Ship>();   
        }

        public GameObject AssembleTestShip()
        {
            GameObject shipObject = new GameObject();
            var ship = shipObject.AddComponent<Ship>();
            //ship.tag = "Enemy";
            ship.HardpointGroups = new Dictionary<string, HardpointGroup>();
            ship.ShipPhases = new List<ShipPhase>();
            ship.ShipSections = new List<ShipSection>();
            var phaseData = new GameData.ShipPhase();
            phaseData.MovementPattern = GameDataManager.Instance.MovementPatternManager.MovementPatterns["TestPattern"];
            phaseData.ShipSections = new List<GameData.ShipSection>();

            var section1 = new GameData.ShipSection
            {
                Offset = new Vector2(0, 0),
                Name = "ShipPartSquare",
                Hardpoints = new List<GameData.Hardpoint>(),
                Hull = 10,
                Type = ShipSectionType.Critical
            };

            var subsystemTypes = new List<SubsystemType> {SubsystemType.Weapon};

            var section2 = new GameData.ShipSection
            {
                Offset = new Vector2(1.95f, 0),
                Name = "ShipPartRectangle",
                Hardpoints = new List<GameData.Hardpoint>
                {
                    new GameData.Hardpoint
                    {
                        Types = subsystemTypes,
                        Group = "Weapon",
                        Position = new Vector2(0, 2),
                        SubsystemID = null
                    }
                },
                Hull = 10,
                Type = ShipSectionType.Standard
            };

            var section3 = new GameData.ShipSection
            {
                Offset = new Vector2(-1.95f, 0),
                Name = "ShipPartRectangle",
                Hardpoints = new List<GameData.Hardpoint>
                {
                    new GameData.Hardpoint
                    {
                        Types = subsystemTypes,
                        Group = "Weapon",
                        Position = new Vector2(0, 2),
                        SubsystemID = null
                    }
                },
                Hull = 10,
                Type = ShipSectionType.Standard
            };

            phaseData.ShipSections.Add(section1);
            phaseData.ShipSections.Add(section2);
            phaseData.ShipSections.Add(section3);

            var phase1 = AssembleShipPhase(ship, phaseData);
            ship.ShipPhases.Add(phase1);
            ship.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            return shipObject;
        }

        public Ship AssembleShip()
        {
            return null;
        }

        public ShipPhase AssembleShipPhase(Ship ship, GameData.ShipPhase phaseData)
        {
            List<ShipSection> sections = new List<ShipSection>();
            ShipPhase phase = new ShipPhase(ship);
            phase.Ship = ship;
            foreach (var section in phaseData.ShipSections)
            {
                sections.Add(AssembleShipSection(phase, section));
            }
            phase.ShipSections = sections;
            phase.MovementPattern = phaseData.MovementPattern;
            return phase;
        }

        public ShipSection AssembleShipSection(ShipPhase phase, GameData.ShipSection sectionData)
        {
            GameObject shipSectionObject =
                Instantiate(Resources.Load("Prefabs/ShipSections/" + sectionData.Name)) as GameObject;
            shipSectionObject.GetComponent<Renderer>().enabled = false;
            shipSectionObject.GetComponent<Collider2D>().enabled = false;
            ShipSection shipSection = shipSectionObject.GetComponent<ShipSection>();
            shipSection.Type = sectionData.Type;
            shipSection.Ship = phase.Ship;
            var hardpointObjects = new List<Hardpoint>();
            foreach (var hardpoint in sectionData.Hardpoints)
            {
                GameObject hardpointObject = new GameObject();
                var hardpointComponent = hardpointObject.AddComponent<Hardpoint>();
                hardpointComponent.Types = hardpoint.Types;
                hardpointComponent.Position = hardpoint.Position;
                hardpointComponent.Subsystem = null;
                shipSection.Hardpoints.Add(hardpointComponent);
                if (!phase.HardpointGroups.ContainsKey(hardpoint.Group))
                {
                    phase.HardpointGroups.Add(hardpoint.Group, new HardpointGroup());
                    phase.HardpointGroups[hardpoint.Group].Name = hardpoint.Group;
                }
                phase.HardpointGroups[hardpoint.Group].Hardpoints.Add(hardpointComponent);
                hardpointComponent.transform.parent = shipSection.transform;
            }
            shipSection.transform.position = phase.Ship.transform.position + new Vector3(sectionData.Offset.x, sectionData.Offset.y);
            shipSection.Hull = sectionData.Hull;
            shipSection.Type = sectionData.Type;
            return shipSection;
        }

        public Weapon AssembleWeapon(string weaponSpriteName, FirePattern pattern, FireMode fireMode)
        {
            GameObject weaponObject = Instantiate(Resources.Load("Prefabs/" + weaponSpriteName)) as GameObject;
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            weapon.FirePattern = pattern;
            weapon.FireMode = fireMode;
            return weapon;
        }

        public Projectile AssembleProjectile(string shotSpriteName, MovementPattern pattern, string tag)
        {
            GameObject shotObject = Resources.Load("Prefabs/" + shotSpriteName) as GameObject;
            shotObject.tag = tag;
            Projectile shot = shotObject.GetComponent<Projectile>();
            shot.MovementPattern = pattern;
            return shot;
        }
    }
}
