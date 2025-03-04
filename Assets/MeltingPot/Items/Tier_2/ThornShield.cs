﻿using BepInEx.Configuration;
using MeltingPot.Utils;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static R2API.RecalculateStatsAPI;

namespace MeltingPot.Items
{
    public class ThornShield : ItemBase<ThornShield>
    {
        public static float armourGrowth = 0.25f;
        public static float flatArmour = 10f;
        public static float flatDamage = 5f;
        public static float flatStack = 2f;
        public override string ItemName => "Shield of Thorns";
        public override string ItemLangTokenName => "THORNSHIELD";
        public override string ItemPickupDesc =>
            $"<style=cIsHealing>Reflect</style> melee damage equal to a <style=cIsHealing>portion of armor</style>.";
        public override string ItemFullDescription =>
            $"Gain <style=cIsHealing>{flatArmour} armor</style> <style=cStack>(+{flatArmour} per stack)</style>. Taking damage in melee range returns <style=cIsDamage>{flatDamage}</style> <style=cStack>(+{flatStack} per stack)</style> plus <style=cIsDamage>{armourGrowth * 100}%</style> <style=cStack>(+{armourGrowth * 100}% per stack)</style> of your <style=cIsHealing>armor</style> as damage.";
        public override string ItemLore =>
            "Forgive me. Forgive my friends. We burned the druids in anger, and it has come back to haunt us. The ground rages. They are near.";

        public override string VoidCounterpart => null;
        public static BepInEx.Logging.ManualLogSource BSModLogger;
        public static GameObject ThornEffect;

        public static GameObject ItemModel;
        public static GameObject ItemBodyModelPrefab;

        public static BuffDef ThornActiveBuff =>
            ContentPackProvider.contentPack.buffDefs.Find("MeltingPot_ThornBuff");

        public override void Init(ConfigFile config, bool enabled)
        {
            CreateItem("ThornShield_ItemDef", enabled);
            if (enabled)
            {
                ItemModel = Assets.mainAssetBundle.LoadAsset<GameObject>(
                    $"{ModelPath}/thorn_shield/ShieldofThorns.prefab"
                );
                CreateLang();
                CreateEffect();
                Hooks();
            }
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>(
                $"{ModelPath}/thorn_shield/displayThornShield.prefab"
            );
            Vector3 generalScale = new Vector3(1f, 1f, 1f);
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add(
                "mdlCommandoDualies",
                new RoR2.ItemDisplayRule[]
                {
                    new RoR2.ItemDisplayRule
                    {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemBodyModelPrefab,
                        childName = "UpperArmL",
                        localPos = new Vector3(0.08158F, 0.364F, 0.04397F),
                        localAngles = new Vector3(357.7899F, 260.6965F, 270.743F),
                        localScale = new Vector3(0.16226F, 0.16226F, 0.16226F)
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
                        childName = "Pelvis",
                        localPos = new Vector3(0F, -0.27727F, 0.15052F),
                        localAngles = new Vector3(0F, 180F, 112.5631F),
                        localScale = new Vector3(0.18139F, 0.18139F, 0.18139F)
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
                        childName = "Hip",
                        localPos = new Vector3(2.07188F, 0.56545F, 0.00017F),
                        localAngles = new Vector3(0F, 270F, 107.0169F),
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
                        childName = "Chest",
                        localPos = new Vector3(0.002F, -0.04605F, -0.30533F),
                        localAngles = new Vector3(354.4472F, 0.53189F, 292.1049F),
                        localScale = new Vector3(0.13121F, 0.13121F, 0.13121F)
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
                        childName = "LowerArmR",
                        localPos = new Vector3(-0.07468F, 0.17895F, -0.0002F),
                        localAngles = new Vector3(0F, 133.4465F, 0F),
                        localScale = new Vector3(0.25F, 0.25F, 0.25F)
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
                        localPos = new Vector3(-0.00099F, 0.23105F, 0.01854F),
                        localAngles = new Vector3(86.19852F, 3.67883F, 3.4493F),
                        localScale = new Vector3(0.35F, 0.35F, 0.25F)
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
                        childName = "FlowerBase",
                        localPos = new Vector3(0.77055F, 1.84111F, -0.34754F),
                        localAngles = new Vector3(0F, -0.00001F, 270F),
                        localScale = new Vector3(0.2F, 0.2F, 0.2F)
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
                        childName = "Stomach",
                        localPos = new Vector3(0.0013F, 0.3853F, 0.16613F),
                        localAngles = new Vector3(22.26286F, 175.4879F, 262.9642F),
                        localScale = new Vector3(0.2F, 0.2F, 0.2F)
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
                        localPos = new Vector3(-0.05746F, 2.66866F, 4.02146F),
                        localAngles = new Vector3(348.6305F, 180.5293F, 178.9295F),
                        localScale = new Vector3(1F, 1F, 1F)
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
                        localPos = new Vector3(-0.00569F, 0.25728F, 0.14752F),
                        localAngles = new Vector3(352.3789F, 180.6956F, 269.482F),
                        localScale = new Vector3(0.04502F, 0.04502F, 0.04502F)
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
                        childName = "Hat",
                        localPos = new Vector3(-0.01844F, 0.10087F, 0.07106F),
                        localAngles = new Vector3(31.12656F, 161.9059F, 279.5356F),
                        localScale = new Vector3(0.02676F, 0.02676F, 0.02676F)
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
                        childName = "LegBar1",
                        localPos = new Vector3(0F, 0.07968F, 0.18019F),
                        localAngles = new Vector3(359.0982F, 180.6359F, 111.653F),
                        localScale = new Vector3(0.66596F, 0.66596F, 0.66596F)
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
                        childName = "LowerArmL",
                        localPos = new Vector3(-0.35742F, -0.02077F, 0.62581F),
                        localAngles = new Vector3(355.7607F, 152.8153F, 64.15575F),
                        localScale = new Vector3(4.51801F, 4.51801F, 4.51801F)
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
                        childName = "LowerArmR",
                        localPos = new Vector3(0F, -0.01907F, -0.0112F),
                        localAngles = new Vector3(300.4392F, 0F, 0F),
                        localScale = new Vector3(0.16226F, 0.16226F, 0.16226F)
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
                        childName = "ForeArmL",
                        localPos = new Vector3(0.00375F, 0.09227F, -0.07572F),
                        localAngles = new Vector3(358.9445F, 342.3807F, 267.9208F),
                        localScale = new Vector3(0.16226F, 0.16226F, 0.16226F)
                    }
                }
            );
            return rules;
        }

