using Mask;
using NaughtyAttributes;
using UnityEngine;

namespace Snapshotter
{
	public class MaskSnapshotter : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Camera CaptureCamera;

		[BoxGroup("References")]
		[SerializeField] private Transform ContentRoot;

		[BoxGroup("Settings")]
		[SerializeField] private float Padding = 0.5f;

		[Button]
		public Sprite Snapshot()
		{
			if (CaptureCamera == null || ContentRoot == null)
				return null;

			if (!TryGetBaseMask(out MaskComponent baseMask))
				return null;

			if (!TryCalculateContentBounds(out Bounds contentBounds))
				return null;

			PositionCamera(baseMask.transform.position, contentBounds);
			return CaptureSprite();
		}

		private bool TryGetBaseMask(out MaskComponent baseMask)
		{
			baseMask = null;

			MaskComponent[] components = ContentRoot.GetComponentsInChildren<MaskComponent>();
			foreach (MaskComponent component in components)
			{
				if (component.GetMaskComponentType() != MaskComponentType.Base)
					continue;

				baseMask = component;
				return true;
			}

			return false;
		}

		private bool TryCalculateContentBounds(out Bounds bounds)
		{
			bounds = default;

			SpriteRenderer[] sprites =
				ContentRoot.GetComponentsInChildren<SpriteRenderer>();

			if (sprites.Length == 0)
				return false;

			bounds = sprites[0].bounds;
			for (int i = 1; i < sprites.Length; i++)
			{
				bounds.Encapsulate(sprites[i].bounds);
			}

			return true;
		}


		private void PositionCamera(Vector3 baseCenter, Bounds contentBounds)
		{
			Vector3 cameraPosition = baseCenter;
			cameraPosition.z = CaptureCamera.transform.position.z;

			CaptureCamera.transform.position = cameraPosition;

			float verticalSize = contentBounds.extents.y + Padding;
			float horizontalSize = (contentBounds.extents.x + Padding) / CaptureCamera.aspect;

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
