using NaughtyAttributes;
using UnityEngine;

namespace Snapshotter
{
	public class MaskSnapshotter : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Camera CaptureCamera;

		[BoxGroup("Settings")]
		[SerializeField] private Transform ContentRoot;

		[BoxGroup("Settings")]
		[SerializeField] private float Padding = 0.5f;

		[Button]
		public Sprite Snapshot()
		{
			if (CaptureCamera == null || ContentRoot == null)
				return null;

			if (!TryCalculateBounds(out Bounds bounds))
				return null;

			PositionCamera(bounds);
			return CaptureSprite();
		}

		private bool TryCalculateBounds(out Bounds bounds)
		{
			Renderer[] renderers = ContentRoot.GetComponentsInChildren<Renderer>();
			bounds = default;

			if (renderers.Length == 0)
				return false;

			bounds = renderers[0].bounds;
			for (int i = 1; i < renderers.Length; i++)
			{
				bounds.Encapsulate(renderers[i].bounds);
			}

			return true;
		}

		private void PositionCamera(Bounds bounds)
		{
			Vector3 center = bounds.center;

			// Push camera in front of content
			center.z = bounds.min.z - 10f;

			CaptureCamera.transform.position = center;

			float verticalSize = bounds.extents.y + Padding;
			float horizontalSize = (bounds.extents.x + Padding) / CaptureCamera.aspect;

			CaptureCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
		}

		private Sprite CaptureSprite()
		{
			CaptureCamera.Render();

			RenderTexture rt = CaptureCamera.targetTexture;
			RenderTexture.active = rt;

			Texture2D tex = new(rt.width, rt.height, TextureFormat.ARGB32, false);
			tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			tex.Apply();

			RenderTexture.active = null;

			return Sprite.Create(
				tex,
				new Rect(0, 0, tex.width, tex.height),
				new Vector2(0.5f, 0.5f),
				100f
			);
		}
	}
}
