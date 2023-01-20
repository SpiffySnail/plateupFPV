using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

using System.Linq;
using Kitchen.Modules;

namespace plateupFPV
{
    public class SetFPV : GenericSystemBase, IModSystem
    {


        InputAction f1Action;
        InputAction rmbAction;
        InputAction tAction;
        InputAction fAction;
        InputAction gAction;
        InputAction hAction;
        InputAction mouseMoveAction;
        Camera fpvCamera;

        
        protected override void Initialise()
        {
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
                GameObject cameraObject2 = new GameObject("Top Down Camera");
                Camera topDownCamera = cameraObject2.AddComponent<Camera>();
                topDownCamera.transform.localPosition = new Vector3(0, 10f, 0);
                topDownCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);
                Vector3 pos2 = new Vector3(player.transform.position.x, player.transform.position.y + 10f, 0);
                Vector3 rot = new Vector3(90, 0, 0);
                topDownCamera.transform.SetPositionAndRotation(pos2, Quaternion.Euler(rot));
                topDownCamera.fieldOfView = 60;
                topDownCamera.nearClipPlane = 0.3f;
                topDownCamera.rect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
                topDownCamera.clearFlags = CameraClearFlags.Skybox;
                topDownCamera.backgroundColor = new Color(0.5f, 0.5f, 1f);
                topDownCamera.farClipPlane = 3000f;
                
            };
            f1Action.Enable();
            
            tAction = new InputAction("t", binding: "<Keyboard>/t");
            tAction.Enable();

            fAction = new InputAction("f", binding: "<Keyboard>/f");
            fAction.Enable();

            gAction = new InputAction("g", binding: "<Keyboard>/g");
            gAction.Enable();

            hAction = new InputAction("h", binding: "<Keyboard>/h");
            hAction.Enable();

            rmbAction = new InputAction("rmb", binding: "<Mouse>/rightButton");
            rmbAction.Enable();
            
            mouseMoveAction = new InputAction("MouseMove", binding: "<Mouse>/delta");
            mouseMoveAction.Enable();



        }
                
        


        protected override void OnUpdate()
        { 
            if (tAction.ReadValue<float>() > 0)
            {
                GameObject player = GameObject.Find("Player(Clone)");
                player.transform.position += player.transform.forward * 0.05f;
            }
            if (gAction.ReadValue<float>() > 0)
            {
                GameObject player = GameObject.Find("Player(Clone)");
                player.transform.position -= player.transform.forward * 0.05f;
            }

            if (fAction.ReadValue<float>() > 0)
            {
                GameObject player = GameObject.Find("Player(Clone)");
                player.transform.position -= player.transform.right * 0.05f;
            }
            if (hAction.ReadValue<float>() > 0)
            {
                GameObject player = GameObject.Find("Player(Clone)");
                player.transform.position += player.transform.right * 0.05f;
            }

            if (fpvCamera != null)
            {
               
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                

                GameObject player = GameObject.Find("Player(Clone)");
                Vector2 mouseMove = mouseMoveAction.ReadValue<Vector2>();
                float mouseX = mouseMove.x / 2;
                float mouseY = mouseMove.y / 4;
                player.transform.Rotate(Vector3.up * mouseX, Space.World);
                player.transform.Rotate(Vector3.left * mouseY, Space.Self);




            }

        }
    }
}
