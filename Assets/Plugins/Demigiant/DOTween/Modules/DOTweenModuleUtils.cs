﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/13

#region

using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

#endregion

#pragma warning disable 1591
namespace DG.Tweening
{
    /// <summary>
    ///     Utility functions that deal with available Modules.
    ///     Modules defines:
    ///     - DOTAUDIO
    ///     - DOTPHYSICS
    ///     - DOTPHYSICS2D
    ///     - DOTSPRITE
    ///     - DOTUI
    ///     Extra defines set and used for implementation of external assets:
    ///     - DOTWEEN_TMP ► TextMesh Pro
    ///     - DOTWEEN_TK2D ► 2D Toolkit
    /// </summary>
    public static class DOTweenModuleUtils
    {
        private static bool _initialized;

    #region Reflection

        /// <summary>
        ///     Called via Reflection by DOTweenComponent on Awake
        /// </summary>
    #if UNITY_2018_1_OR_NEWER
        [Preserve]
    #endif
        public static void Init()
        {
            if (_initialized) return;

            _initialized                                =  true;
            DOTweenExternalCommand.SetOrientationOnPath += Physics.SetOrientationOnPath;

        #if UNITY_EDITOR
        #if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_2017_1
            UnityEditor.EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
        #else
            EditorApplication.playModeStateChanged += PlaymodeStateChanged;
        #endif
        #endif
        }

    #if UNITY_2018_1_OR_NEWER
    #pragma warning disable
        [Preserve]
        // Just used to preserve methods when building, never called
        private static void Preserver()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var mi               = typeof(MonoBehaviour).GetMethod("Stub");
        }
    #pragma warning restore
    #endif

    #endregion

    #if UNITY_EDITOR
        // Fires OnApplicationPause in DOTweenComponent even when Editor is paused (otherwise it's only fired at runtime)
    #if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_2017_1
        static void PlaymodeStateChanged()
    #else
        private static void PlaymodeStateChanged(PlayModeStateChange state)
    #endif
        {
            if (DOTween.instance == null) return;
            DOTween.instance.OnApplicationPause(EditorApplication.isPaused);
        }
    #endif

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public static class Physics
        {
        #region Public Methods

            // Called via Reflection by DOTweenPath
        #if UNITY_2018_1_OR_NEWER
            [Preserve]
        #endif
            public static TweenerCore<Vector3 , Path , PathOptions> CreateDOTweenPathTween(
                    MonoBehaviour target , bool tweenRigidbody , bool isLocal , Path path , float duration , PathMode pathMode
            )
            {
                TweenerCore<Vector3 , Path , PathOptions> t                    = null;
                var                                       rBodyFoundAndTweened = false;
            #if true // PHYSICS_MARKER
                if (tweenRigidbody)
                {
                    var rBody = target.GetComponent<Rigidbody>();
                    if (rBody != null)
                    {
                        rBodyFoundAndTweened = true;
                        t = isLocal
                                ? rBody.DOLocalPath(path , duration , pathMode)
                                : rBody.DOPath(path , duration , pathMode);
                    }
                }
            #endif
            #if true // PHYSICS2D_MARKER
                if (!rBodyFoundAndTweened && tweenRigidbody)
                {
                    var rBody2D = target.GetComponent<Rigidbody2D>();
                    if (rBody2D != null)
                    {
                        rBodyFoundAndTweened = true;
                        t = isLocal
                                ? rBody2D.DOLocalPath(path , duration , pathMode)
                                : rBody2D.DOPath(path , duration , pathMode);
                    }
                }
            #endif
                if (!rBodyFoundAndTweened)
                    t = isLocal
                            ? target.transform.DOLocalPath(path , duration , pathMode)
                            : target.transform.DOPath(path , duration , pathMode);
                return t;
            }

            // Called via Reflection by DOTweenPathInspector
            // Returns FALSE if the DOTween's Physics Module is disabled, or if there's no rigidbody attached
        #if UNITY_2018_1_OR_NEWER
            [Preserve]
        #endif
            public static bool HasRigidbody(Component target)
            {
            #if true // PHYSICS_MARKER
                return target.GetComponent<Rigidbody>() != null;
            #else
                return false;
            #endif
            }

            // Returns FALSE if the DOTween's Physics2D Module is disabled, or if there's no Rigidbody2D attached
            public static bool HasRigidbody2D(Component target)
            {
            #if true // PHYSICS2D_MARKER
                return target.GetComponent<Rigidbody2D>() != null;
            #else
                return false;
            #endif
            }

            // Called via DOTweenExternalCommand callback
            public static void SetOrientationOnPath(PathOptions options , Tween t , Quaternion newRot , Transform trans)
            {
            #if true // PHYSICS_MARKER
                if (options.isRigidbody) ((Rigidbody)t.target).rotation = newRot;
                else trans.rotation                                     = newRot;
            #else
                trans.rotation = newRot;
            #endif
            }

        #endregion
        }
    }
}