        public override void Hooks()
        {
            On.RoR2.SetStateOnHurt.OnTakeDamageServer += Thorns;
            On.RoR2.CharacterBody.FixedUpdate += BoostRegen;
            GetStatCoefficients += GrantArmour;
        }

        private static GameObject LoadEffect(string soundName, bool parentToTransform)
        {
            GameObject newEffect = Assets.mainAssetBundle.LoadAsset<GameObject>(
                "assets/meltingpot/mpassets/effects/Thorns.prefab"
            );

            newEffect.AddComponent<DestroyOnTimer>().duration = 1;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = true;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            return newEffect;
        }

        private void GrantArmour(CharacterBody sender, StatHookEventArgs args)
        {
            var count = GetCount(sender);
            if (count > 0)
            {
                args.armorAdd += flatArmour * count;
            }
        }

        public void CreateEffect()
        {
            ThornEffect = LoadEffect("", false);

            if (ThornEffect)
            {
                PrefabAPI.RegisterNetworkPrefab(ThornEffect);
            }
            ContentAddition.AddEffect(ThornEffect);
        }

        private void Thorns(
            On.RoR2.SetStateOnHurt.orig_OnTakeDamageServer orig,
            SetStateOnHurt self,
            DamageReport damageReport
        )
        {
            var body = damageReport.attackerBody;
            var victimBody = damageReport.victimBody;
            if (body && victimBody && body != victimBody)
            {
                var InventoryCount = GetCount(victimBody);
                if (InventoryCount > 0)
                {
                    float distance = Vector3.Distance(body.corePosition, victimBody.corePosition);
                    if (
                        distance < 5.0f
                        && !damageReport.damageInfo.inflictor.name.Contains("FauxThornObj")
                    )
                    {
                        victimBody.AddTimedBuff(ThornActiveBuff, 0.5f);
                        EffectData effectData = new EffectData
                        {
                            origin = victimBody.corePosition,
                            scale = 0.5f,
                        };
                        effectData.SetHurtBoxReference(victimBody.mainHurtBox);
                        GameObject effectPrefab = ThornEffect;
                        EffectManager.SpawnEffect(effectPrefab, effectData, true);
                        DamageInfo reflect = new DamageInfo
                        {
                            damage =
                                flatDamage
                                + flatStack * (InventoryCount - 1)
                                + victimBody.armor * (armourGrowth * InventoryCount),
                            crit = false,
                            attacker = victimBody.gameObject,
                            inflictor = new GameObject("FauxThornObj"),
                            position = victimBody.corePosition,
                            force = new Vector3(0, 0, 0),
                            rejected = false,
                            procCoefficient = 0f,
                            damageType = DamageType.BypassArmor,
                            damageColorIndex = DamageColorIndex.Fragile
                        };
                        body.healthComponent.TakeDamage(reflect);
                    }
                }
            }
            orig(self, damageReport);
            return;
        }

        private void BoostRegen(
            On.RoR2.CharacterBody.orig_FixedUpdate orig,
            RoR2.CharacterBody self
        )
        {
            orig(self);
            var InventoryCount = GetCount(self);
            if (InventoryCount > 0 && self.HasBuff(ThornActiveBuff.buffIndex))
            {
                self.regen += InventoryCount * (0.2f);
            }
        }
    }
}
