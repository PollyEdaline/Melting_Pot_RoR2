﻿using BepInEx.Configuration;
using MeltingPot.Utils;
using R2API;
using RoR2;
using System.Linq;
using UnityEngine;

namespace MeltingPot.Items
{
    public class AcidicBubbleWand : ItemBase<AcidicBubbleWand>
    {
        public static float debuffChance = 0.025f;
        public static float durationGrowth = 0.5f;
        public static float damageMult = 0.05f;
        public override string ItemName => "Acidic Bubble Wand";
        public override string ItemLangTokenName => "ACIDICBUBBLEWAND";

        public override string ItemPickupDesc =>
            $"Increase the potency of <style=cIsDamage>damage over time effects</style>.";

        public override string ItemFullDescription =>
            $"Increase the damage of all <style=cIsDamage>damage over time effects</style> by <style=cIsDamage>{damageMult*100}%</style> <style=cStack>(+ {damageMult*100}% per stack)</style>.";

        public override string ItemLore =>
            "The inside is stained lightly from extensive use, mostly water, but who really knows?\n\nIt's a bucket.";

        public override string VoidCounterpart => null;
        public GameObject ItemModel;

        public static GameObject ItemBodyModelPrefab;

        public override void Init(ConfigFile config, bool enabled)
        {
            CreateItem("AcidicBubbleWand_ItemDef", enabled);
            if (enabled)
            {
                ItemModel = Assets.mainAssetBundle.LoadAsset<GameObject>(
                    $"{ModelPath}/acidic_bubble_wand/acidicbubblewand.prefab"
                );
                CreateLang();
                Hooks();
            }
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>(
                $"{ModelPath}/just_bucket/displayjustbucket.prefab"
            );
            Vector3 generalScale = new Vector3(1f, 1f, 1f);
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            return rules;
            rules.Add(
                "mdlCommandoDualies",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(0.02823F, 0.28457F, -0.04327F),
                        localAngles = new Vector3(347.428F, 5.31903F, 167.7263F),
                        localScale = new Vector3(0.5458F, 0.5458F, 0.5458F)
                    }
                }
            );
            rules.Add(
                "mdlHuntress",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(0.09035F, 0.30118F, -0.17646F),
                        localAngles = new Vector3(325.5183F, 11.308F, 149.5922F),
                        localScale = new Vector3(0.27296F, 0.27296F, 0.27296F)
                    }
                }
            );
            rules.Add(
                "mdlToolbot",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Neck",
                        localPos = new Vector3(-1.925F, 1.55704F, -1.83531F),
                        localAngles = new Vector3(0F, 0F, 0F),
                        localScale = new Vector3(1F, 1F, 1F)
                    }
                }
            );
            rules.Add(
                "mdlEngi",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "HeadCenter",
                        localPos = new Vector3(-0.08227F, 0.22103F, -0.21335F),
                        localAngles = new Vector3(20.18572F, 358.2665F, 179.9301F),
                        localScale = new Vector3(0.14738F, 0.14738F, 0.14738F)
                    }
                }
            );
            rules.Add(
                "mdlMage",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(-0.01007F, 0.21082F, -0.09254F),
                        localAngles = new Vector3(354.5292F, 1.63182F, 185.2126F),
                        localScale = new Vector3(0.25139F, 0.25139F, 0.25139F)
                    }
                }
            );
            rules.Add(
                "mdlMerc",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(0.07967F, 0.25218F, 0.04636F),
                        localAngles = new Vector3(323.5562F, 263.9417F, 169.6504F),
                        localScale = new Vector3(0.22611F, 0.22611F, 0.22611F)
                    }
                }
            );
            rules.Add(
                "mdlTreebot",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "PlatformBase",
                        localPos = new Vector3(0.8597F, 2.57321F, -0.34448F),
                        localAngles = new Vector3(14.31126F, 104.2635F, 3.59551F),
                        localScale = new Vector3(0.21396F, 0.21396F, 0.21396F)
                    }
                }
            );
            rules.Add(
                "mdlLoader",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "FootL",
                        localPos = new Vector3(-0.01514F, -0.00521F, 0.00453F),
                        localAngles = new Vector3(327.554F, 128.2742F, 208.1566F),
                        localScale = new Vector3(0.72437F, 0.72437F, 0.72437F)
                    }
                }
            );
            rules.Add(
                "mdlCroco",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "MouthMuzzle",
                        localPos = new Vector3(-0.00319F, -0.27435F, -0.01359F),
                        localAngles = new Vector3(313.655F, 175.0468F, 2.53212F),
                        localScale = new Vector3(5.57909F, 5.57909F, 5.57909F)
                    }
                }
            );
            rules.Add(
                "mdlCaptain",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(0.12628F, 0.06212F, -0.01986F),
                        localAngles = new Vector3(28.20466F, 95.05783F, 341.879F),
                        localScale = new Vector3(0.07361F, 0.07361F, 0.07361F)
                    }
                }
            );
            rules.Add(
                "mdlBandit2",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "FootL",
                        localPos = new Vector3(-0.0004F, 0.02846F, -0.04664F),
                        localAngles = new Vector3(313.5162F, 181.5179F, 180.9492F),
                        localScale = new Vector3(0.62782F, 0.62782F, 0.62782F)
                    }
                }
            );
            rules.Add(
                "mdlRailGunner",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Backpack",
                        localPos = new Vector3(-0.17597F, 0.43614F, 0.04376F),
                        localAngles = new Vector3(359.2964F, 35.50795F, 357.1917F),
                        localScale = new Vector3(0.14836F, 0.14836F, 0.14836F)
                    }
                }
            );
            rules.Add(
                "mdlVoidSurvivor",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(-0.16807F, 0.13819F, -0.06966F),
                        localAngles = new Vector3(325.9055F, 127.957F, 126.0954F),
                        localScale = new Vector3(0.26691F, 0.26691F, 0.26691F)
                    }
                }
            );
            rules.Add(
                "mdlEngiTurret",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "Head",
                        localPos = new Vector3(0F, 0.83468F, -0.02207F),
                        localAngles = new Vector3(0F, 0F, 0F),
                        localScale = new Vector3(1.54859F, 1.54859F, 1.54859F)
                    }
                }
            );
            rules.Add(
                "mdlScav",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "FootR",
                        localPos = new Vector3(-0.05243F, -0.02199F, -0.63673F),
                        localAngles = new Vector3(352.546F, 265.745F, 175.5882F),
                        localScale = new Vector3(3.19819F, 3.19819F, 3.19819F)
                    }
                }
            );
            return rules;
        }

        public override void Hooks()
        {
            On.RoR2.DotController.InflictDot_refInflictDotInfo += AugmentDot;
        }

        private void AugmentDot(On.RoR2.DotController.orig_InflictDot_refInflictDotInfo orig, ref InflictDotInfo inflictDotInfo){
            if (inflictDotInfo.attackerObject) {
                CharacterBody attackerBody = inflictDotInfo.attackerObject.GetComponent<CharacterBody>();
                if (attackerBody && GetCount(attackerBody) > 0) {
                    inflictDotInfo.damageMultiplier += damageMult * GetCount(attackerBody);
				}
			}
            orig(ref inflictDotInfo);
        }
    }
}
