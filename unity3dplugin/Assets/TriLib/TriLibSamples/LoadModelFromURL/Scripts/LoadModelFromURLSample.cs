using UnityEngine;
namespace TriLibCore.Samples
{
    /// <summary>
    /// Represents a sample that loads a compressed (Zipped) Model.
    /// </summary>
    public class LoadModelFromURLSample : MonoBehaviour
    {
        /// <summary>
        /// The Model URL.
        /// </summary>
        public string ModelURL = "https://ricardoreis.net/trilib/demos/sample/TriLibSampleModel.zip";
        public GameObject parent;

        /// <summary>
        /// Creates the AssetLoaderOptions instance, configures the Web Request, and downloads the Model.
        /// </summary>
        /// <remarks>
        /// You can create the AssetLoaderOptions by right clicking on the Assets Explorer and selecting "TriLib->Create->AssetLoaderOptions->Pre-Built AssetLoaderOptions".
        /// </remarks>
        private void Start()
        {
#if UNITY_EDITOR

            Send(ModelURL);
#endif
        }

        void Send(string message)
        {
            Debug.LogError("Bridge:" + message);
            ModelURL = message;
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var webRequest = AssetDownloader.CreateWebRequest(ModelURL);
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, parent, assetLoaderOptions);
        }

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            Debug.Log($"Loading Model. Progress: {progress:P}");
            
        }
        private void SetGameObjectRecursive(GameObject gameObject, Texture texture)
        {
            foreach (Transform child in gameObject.transform)
            {
                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                if (renderer)
                {
                    if (renderer.material.name.StartsWith("Spec"))
                    {
                        Shader shaderSpecular = Shader.Find("Standard (Specular setup)");
                        renderer.material.shader = shaderSpecular;

                        Debug.Log(child.name + ":" + renderer.material.name);
                        //www.LoadImageIntoTexture(renderer.material.mainTexture.);
                        //renderer.material.mainTexture = texture;

                        renderer.material.SetTexture("_SpecGlossMap", null);
                        renderer.material.SetColor("_SpecColor", Color.white);
                        renderer.material.SetColor("_Color", Color.black);
                        renderer.material.SetTexture("_MainTex", null);
                        renderer.material.SetTexture("_MetallicGlossMap", null);
                        renderer.material.SetFloat("_Glossiness", 1.0f);

                    }


                }

                SetGameObjectRecursive(child.gameObject, texture);
            }
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Materials loaded. Model fully loaded.");
            SetGameObjectRecursive(parent, null);
        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Model loaded. Loading materials.");
        }
    }
}
