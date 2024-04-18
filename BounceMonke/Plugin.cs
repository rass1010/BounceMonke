﻿using BepInEx;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using Utilla;

namespace BounceMonke
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {

        bool inAllowedRoom;
        bool holding;
        float bounce;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        void Update()
        {
            if (inAllowedRoom)
            {
                if (ControllerInputPoller.instance.rightControllerPrimaryButton && !holding)
                {
                    holding = true;


                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = 1.0f;

                }
                else if (!ControllerInputPoller.instance.rightControllerPrimaryButton && holding)
                {
                    holding = false;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = bounce;
                }
            }
        }

        [ModdedGamemodeJoin]
        private void RoomJoined(string gamemode)
        {
            // The room is modded. Enable mod stuff.
            inAllowedRoom = true;
            bounce = GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness;
        }

        [ModdedGamemodeLeave]
        private void RoomLeft(string gamemode)
        {
            // The room was left. Disable mod stuff.
            inAllowedRoom = false;
            GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = bounce;
        }
    }
}