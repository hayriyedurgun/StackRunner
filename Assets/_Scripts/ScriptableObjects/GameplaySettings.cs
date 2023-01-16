﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "ScriptableObjects/GameplaySettings", order = 0)]
    public class GameplaySettings : ScriptableObject
    {
        public float CharacterSpeed = 5;

        public float TileSpeed = 5;

        public float SpawnX = 4f;

        public float YoyoDuration = 2f;
        public Ease Ease = Ease.Linear;
    }
}
