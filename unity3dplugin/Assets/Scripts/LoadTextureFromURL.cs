using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.General;
using UnityEngine;
using UnityEngine.Networking;

public class LoadTextureFromURL : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    public void Load(TextureLoadingContext textureLoadingContext, TextureType textureType)
    {
        string fileName = System.IO.Path.GetFileName(textureLoadingContext.Texture.Filename);
        if (fileName.LastIndexOf('/') > 0)
        {
            fileName = fileName.Substring(fileName.LastIndexOf('/'));
        };

        if (fileName.LastIndexOf('\\') > 0)
        {
            fileName = fileName.Substring(fileName.LastIndexOf('\\'));
        };

        fileName = fileName.Trim('/');
        fileName = fileName.Trim('\\');
        textureLoadingContext.Texture.Filename = fileName;
        Debug.Log("ApplyDiffuseMapTexture filename:" + fileName);

        string url = textureLoadingContext.Context.BasePath + "//" + fileName;

        Debug.Log("Load:" + url);
        StartCoroutine(setImage(url, textureLoadingContext, textureType));
    }

    private void SetGameObjectRecursive(GameObject gameObject, Texture texture)
    {
        foreach (Transform child in gameObject.transform)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            if (renderer) {
                Debug.Log(child.name + ":" + renderer.material.name);
                //www.LoadImageIntoTexture(renderer.material.mainTexture.);
                renderer.material.mainTexture = texture;
            }

            SetGameObjectRecursive(child.gameObject, texture);
        }
    }

    void ApplyTexture(Texture texture)
    {

        SetGameObjectRecursive(parent, texture);
    }

    IEnumerator setImage(string url, TextureLoadingContext textureLoadingContext, TextureType textureType)
    {
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            textureLoadingContext.UnityTexture = ((UnityEngine.Networking.DownloadHandlerTexture)www.downloadHandler).texture;
            if (textureLoadingContext.UnityTexture != null)
            {
                //textureLoadingContext.Context.AddUsedTexture(textureLoadingContext.UnityTexture);
                //textureLoadingContext.MaterialMapperContext.VirtualMaterial.SetProperty("_MainTex", textureLoadingContext.UnityTexture, GenericMaterialProperty.DiffuseMap);
                Debug.LogError("setImage texture:" + url);
                
                foreach (var dictObj in textureLoadingContext.Context.GameObjects) {
                    GameObject obj = dictObj.Value;
                    MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                    
                    if (renderer)
                    {                        
                        
                        Debug.Log(obj.name + ":" + renderer.material.name);
                        string matName = (textureLoadingContext.MaterialMapperContext.Material.Name + " (Instance)");
                        if (renderer.material.name == matName)
                        {
                            if (textureType == TextureType.Diffuse)
                            {
                                renderer.material.mainTexture = textureLoadingContext.UnityTexture;// ((UnityEngine.Networking.DownloadHandlerTexture)www.downloadHandler).texture;
                            }
                            else if (textureType == TextureType.Metalness)
                            {
                                float value = textureLoadingContext.MaterialMapperContext.Material.GetFloatValue("ReflectionFactor");
                                renderer.material.SetFloat("_GlossMapScale", value);
                                renderer.material.SetTexture("_MetallicGlossMap", textureLoadingContext.UnityTexture);
                            }
                            else if (textureType == TextureType.NormalMap)
                            {
                                renderer.material.SetTexture("_BumpMap", textureLoadingContext.UnityTexture);
                            }


                        }
                    }
                }
            }
        }
    }
}
