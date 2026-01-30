using NaughtyAttributes;
using UnityEngine;

namespace Snapshotter
{
	public class MaskSnapshotter : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Camera CaptureCamera;

		private Sprite _snapshotSprite;

		[Button]
		public void Snapshot()
		{
			CaptureCamera.Render();

			RenderTexture rt = CaptureCamera.targetTexture;
			RenderTexture.active = rt;

			Texture2D tex = new(rt.width, rt.height, TextureFormat.ARGB32, false);
			tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			tex.Apply();

			RenderTexture.active = null;

			Sprite snapshotSprite = Sprite.Create(
				tex,
				new Rect(0, 0, tex.width, tex.height),
				new Vector2(0.5f, 0.5f),
				100f
			);

			_snapshotSprite = snapshotSprite;
		}
	}
}
