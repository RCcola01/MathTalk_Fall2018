using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LylekGames {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class CombineMeshes : MonoBehaviour {

		private Matrix4x4 myMatrix;
        private MeshFilter myMeshFilter;
        private MeshRenderer myMeshRenderer;
        private MeshFilter[] meshFilters;

        public void Start()
        {
            //gameObject.AddComponent<MeshFilter>();
            myMeshFilter = GetComponent<MeshFilter>();
            myMeshRenderer = GetComponent<MeshRenderer>();
        }

        public void EnableMesh() {
            if(myMeshFilter == null){
                myMeshFilter = GetComponent<MeshFilter>();
            }
            if(myMeshRenderer == null){
                myMeshRenderer = GetComponent<MeshRenderer>();
            }
			myMatrix = transform.worldToLocalMatrix;
            CombineInstance[] combine;
            meshFilters = GetComponentsInChildren<MeshFilter>();
            combine = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++) {
				if (meshFilters[i].sharedMesh != null) {
					combine[i].mesh = meshFilters[i].sharedMesh;
					combine[i].transform = myMatrix * meshFilters[i].transform.localToWorldMatrix;
                    meshFilters[i].gameObject.SetActive(false);
				}
			}
            myMeshFilter.mesh = new Mesh();
            myMeshFilter.sharedMesh.CombineMeshes(combine);
            myMeshRenderer.material = meshFilters[1].GetComponent<Renderer>().sharedMaterial;
            //gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().convex = true;
            gameObject.isStatic = true;
            gameObject.SetActive(true);
        }

		public void DisableMesh() {
			for(int i = 0; i < meshFilters.Length; i++) {
                meshFilters[i].gameObject.SetActive(true);
            }
            myMeshFilter.mesh = null;
            myMeshRenderer.material = null;
            if (GetComponent<BoxCollider>())
                DestroyImmediate(gameObject.GetComponent<BoxCollider>());
        }
	}
}
