using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Text;
using System;

public class TakeScreenshotURP : MonoBehaviour {


    private bool takeScreenshot;

    private void OnEnable() {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }

    private void OnDisable() {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2) {
        if (takeScreenshot) {
            takeScreenshot = false;
            int width = 256;
            int height = 256;
            Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, width, height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            Debug.Log("CaptureScreenshot");
            //ScreenCapture.CaptureScreenshot("GameScreenshot.png");
            string imagePath = Application.persistentDataPath + "/image.png";
            StartCoroutine(CaptureScreenshot(imagePath));
            //takeScreenshot = true;
        }
    }

    public IEnumerator CaptureScreenshot(string imagePath)
    {
        yield return new WaitForEndOfFrame();
        //about to save an image capture
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);


        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        Debug.Log(" screenImage.width" + screenImage.width + " texelSize" + screenImage.texelSize);
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToJPG();

        Debug.Log(imagePath+" imagesBytes=" + imageBytes.Length);
        string encodedText = Convert.ToBase64String(imageBytes);
        Debug.Log(encodedText);

        Bridge.Instance.ShareFbScreenShot(encodedText);

        //Save image to file
        //System.IO.File.WriteAllBytes(imagePath, imageBytes);
    }

}