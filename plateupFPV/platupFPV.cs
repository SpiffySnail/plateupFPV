using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using Kitchen.Modules;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;
using Controllers;
using Kitchen.Layouts.Modules;

namespace plateupFPV
{
    public class SetFPV : GenericSystemBase, IModSystem
    {


        InputAction f1Action;
        InputAction lftStick;
        InputAction rgtStick;
        InputAction wAction;
        InputAction aAction;
        InputAction sAction;
        InputAction dAction;
        InputAction mouseMoveAction;
        Camera fpvCamera;


        protected override void Initialise()
        {
            //loop through all InputActions and find DebugLog all of them
            foreach (var action in InputSystem.ListEnabledActions())
            {
                if (action.name == "Movement")
                {
                    action.Disable();
                }
            }

            f1Action = new InputAction("f1", binding: "<Keyboard>/f1");
            f1Action.performed += ctx =>
            {
                GameObject player = GameObject.Find("Player(Clone)");
                GameObject cameraObject = new GameObject("FPV Camera");
                fpvCamera = cameraObject.AddComponent<Camera>();
                fpvCamera.transform.localPosition = new Vector3(0, 1f, 0);
                fpvCamera.transform.parent = player.transform;
                fpvCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
                Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
                fpvCamera.transform.SetPositionAndRotation(pos, player.transform.rotation);
                fpvCamera.fieldOfView = 60;
                fpvCamera.nearClipPlane = 0.3f;
                fpvCamera.clearFlags = CameraClearFlags.Skybox;
                fpvCamera.backgroundColor = new Color(0.5f, 0.5f, 1f);
                fpvCamera.farClipPlane = 3000f;
                Material skyboxMaterial = new Material(Shader.Find("Skybox/Procedural"));
                skyboxMaterial.SetColor("_SkyTint", new Color(0.5f, 0.5f, 1f));
                skyboxMaterial.SetFloat("_SunSize", 0.04f);
                skyboxMaterial.SetFloat("_AtmosphereThickness", 1f);
                skyboxMaterial = Resources.Load<Material>("Skybox/Blue Sky");
                RenderSettings.skybox = skyboxMaterial;
                
             
                Camera topDownCamera = Camera.main;
                topDownCamera.transform.localPosition = new Vector3(0, 10f, 0);
                topDownCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);
                Vector3 pos2 = new Vector3(player.transform.position.x, player.transform.position.y + 10f, 0);
                Vector3 rot = new Vector3(90, 0, 0);
                topDownCamera.transform.SetPositionAndRotation(pos2, Quaternion.Euler(rot));
                topDownCamera.fieldOfView = 60;
                topDownCamera.nearClipPlane = 0.3f;
                topDownCamera.rect = new Rect(0.75f, 0.75f, 0.333f, 0.333f);
                topDownCamera.clearFlags = CameraClearFlags.Skybox;
                topDownCamera.backgroundColor = new Color(0.5f, 0.5f, 1f);
                topDownCamera.farClipPlane = 3000f;
                //make TopDownCamera on top of fpvcamera

                topDownCamera.depth = 1;

            };
            f1Action.Enable();
            
            wAction = new InputAction("w", binding: "<Keyboard>/w");
            wAction.Enable();
            aAction = new InputAction("a", binding: "<Keyboard>/a");
            aAction.Enable();
            sAction = new InputAction("s", binding: "<Keyboard>/s");
            sAction.Enable();
            dAction = new InputAction("d", binding: "<Keyboard>/d");
            dAction.Enable();
            
            mouseMoveAction = new InputAction("MouseMove", binding: "<Mouse>/delta");
            mouseMoveAction.Enable();

            lftStick = new InputAction("LeftStick", binding: "<Gamepad>/leftStick");
            lftStick.Enable();

            rgtStick = new InputAction("RightStick", binding: "<Gamepad>/rightStick");
            rgtStick.Enable();




        }




        protected override void OnUpdate()
        { 


            if (fpvCamera != null)
            {
                GameObject player = GameObject.Find("Player(Clone)");
                if (lftStick.ReadValue<Vector2>().y > 0.1f)
                {
                    player.transform.Translate(Vector3.forward * 0.05f);
                }
                if (lftStick.ReadValue<Vector2>().y < -0.1f)
                {
                    player.transform.Translate(Vector3.back * 0.05f);
                }
                if (lftStick.ReadValue<Vector2>().x > 0.1f)
                {
                    player.transform.Translate(Vector3.right * 0.025f);
                }
                if (lftStick.ReadValue<Vector2>().x < -0.1f)
                {
                    player.transform.Translate(Vector3.left * 0.025f);
                }


                if (rgtStick.ReadValue<Vector2>().x != 0 || rgtStick.ReadValue<Vector2>().y != 0)
                {
                    float x = rgtStick.ReadValue<Vector2>().x * 2f;
                    float y = rgtStick.ReadValue<Vector2>().y * -2f;
                    player.transform.Rotate(new Vector3(0, x, 0));
                    fpvCamera.transform.Rotate(new Vector3(y, 0, 0));
                }

                if (wAction.ReadValue<float>() > 0)
                {
                    player.transform.position += player.transform.forward * 0.05f;
                }
                if (aAction.ReadValue<float>() > 0)
                {
                    player.transform.position -= player.transform.right * 0.05f;
                   
                }

                if (sAction.ReadValue<float>() > 0)
                {
                    player.transform.position -= player.transform.forward * 0.05f;
                }
                if (dAction.ReadValue<float>() > 0)
                {
                    
                    player.transform.position += player.transform.right * 0.05f;
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Vector2 mouseMove = mouseMoveAction.ReadValue<Vector2>();
                float mouseX = mouseMove.x / 2;
                float mouseY = (mouseMove.y / 4) * -1;
                player.transform.Rotate(new Vector3(0, mouseX, 0));
                fpvCamera.transform.Rotate(new Vector3(mouseY, 0, 0));



            }

        }
    }
}
