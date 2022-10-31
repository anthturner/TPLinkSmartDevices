// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TPLinkSmartDevices.Model
{
    public static class LightingEffects
    {
        private static readonly List<LightingEffect> _effects = new List<LightingEffect>
        {
            new LightingEffect
            {
                Name = "Aurora",
                Id = "xqUxDhbAhNLqulcuRMyPBmVGyTOyEMEu",
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "sequence",
                Duration = 0,
                Transition = 1500,
                Direction = 4,
                Spread = 7,
                RepeatTimes = 0,
                Sequence = new[]
                {
                    new[] { 120, 100, 100 },
                    new[] { 240, 100, 100 },
                    new[] { 260, 100, 100 },
                    new[] { 280, 100, 100 }
                }
            },
            new LightingEffect
            {
                Name = "Bubbling Cauldron",
                Id = "tIwTRQBqJpeNKbrtBMFCgkdPTbAQGfRP",
                Segments = new[] {0},
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] {100, 270},
                SaturationRange = new[] {80, 100},
                BrightnessRange = new[] {50, 100},
                Duration = 0,
                Transition = 200,
                InitStates = new[]
                {
                    new[] {270, 100, 100}
                },
                Fadeoff = 1000,
                RandomSeed = 24,
                Backgrounds = new[]
                {
                    new[] {270, 40, 50}
                }
            },
            new LightingEffect
            {
                Name = "Candy Cane",
                Id = "HCOttllMkNffeHjEOLEgrFJjbzQHoxEJ",
                Segments = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
                ExpansionStrategy = 1,
                Type = "sequence",
                Duration = 700,
                Transition = 500,
                Direction = 1,
                Spread = 1,
                RepeatTimes = 0,
                Sequence = new[]
                {
                    new[] { 0, 0, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 360, 81, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 0, 0, 100 },
                    new[] { 360, 81, 100 }
                }
            },
            new LightingEffect
            {
                Name = "Christmas",
                Id = "bwTatyinOUajKrDwzMmqxxJdnInQUgvM",
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 136, 146 },
                SaturationRange = new[] { 90, 100 },
                BrightnessRange = new[] { 50, 100 },
                Duration = 5000,
                Transition = 0,
                InitStates = new[] { new[] { 136, 0, 100 } },
                Fadeoff = 2000,
                RandomSeed = 100,
                Backgrounds = new[]
                {
                    new[] { 136, 98, 75 },
                    new[] { 136, 0, 0 },
                    new[] { 350, 0, 100 },
                    new[] { 350, 97, 94 }
                }
            },
            new LightingEffect
            {
                Name = "Flicker",
                Id = "bCTItKETDFfrKANolgldxfgOakaarARs",
                Segments = new[] { 1 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 30, 40 },
                SaturationRange = new[] { 100, 100 },
                BrightnessRange = new[] { 50, 100 },
                Duration = 0,
                Transition = 0,
                TransitionRange = new[] { 375, 500 },
                InitStates = new[]
                {
                    new[] { 30, 81, 80 }
                }
            },
            new LightingEffect
            {
                Name = "Hanukkah",
                Id = "CdLeIgiKcQrLKMINRPTMbylATulQewLD",
                Segments = new[] { 1 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 200, 210 },
                SaturationRange = new[] { 0, 100 },
                BrightnessRange = new[] { 50, 100 },
                Duration = 1500,
                Transition = 0,
                TransitionRange = new[] { 400, 500 },
                InitStates = new[]
                {
                    new[] { 35, 81, 80 }
                }
            },
            new LightingEffect
            {
                Name = "Haunted Mansion",
                Id = "oJnFHsVQzFUTeIOBAhMRfVeujmSauhjJ",
                Brightness = 80,
                Segments = new[] { 80 },
                ExpansionStrategy = 2,
                Type = "random",
                HueRange = new[] { 45, 45 },
                SaturationRange = new[] { 10, 10 },
                BrightnessRange = new[] { 0, 80 },
                Duration = 0,
                Transition = 0,
                TransitionRange = new[] { 50, 1500 },
                InitStates = new[]
                {
                    new[] { 45, 10, 100 }
                },
                Fadeoff = 200,
                RandomSeed = 1,
                Backgrounds = new[]
                {
                    new[] { 45, 10, 100 }
                }
            },
            new LightingEffect
            {
                Name = "Icicle",
                Id = "joqVjlaTsgzmuQQBAlHRkkPAqkBUiqeb",
                Brightness = 70,
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "sequence",
                Duration = 0,
                Transition = 400,
                Direction = 4,
                Spread = 3,
                RepeatTimes = 0,
                Sequence = new[] 
                { 
                    new[] { 190, 100, 70 },
                    new[] { 190, 100, 70 },
                    new[] { 190, 30, 50 },
                    new[] { 190, 100, 70 },
                    new[] { 190, 100, 70 },
                 }
            },
            new LightingEffect
            {
                Name = "Lightning",
                Id = "ojqpUUxdGHoIugGPknrUcRoyJiItsjuE",
                Segments = new[] { 7, 20, 23, 32, 34, 35, 49, 65, 66, 74, 80 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 240, 240 },
                SaturationRange = new[] { 10, 11 },
                BrightnessRange = new[] { 90, 100 },
                Duration = 0,
                Transition = 50,
                InitStates = new[]
                {
                    new[] { 240, 30, 100 }
                },
                Fadeoff = 150,
                RandomSeed = 600,
                Backgrounds = new[]
                {
                    new[] { 200, 100, 100 },
                    new[] { 200, 50, 10 },
                    new[] { 210, 10, 50 },
                    new[] { 240, 10, 0 }
                }
            },
            new LightingEffect
            {
                Name = "Ocean",
                Id = "oJjUMosgEMrdumfPANKbkFmBcAdEQsPy",
                Brightness = 30,
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "sequence",
                Duration = 0,
                Transition = 2000,
                Direction = 3,
                Spread = 16,
                RepeatTimes = 0,
                Sequence = new[]
                {
                    new[] { 198, 84, 30 }, 
                    new[] { 198, 70, 30 }, 
                    new[] { 198, 10, 30 }
                }
            },
            new LightingEffect
            {
                Name = "Rainbow",
                Id = "izRhLCQNcDzIKdpMPqSTtBMuAIoreAuT",
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "sequence",
                Duration = 0,
                Transition = 1500,
                Direction = 1,
                Spread = 12,
                RepeatTimes = 0,
                Sequence = new[]
                {
                    new[] { 0, 100, 100 }, 
                    new[] { 100, 100, 100 }, 
                    new[] { 200, 100, 100 }, 
                    new[] { 300, 100, 100 }
                }
            },
            new LightingEffect
            {
                Name = "Raindrop",
                Id = "QbDFwiSFmLzQenUOPnJrsGqyIVrJrRsl",
                Brightness = 30,
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 200, 200 },
                SaturationRange = new[] { 10, 20 },
                BrightnessRange = new[] { 10, 30 },
                Duration = 0,
                Transition = 1000,
                InitStates = new[]
                {
                    new[] { 200, 40, 100 }
                },
                Fadeoff = 1000,
                RandomSeed = 24,
                Backgrounds = new[]
                {
                    new[] { 200, 40, 0 }
                }
            },
            new LightingEffect
            {
                Name = "Spring",
                Id = "URdUpEdQbnOOechDBPMkKrwhSupLyvAg",
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 0, 90 },
                SaturationRange = new[] { 30, 100 },
                BrightnessRange = new[] { 90, 100 },
                Duration = 600,
                Transition = 0,
                TransitionRange = new[] { 2000, 6000 },
                InitStates = new[]
                {
                    new[] { 80, 30, 100 }
                },
                Fadeoff = 1000,
                RandomSeed = 20,
                Backgrounds = new[]
                {
                    new[] { 130, 100, 40 }
                }
            },
            new LightingEffect
            {
                Name = "Valentines",
                Id = "QglBhMShPHUAuxLqzNEefFrGiJwahOmz",
                Segments = new[] { 0 },
                ExpansionStrategy = 1,
                Type = "random",
                HueRange = new[] { 340, 340 },
                SaturationRange = new[] { 30, 40 },
                BrightnessRange = new[] { 90, 100 },
                Duration = 600,
                Transition = 2000,
                InitStates = new[]
                {
                    new[] { 340, 30, 100 }
                },
                Fadeoff = 3000,
                RandomSeed = 100,
                Backgrounds = new[]
                {
                    new[] { 340, 20, 50 }, 
                    new[] { 20, 50, 50 },
                    new[] { 0, 100, 50 }
                }
            }
        };

        public static List<LightingEffect> GetEffects()
        {
            return _effects;
        }

        public static LightingEffect GetByName(string name)
        {
            return _effects.SingleOrDefault(e => e.Name == name);
        }
    }

    public class LightingEffect
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int Custom { get; set; } = 0;
        public int Brightness { get; set; } = 100;
        public int[] Segments { get; set; }
        [JsonProperty("expansion_strategy")]
        public int? ExpansionStrategy { get; set; }
        public int Enable { get; set; } = 1;
        public string Type { get; set; }
        public int? Duration { get; set; }
        public int? Transition { get; set; }
        public int? Direction { get; set; }
        public int? Spread { get; set; }
        [JsonProperty("repeat_times")]
        public int? RepeatTimes { get; set; }
        public int[][] Sequence { get; set; }
        [JsonProperty("hue_range")]
        public int[] HueRange { get; set; }
        [JsonProperty("saturation_range")]
        public int[] SaturationRange { get; set; }
        [JsonProperty("brightness_range")]
        public int[] BrightnessRange { get; set; }
        [JsonProperty("init_states")]
        public int[][] InitStates { get; set; }
        public int? Fadeoff { get; set; }
        [JsonProperty("random_seed")]
        public int? RandomSeed { get; set; }
        public int[][] Backgrounds { get; set; }
        [JsonProperty("transition_range")]
        public int[] TransitionRange { get; set; }
    }
}
